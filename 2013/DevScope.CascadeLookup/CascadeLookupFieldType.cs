using DevScope.CascadeLookup.Common;
using DevScope.CascadeLookup.Framework.SharePoint;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Xml;

namespace DevScope.CascadeLookup
{
    /// <summary>
    /// This class represents the cascade lookup field type
    /// </summary>
    public class CascadeLookupFieldType : SPFieldLookup
    {
        #region Properties

        /// <summary>
        /// Gets or sets the cascade render mode.
        /// </summary>
        /// <value>
        /// The cascade render mode.
        /// </value>
        CascadeModeEnum CascadeRenderMode { get; set; }

        public Guid WebSourceId
        {
            get { return GetThreadDataValue("Thread_WebSourceId") != Guid.Empty ? GetThreadDataValue("Thread_WebSourceId") : LookupWebId; }
            set { SetThreadDataValue("Thread_WebSourceId", value); }
        }

        public Guid LookupListId
        {
            get { return GetThreadDataValue("Thread_LookupListId") != Guid.Empty ? GetThreadDataValue("Thread_LookupListId") : new Guid(LookupList); }
            set { SetThreadDataValue("Thread_LookupListId", value); }
        }

        public Guid DisplayColumnId
        {
            get { return GetThreadDataValue("Thread_DisplayColumnId") != Guid.Empty ? GetThreadDataValue("Thread_DisplayColumnId") : new Guid(LookupField); }
            set { SetThreadDataValue("Thread_DisplayColumnId", value); }
        }

        //Point to a dummy js file.
        private const string JSLinkUrl = "/_layouts/15/DevScope.CascadeLookup/Js/dummy.js";

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="CascadeLookupFieldType"/> class.
        /// </summary>
        /// <param name="fields">An <see cref="T:Microsoft.SharePoint.SPFieldCollection" /> object that represents the parent field collection.</param>
        /// <param name="fieldName">A string that contains the name of the field.</param>
        public CascadeLookupFieldType(SPFieldCollection fields, string fieldName)
            : base(fields, fieldName)
        {
            GetRenderMode();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CascadeLookupFieldType"/> class.
        /// </summary>
        /// <param name="fields">An <see cref="T:Microsoft.SharePoint.SPFieldCollection" /> object that represents the parent field collection.</param>
        /// <param name="typeName">A string that contains the type name.</param>
        /// <param name="displayName">A string that contains the display name of the field.</param>
        public CascadeLookupFieldType(SPFieldCollection fields, string typeName, string displayName)
            : base(fields, typeName, displayName)
        {
            GetRenderMode();
        }

        /// <summary>
        /// Gets the control that is used to render the field.
        /// </summary>
        public override BaseFieldControl FieldRenderingControl
        {
            get
            {
                switch (this.CascadeRenderMode)
                {
                    case CascadeModeEnum.SERVER:
                        return new CascadeLookupFieldControl() { FieldName = base.InternalName };
                    case CascadeModeEnum.CLIENT:
                        return new CascadeLookupClientFieldControl() { FieldName = base.InternalName };
                    default:
                        return new CascadeLookupFieldControl() { FieldName = base.InternalName };
                }
            }
        }

        /// <summary>
        /// Occurs after a field is added.
        /// </summary>
        /// <param name="op">An <see cref="T:Microsoft.SharePoint.SPAddFieldOptions" /> value that specifies an option that is implemented after the field is created.</param>
        public override void OnAdded(SPAddFieldOptions op)
        {
            /*We will need to update the field again after it is added to save the custom setting properties. For more
             * info see http://msdn.microsoft.com/en-us/library/cc889345(office.12).aspx#CreatingWSS3CustomFields_StoringFieldSetting */

            base.OnAdded(op);
            this.Update();
        }

        /// <summary>
        /// Updates the database with changes that are made to the field.
        /// </summary>
        public override void Update()
        {
            object listGuid = Thread.GetData(Thread.GetNamedDataSlot(Constants.EditorListGuidProperty));
            base.SetCustomProperty(Constants.EditorListGuidProperty, listGuid);

            object columnName = Thread.GetData(Thread.GetNamedDataSlot(Constants.EditorListColumnProperty));
            base.SetCustomProperty(Constants.EditorListColumnProperty, columnName);

            base.SetCustomProperty(Constants.EditorDependenciesProperty,
                Thread.GetData(Thread.GetNamedDataSlot(Constants.EditorDependenciesProperty)));

            base.SetCustomProperty(Constants.EditorDependencyColumnProperty,
               Thread.GetData(Thread.GetNamedDataSlot(Constants.EditorDependencyColumnProperty)));

            base.SetCustomProperty(Constants.EditorDependencyListColumnProperty,
               Thread.GetData(Thread.GetNamedDataSlot(Constants.EditorDependencyListColumnProperty)));

            // Update base lookup field properties
            XmlDocument document = new XmlDocument();
            document.LoadXml(SchemaXml);

            SPList list = base.ParentList.ParentWeb.Lists[new Guid((string)listGuid)];
            WebSourceId = base.ParentList.ParentWeb.ID;
            LookupListId = list.ID;
            DisplayColumnId = list.Fields.GetFieldByInternalName((string)columnName).Id;
            UpdateProperty(document, "WebId", WebSourceId);
            UpdateProperty(document, "List", LookupListId);
            UpdateProperty(document, "ShowField", DisplayColumnId);

            SchemaXml = document.OuterXml;

            base.Update();
            FreeThreadData();
        }

        /// <summary>
        /// Used for data serialization logic and for field validation logic that is specific to a custom field type to convert the field value object into a validated, serialized string.
        /// </summary>
        /// <param name="value">An object that represents the value object to convert.</param>
        /// <returns>
        /// A string that serializes the value object.
        /// </returns>
        /// <exception cref="Microsoft.SharePoint.SPFieldValidationException"></exception>
        public override string GetValidatedString(object value)
        {
            if (!base.Required)
            {
                return base.GetValidatedString(value);
            }

            var lookupValue = value as SPFieldLookupValue;
            if (lookupValue == null || lookupValue.LookupId == 0)
            {
                throw new SPFieldValidationException(string.Format(Framework.SharePoint.Resources.GetLocalizedString("EditorRequiredFieldMessage"
                , "CascadeLookupResources", (uint)SPContext.Current.Web.UICulture.LCID), base.Title));
            }

            return base.GetValidatedString(value);
        }

        /// <summary>
        /// JSLink
        /// </summary>
        public override string JSLink
        {
            get
            {
                if (SPContext.Current.FormContext.FormMode != SPControlMode.Invalid)
                    return base.JSLink;
                else
                    return JSLinkUrl;
            }
            set
            {
                base.JSLink = value;
            }
        }

        #region Private Methods

        /// <summary>
        /// Gets the render mode.
        /// </summary>
        /// <returns></returns>
        private void GetRenderMode()
        {
            // get from list property bag
            string mode = base.ParentList.RootFolder.Properties.ContainsKey(Constants.CascadeModePropertyBag)
                ? base.ParentList.RootFolder.Properties[Constants.CascadeModePropertyBag] + string.Empty
                : string.Empty;

            this.CascadeRenderMode = String.IsNullOrEmpty(mode)
            ? CascadeModeEnum.CLIENT
            : (CascadeModeEnum)int.Parse(mode);
        }

        private Guid GetThreadDataValue(string propertyName)
        {
            LocalDataStoreSlot slot = Thread.GetNamedDataSlot(propertyName);
            object dataSlot = Thread.GetData(slot);

            return dataSlot != null ? (Guid)dataSlot : Guid.Empty;
        }

        private void SetThreadDataValue(string propertyName, object value)
        {
            Thread.SetData(Thread.GetNamedDataSlot(propertyName), value);
        }

        private void FreeThreadData()
        {
            Thread.FreeNamedDataSlot("Thread_WebSourceId");
            Thread.FreeNamedDataSlot("Thread_LookupListId");
            Thread.FreeNamedDataSlot("Thread_DisplayColumnId");
        }

        /// <summary>
        /// Updates the property.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        private void UpdateProperty(XmlDocument document, string name, object value)
        {
            if (document == null || document.DocumentElement == null)
                return;

            XmlAttribute attribute = document.DocumentElement.Attributes[name] ?? document.CreateAttribute(name);
            attribute.Value = value.ToString();

            document.DocumentElement.Attributes.Append(attribute);
        }

        #endregion
    }
}
