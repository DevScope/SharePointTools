using DevScope.CascadeLookup.Common;
using DevScope.CascadeLookup.Framework.Loggers;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace DevScope.CascadeLookup.CONTROLTEMPLATES
{
    /// <summary>
    /// This control represents the additional properties when creating the field in the list
    /// </summary>
    public partial class CascadeLookupFieldEditor : UserControl, IFieldEditor
    {
        #region Properties

        Guid[] FieldExceptions = new Guid[] { 
            SPBuiltInFieldId.Title, 
            SPBuiltInFieldId.FileLeafRef,
            SPBuiltInFieldId.Created,
            SPBuiltInFieldId.Created_x0020_By,
            SPBuiltInFieldId.Modified,
            SPBuiltInFieldId.Modified_x0020_By};

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            SetResources();
        }

        public bool DisplayAsNewSection
        {
            get { return false; }
        }

        public void InitializeWithField(SPField field)
        {
            try
            {
                //In this method we will sync the custom settings of the field with
                //our custom setting controls on the editor screen. Field however will be null when we are in create mode, in
                //which case the default value (as specified in the fldtypes XML will apply).

                // get properties
                string listGuidPropertyValue = field != null
                    ? (string)field.GetCustomProperty(Constants.EditorListGuidProperty)
                    : string.Empty;

                string listColumnPropertyValue = field != null
                    ? (string)field.GetCustomProperty(Constants.EditorListColumnProperty)
                    : string.Empty;

                bool listColumnDependenciesValue = field != null
                    ? Convert.ToBoolean((int)field.GetCustomProperty(Constants.EditorDependenciesProperty))
                    : false;

                string dependencyColumnPropertyValue = field != null
                    ? (string)field.GetCustomProperty(Constants.EditorDependencyColumnProperty)
                    : string.Empty;

                string dependencyListColumnPropertyValue = field != null
                    ? (string)field.GetCustomProperty(Constants.EditorDependencyListColumnProperty)
                    : string.Empty;

                if (!Page.IsPostBack)
                {
                    // get lists that are not hidden
                    List<SPList> lists = SPContext.Current.Web.Lists.OfType<SPList>().Where(x => !x.Hidden).ToList();
                    if (lists != null && lists.Count > 0)
                    {
                        foreach (SPList list in lists)
                        {
                            ListItem item = new ListItem(list.Title, list.ID.ToString());
                            if (item.Value == listGuidPropertyValue)
                                item.Selected = true;
                            ddlCascadeList.Items.Add(item);
                        }

                        // set selected column from the selected or first list
                        ChangeColumn(ddlCascadeList, ddlCascadeListColumn, listColumnPropertyValue);
                        // set selected lookup column from the selected or first list
                        ChangeColumn(ddlCascadeList, ddlCascadeDependencyColumn, dependencyColumnPropertyValue);
                    }

                    chkCascadeDependencies.Checked = listColumnDependenciesValue;
                    tblDependencies.Visible = listColumnDependenciesValue;

                    // get dependency list columns - columns from this type in this list
                    List<SPField> cascadeFields = SPContext.Current.List.Fields.OfType<SPField>().Where(x => x.TypeAsString == Constants.CascadeLookupFieldType).ToList();
                    if (cascadeFields != null && cascadeFields.Count > 0)
                        foreach (SPField cascadeField in cascadeFields)
                        {
                            // ignore current field
                            if (field != null && field.InternalName == cascadeField.InternalName)
                                continue;

                            ListItem item = new ListItem(cascadeField.Title, cascadeField.InternalName);
                            if (cascadeField.InternalName == dependencyListColumnPropertyValue)
                                item.Selected = true;
                            ddlCascadeDependencyListColumn.Items.Add(item);
                        }
                }
            }
            catch (Exception ex)
            {
                lblError.Visible = true;
                SharePointLogger.LogError(ex);
            }
        }

        public void OnSaveChange(SPField field, bool isNewField)
        {
            try
            {
                /*This is perhaps the most tricky part in implementing custom field type properties.
                * The field param passed in to this method is a different object instance to the actual field being edited.
                * This is why we'll need to set the value to be saved into the LocalThreadStorage, and retrieve it back out
                * in the FieldType class and update the field with the custom setting properties. For more info see
                * http://msdn.microsoft.com/en-us/library/cc889345(office.12).aspx#CreatingWSS3CustomFields_StoringFieldSetting */

                Thread.SetData(Thread.GetNamedDataSlot(Constants.EditorListGuidProperty), ddlCascadeList.SelectedValue);
                Thread.SetData(Thread.GetNamedDataSlot(Constants.EditorListColumnProperty), ddlCascadeListColumn.SelectedValue);
                Thread.SetData(Thread.GetNamedDataSlot(Constants.EditorDependenciesProperty), chkCascadeDependencies.Checked ? 1 : 0);
                Thread.SetData(Thread.GetNamedDataSlot(Constants.EditorDependencyColumnProperty), chkCascadeDependencies.Checked ? ddlCascadeDependencyColumn.SelectedValue : string.Empty);
                Thread.SetData(Thread.GetNamedDataSlot(Constants.EditorDependencyListColumnProperty), chkCascadeDependencies.Checked ? ddlCascadeDependencyListColumn.SelectedValue : string.Empty);
            }
            catch (Exception ex)
            {
                lblError.Visible = true;
                SharePointLogger.LogError(ex);
            }
        }

        #region Private Methods

        private void SetResources()
        {
            string resourceFile = "CascadeLookupResources";
            uint lcid = (uint)SPContext.Current.Web.UICulture.LCID;
            ltrListLabel.Text = Framework.SharePoint.Resources.GetLocalizedString("EditorListLabel", resourceFile, lcid);
            ltrColumnLabel.Text = Framework.SharePoint.Resources.GetLocalizedString("EditorColumnLabel", resourceFile, lcid);
            ltrDependenciesLabel.Text = Framework.SharePoint.Resources.GetLocalizedString("EditorDependenciesLabel", resourceFile, lcid);
            ltrDependencyListColumnLabel.Text = Framework.SharePoint.Resources.GetLocalizedString("EditorDependencyListColumnLabel", resourceFile, lcid);
            ltrDependencyColumnLabel.Text = Framework.SharePoint.Resources.GetLocalizedString("EditorDependencyColumnLabel", resourceFile, lcid);
            lblError.Text = Framework.SharePoint.Resources.GetLocalizedString("EditorErrorLabel", resourceFile, lcid);
        }

        /// <summary>
        /// Changes the column.
        /// </summary>
        private void ChangeColumn(DropDownList ddl, DropDownList ddlChild, string column)
        {
            if (ddl.Items.Count == 0)
                return;

            // clear items
            ddlChild.Items.Clear();

            // get list
            SPList list = SPContext.Current.Web.Lists[new Guid(ddl.SelectedValue)];

            if (String.IsNullOrEmpty(column))
                column = list.Fields.ContainsFieldWithStaticName("Title")
                    ? "Title"
                    : string.Empty;

            // get columns from selected list
            foreach (SPField field in list.Fields)
            {
                // remove read only fields and base type
                if (FieldExceptions.Contains(field.Id) || (!field.ReadOnlyField && !field.FromBaseType))
                {
                    ListItem item = new ListItem(field.Title, field.InternalName);
                    if (item.Value == column)
                        item.Selected = true;
                    ddlChild.Items.Add(item);
                }
            }
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Handles the SelectedChanged event of the ddlCascadeList control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        public void ddlCascadeList_SelectedChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;

            ChangeColumn(ddl, ddlCascadeListColumn, null);
            ChangeColumn(ddl, ddlCascadeDependencyColumn, null);
        }

        /// <summary>
        /// Handles the CheckedChange event of the chkCascadeDependencies control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        public void chkCascadeDependencies_CheckedChange(object sender, EventArgs e)
        {
            CheckBox chk = (CheckBox)sender;

            tblDependencies.Visible = chk.Checked;
        }

        #endregion
    }
}
