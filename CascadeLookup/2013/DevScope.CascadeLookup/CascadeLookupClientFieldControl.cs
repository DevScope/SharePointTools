using DevScope.CascadeLookup.Common;
using DevScope.CascadeLookup.Framework.Helpers;
using DevScope.CascadeLookup.Framework.Loggers;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DevScope.CascadeLookup
{
    public class CascadeLookupClientFieldControl : BaseFieldControl
    {
        #region Controls

        /// <summary>
        /// Gets or sets the hyperlink cascade control.
        /// </summary>
        /// <value>
        /// The hyperlink cascade control.
        /// </value>
        protected HyperLink hypCascade { get; set; }

        /// <summary>
        /// Gets or sets the cascade content panel control.
        /// </summary>
        /// <value>
        /// The cascade cascade content panel control.
        /// </value>
        protected Panel pnlContent { get; set; }

        /// <summary>
        /// Gets or sets the cascade hidden field control.
        /// </summary>
        /// <value>
        /// The the cascade hidden field control.
        /// </value>
        protected HiddenField hfCascade { get; set; }

        #endregion

        /// <summary>
        /// Gets the name of the default rendering template.
        /// </summary>
        protected override string DefaultTemplateName
        {
            get
            {
                return base.ControlMode == SPControlMode.Display ? this.DisplayTemplateName : "CascadeLookupFieldClientEditor";
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
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            // load script if is in new or edit mode
            if (base.ControlMode == SPControlMode.New || base.ControlMode == SPControlMode.Edit)
            {
                ScriptManager currentSM = ScriptManager.GetCurrent(this.Page);
                
                string cssBlockName = "DVCLDECLARECSS";
                this.Page.RegisterCSS(new Uri(string.Format("{0}/_layouts/15/DevScope.CascadeLookup/css/", SPContext.Current.Web.Url)),
                    cssBlockName, "cascadeLookup", false);
                
                if (!currentSM.IsClientScriptBlockRegistered(cssBlockName))
                    currentSM.RegisterScriptFile(new Uri(string.Format("{0}/_layouts/15/DevScope.CascadeLookup/css/", SPContext.Current.Web.Url)),
                        cssBlockName, "cascadeLookup", false);
                                
                string scriptBlockName = "DVCLDECLAREJS";
                if (!currentSM.IsClientScriptBlockRegistered(scriptBlockName))
                    currentSM.RegisterScriptFile(new Uri(string.Format("{0}/_layouts/15/DevScope.CascadeLookup/Js/", SPContext.Current.Web.Url)),
                        scriptBlockName, "cascadeLookup", false);

                scriptBlockName = "DVCLINITIALIZESCRIPT";
                if (!currentSM.IsClientScriptBlockRegistered(scriptBlockName))
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), scriptBlockName, "_spBodyOnLoadFunctionNames.push('loadDVCLDropdowns');", true);
            }
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

                //Returning null here will cause SharePoint to NOT call the GetValidatedString() method
                //in our field type class. Return an empty UrlValue instead.
                if (!String.IsNullOrEmpty(hfCascade.Value))
                    return new SPFieldLookupValue(hfCascade.Value).LookupId + string.Empty;
                return string.Empty;
            }
            set
            {
                EnsureChildControls();

                var lookupValue = (SPFieldLookupValue)value;
                if (lookupValue != null)
                    hfCascade.Value = lookupValue.LookupId + string.Empty;
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
                pnlContent = (Panel)base.TemplateContainer.FindControl("pnlContent");
                hfCascade = (HiddenField)base.TemplateContainer.FindControl("hfCascade");
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
                // add properties to pnlContent
                pnlContent.Attributes.Add("data-fieldid", base.Field.Id.ToString());
                pnlContent.Attributes.Add("data-dependency", hasDependecy ? "true" : "false");
                pnlContent.Attributes.Add("data-listguid", listGuidProperty);
                pnlContent.Attributes.Add("data-listcolumn", listColumnProperty);
                pnlContent.Attributes.Add("data-dependencycolumn", dependencyColumnProperty);
                pnlContent.Attributes.Add("data-dependencylistcolumn", dependencyListColumnProperty);
                pnlContent.Attributes.Add("data-required", base.Field.Required ? "true" : "false");

                SPFieldLookupValue fieldValue = (SPFieldLookupValue)base.ItemFieldValue;
                pnlContent.Attributes.Add("data-selectedvalue", fieldValue == null ? string.Empty : fieldValue.LookupId.ToString());
            }
            catch (Exception ex)
            {
                SharePointLogger.LogError(ex);
            }
        }

        #endregion
    }
}
