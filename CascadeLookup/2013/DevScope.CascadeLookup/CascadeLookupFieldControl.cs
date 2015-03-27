using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevScope.CascadeLookup.Framework.Helpers;
using DevScope.CascadeLookup.Common;
using DevScope.CascadeLookup.Framework.Loggers;

namespace DevScope.CascadeLookup
{
    /// <summary>
    /// This class represents the cascade lookup field control
    /// </summary>
    public class CascadeLookupFieldControl : BaseFieldControl
    {
        #region Controls

        /// <summary>
        /// Gets or sets the dropdown list cascade.
        /// </summary>
        /// <value>
        /// The dropdown list cascade.
        /// </value>
        protected DropDownList ddlCascade { get; set; }

        /// <summary>
        /// Gets or sets the hyperlink cascade control.
        /// </summary>
        /// <value>
        /// The hyperlink cascade control.
        /// </value>
        protected HyperLink hypCascade { get; set; }

        /// <summary>
        /// Gets or sets the hidden field cascade control.
        /// </summary>
        /// <value>
        /// The the hidden field cascade control.
        /// </value>
        protected HiddenField hfCascade { get; set; }


        /// <summary>
        /// Gets or sets the dummy button cascade control.
        /// </summary>
        /// <value>
        /// The the dummy button cascade control.
        /// </value>
        protected Button btnCascade { get; set; }

        #endregion

        /// <summary>
        /// Gets the name of the default rendering template.
        /// </summary>
        protected override string DefaultTemplateName
        {
            get
            {
                return base.ControlMode == SPControlMode.Display ? this.DisplayTemplateName : "CascadeLookupFieldEditor";
            }
        }

        /// <summary>
        /// Gets or sets the name of the template that can be used to control the rendering of the <see cref="T:Microsoft.SharePoint.WebControls.BaseFieldControl" /> object in display mode; that is, when it is not on a New or Edit form.
        /// </summary>
        public override string DisplayTemplateName
        {
            get { return "CascadeLookupFieldDisplay"; }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            // Only for postbacks and new/edit - repopulate dependant cascades
            if (Page.IsPostBack && (base.ControlMode == SPControlMode.New || base.ControlMode == SPControlMode.Edit))
                SetupEditTemplateControlsPostback();
        }

        /// <summary>
        /// Creates any child controls necessary to render the field, such as a label control, link control, or text box control.
        /// </summary>
        protected override void CreateChildControls()
        {
            //If the field we are working on is null then exit and do nothing
            if (base.Field == null)
            {
                return;
            }

            base.CreateChildControls();

            //Now instantiate the control instance variables with the controls defined in the rendering templates.
            InstantiateMemberControls();

            if (base.ControlMode == SPControlMode.Display)
            {
                SetupDisplayTemplateControls();
            }
            else
            {
                SetupEditTemplateControls();
            }
        }

        public override object Value
        {
            //This is called at begining and editing of a list item. This means we only have to deal with
            //the editing template. Our field is based on the Lookup field so we will need to deal with SPFieldLookupValue.
            get
            {
                EnsureChildControls();

                if (ddlCascade.SelectedItem == null)
                {
                    //Returning null here will cause SharePoint to NOT call the GetValidatedString() method
                    //in our field type class. Return an empty UrlValue instead.
                    return new SPFieldLookupValue();
                }

                int value = 0;
                if (int.TryParse(ddlCascade.SelectedValue, out value))
                    return new SPFieldLookupValue(value, ddlCascade.SelectedItem.Text);
                else
                    return new SPFieldLookupValue();
            }
            set
            {
                EnsureChildControls();

                var lookupValue = (SPFieldLookupValue)value;
                if (lookupValue != null)
                {
                    if (ddlCascade.Items.FindByValue(lookupValue.LookupId.ToString()) != null)
                    {
                        ddlCascade.SelectedValue = hfCascade.Value = lookupValue.LookupId.ToString();
                    }
                }
                else
                {
                    // if is not null and is required adds first value in dropdownlist
                    if (base.Field.Required && ddlCascade.Items.Count > 0)
                    {
                        ddlCascade.Items[0].Selected = true;
                        hfCascade.Value = ddlCascade.SelectedValue;
                    }
                }
            }
        }

        #region Private Methods

        private void InstantiateMemberControls()
        {
            if (base.ControlMode == SPControlMode.Display)
            {
                //Display
                hypCascade = (HyperLink)base.TemplateContainer.FindControl("hypCascade");
            }
            else
            {
                // Create/Edit
                ddlCascade = (DropDownList)base.TemplateContainer.FindControl("ddlCascade");
                hfCascade = (HiddenField)base.TemplateContainer.FindControl("hfCascade");
                btnCascade = (Button)base.TemplateContainer.FindControl("btnCascade");

                // set title attribute with field title
                ddlCascade.Attributes.Add("title", base.Field.Title);

                // set autopostback to dropdowns that have child dependency and update hidden field to all
                bool hasChildDependecy = base.Fields.OfType<CascadeLookupFieldType>()
                    .Count(x => (string)x.GetCustomProperty(Constants.EditorDependencyListColumnProperty) == base.Field.InternalName) > 0;

                if (hasChildDependecy)
                    ddlCascade.Attributes.Add("onchange", string.Format("document.getElementById('{0}').value = this.value;document.getElementById('{1}').click()"
                        , hfCascade.ClientID
                        , btnCascade.ClientID));
                else
                    ddlCascade.Attributes.Add("onchange", string.Format("document.getElementById('{0}').value = this.value;"
                        , hfCascade.ClientID));
            }
        }

        /// <summary>
        /// Setups the display template controls.
        /// </summary>
        private void SetupDisplayTemplateControls()
        {
            bool isFieldValueSpecified = base.ItemFieldValue != null;

            if (isFieldValueSpecified)
            {
                try
                {
                    // get relationship list
                    string listGuidProperty = (string)base.Field.GetCustomProperty(Constants.EditorListGuidProperty);

                    SPList list = base.Web.Lists[new Guid(listGuidProperty)];
                    if (list == null)
                        return;

                    SPFieldLookupValue value = (SPFieldLookupValue)base.ItemFieldValue;
                    hypCascade.Text = value.LookupValue;
                    hypCascade.Target = "_blank";
                    hypCascade.NavigateUrl = string.Format("{0}/DispForm.aspx?ID={1}",
                        list.RootFolder.ServerRelativeUrl,
                        value.LookupId);
                }
                catch (Exception ex)
                {
                    SharePointLogger.LogError(ex);
                }
            }
        }

        /// <summary>
        /// Setups the edit template controls.
        /// </summary>
        private void SetupEditTemplateControls()
        {
            bool hasDependecy = Convert.ToBoolean((int)base.Field.GetCustomProperty(Constants.EditorDependenciesProperty));
            string listGuidProperty = (string)base.Field.GetCustomProperty(Constants.EditorListGuidProperty);
            string dependencyColumnProperty = (string)base.Field.GetCustomProperty(Constants.EditorDependencyColumnProperty);
            string listColumnProperty = (string)base.Field.GetCustomProperty(Constants.EditorListColumnProperty);
            string dependencyListColumnProperty = (string)base.Field.GetCustomProperty(Constants.EditorDependencyListColumnProperty);

            try
            {
                List<ListItem> items = new List<ListItem>();

                // get items from list
                SPList list = base.Web.Lists[new Guid(listGuidProperty)];
                if (list == null)
                    return;

                // if has no dependency
                if (!hasDependecy)
                {
                    // add items
                    AddItems(list, null, null, false);

                    // if is not null and is required adds first value in dropdownlist
                    if (base.Field.Required && ddlCascade.Items.Count > 0)
                    {
                        ddlCascade.Items[0].Selected = true;
                        hfCascade.Value = ddlCascade.SelectedValue;
                    }
                }
                else
                {
                    // fill values based on parent
                    CascadeLookupFieldControl dependencyControl = base.RenderContext.FormContext.FieldControlCollection
                        .OfType<DevScope.CascadeLookup.CascadeLookupFieldControl>()
                        .FirstOrDefault(x => x.Field.InternalName == dependencyListColumnProperty);

                    // gets the value from this list (cascade list) dependency
                    int dependencyListColumnValue = dependencyControl.Value != null
                        ? ((SPFieldLookupValue)dependencyControl.Value).LookupId
                        : 0;

                    AddItems(list, dependencyListColumnValue, dependencyColumnProperty, true);

                    // if is not null and is required adds first value in dropdownlist
                    if (base.Field.Required && ddlCascade.Items.Count > 0)
                    {
                        ddlCascade.Items[0].Selected = true;
                        hfCascade.Value = ddlCascade.SelectedValue;
                    }
                }
            }
            catch (Exception ex)
            {
                SharePointLogger.LogError(ex);
            }
        }

        private void SetupEditTemplateControlsPostback()
        {
            bool hasDependecy = Convert.ToBoolean((int)base.Field.GetCustomProperty(Constants.EditorDependenciesProperty));
            string listGuidProperty = (string)base.Field.GetCustomProperty(Constants.EditorListGuidProperty);
            string dependencyColumnProperty = (string)base.Field.GetCustomProperty(Constants.EditorDependencyColumnProperty);
            string listColumnProperty = (string)base.Field.GetCustomProperty(Constants.EditorListColumnProperty);
            string dependencyListColumnProperty = (string)base.Field.GetCustomProperty(Constants.EditorDependencyListColumnProperty);

            if (hasDependecy)
            {
                try
                {
                    List<ListItem> items = new List<ListItem>();

                    // get items from list
                    SPList list = base.Web.Lists[new Guid(listGuidProperty)];
                    if (list == null)
                        return;

                    // fill values based on parent
                    CascadeLookupFieldControl dependencyControl = base.RenderContext.FormContext.FieldControlCollection
                        .OfType<DevScope.CascadeLookup.CascadeLookupFieldControl>()
                        .FirstOrDefault(x => x.Field.InternalName == dependencyListColumnProperty);

                    // gets the value from this list (cascade list) dependency
                    int dependencyListColumnValue = 0;
                    int.TryParse(dependencyControl.hfCascade.Value, out dependencyListColumnValue);

                    // add items
                    AddItems(list, dependencyListColumnValue, dependencyColumnProperty, true);

                    // set selected value if exists
                    if (ddlCascade.Items.FindByValue(hfCascade.Value) != null)
                        ddlCascade.SelectedValue = hfCascade.Value;
                    else
                    {
                        // choose first in case is required and there is no value
                        if (base.Field.Required && ddlCascade.Items.Count > 0)
                        {
                            ddlCascade.Items[0].Selected = true;
                            hfCascade.Value = ddlCascade.SelectedValue;
                        }
                        else
                            hfCascade.Value = string.Empty;
                    }
                }
                catch (Exception ex)
                {
                    SharePointLogger.LogError(ex);
                }
            }
        }

        /// <summary>
        /// Adds the items.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <param name="filterID">The filter identifier.</param>
        /// <param name="filterColumn">The filter column.</param>
        private void AddItems(SPList list, int? filterID, string filterColumn, bool filterRequired)
        {
            ddlCascade.Items.Clear();

            // query
            SPQuery query = new SPQuery()
            {
                Query = filterRequired
                ? string.Format(@"<Where>
                        <And>
                            <Neq><FieldRef Name='ContentType' /><Value Type='Text'>Folder</Value></Neq>
                            <Eq><FieldRef Name='{0}' LookupId='True' /><Value Type='Lookup'>{1}</Value></Eq>
                        </And>
                    </Where>", filterColumn, filterID)
                : "<Where><Neq><FieldRef Name='ContentType' /><Value Type='Text'>Folder</Value></Neq></Where>",
                ViewAttributes = "Scope=\"Recursive\""
            };

            SPListItemCollection items = list.GetItems(query);

            // only adds empty item if field is not required and has values
            if (!base.Field.Required && items.Count > 0)
                ddlCascade.Items.Add(new ListItem(Framework.SharePoint.Resources.GetLocalizedString("EditorEmptyCascadeItem",
                    "CascadeLookupResources", (uint)SPContext.Current.Web.UICulture.LCID), string.Empty));

            // add the rest of items
            foreach (SPListItem item in items)
                ddlCascade.Items.Add(new ListItem(item.Title, item.ID.ToString()));
        }

        #endregion
    }
}
