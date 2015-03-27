using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System.Web.UI.WebControls;
using DevScope.CascadeLookup.Common;

namespace DevScope.CascadeLookup.Layouts.Pages
{
    public partial class CascadeLookupConfig : LayoutsPageBase
    {
        #region Properties

        /// <summary>
        /// Gets or sets the list.
        /// </summary>
        /// <value>
        /// The list.
        /// </value>
        SPList List { get; set; }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            SetResources();

            // get list
            if (Request.QueryString["listID"] != null)
                this.List = SPContext.Current.Web.Lists[new Guid(Request.QueryString["listID"])];
            else
            {
                CloseDialog();
                return;
            }

            if (!IsPostBack)
            {
                // get from list property bag
                string mode = this.List.RootFolder.Properties.ContainsKey(Constants.CascadeModePropertyBag)
                    ? this.List.RootFolder.Properties[Constants.CascadeModePropertyBag] + string.Empty
                    : ((int)CascadeModeEnum.SERVER).ToString();

                // fill radiobutton mode
                rbCascadeMode.Items.Add(new ListItem("Server", ((int)CascadeModeEnum.SERVER).ToString()));
                rbCascadeMode.Items.Add(new ListItem("Client", ((int)CascadeModeEnum.CLIENT).ToString()));
                rbCascadeMode.SelectedValue = mode;
            }
        }

        #region Private Methods

        private void SetResources()
        {
            string resourceFile = "CascadeLookupResources";
            uint lcid = (uint)SPContext.Current.Web.UICulture.LCID;
            ltrTitle.Text = Framework.SharePoint.Resources.GetLocalizedString("ConfigCascadeTitle", resourceFile, lcid);
            ltrCascadModeLabel.Text = Framework.SharePoint.Resources.GetLocalizedString("ConfigCascadeModeLabel", resourceFile, lcid);
            btnSave.Text = Framework.SharePoint.Resources.GetLocalizedString("ConfigCascadeModeSaveButtonText", resourceFile, lcid);
        }

        /// <summary>
        /// Closes the dialog.
        /// </summary>
        private void CloseDialog()
        {
            this.Page.ClientScript.RegisterStartupScript(this.GetType(), "PopupScript", "SP.UI.ModalDialog.commonModalDialogClose(0, 0);", true);
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Handles the Click event of the btnSave control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!this.List.RootFolder.Properties.ContainsKey(Constants.CascadeModePropertyBag))
                this.List.RootFolder.Properties.Add(Constants.CascadeModePropertyBag, rbCascadeMode.SelectedValue);
            else
                this.List.RootFolder.Properties[Constants.CascadeModePropertyBag] = rbCascadeMode.SelectedValue;

            this.List.RootFolder.Update();

            CloseDialog();
        }

        #endregion
    }
}
