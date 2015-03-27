using DevScope.CascadeLookup.Common.Entities;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Client.Services;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;

namespace DevScope.CascadeLookup.Services
{
    [BasicHttpBindingServiceMetadataExchangeEndpoint]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
    public class CascadeLookupService : ICascadeLookupService
    {
        /// <summary>
        /// Gets the items.
        /// </summary>
        /// <param name="listID">The list identifier.</param>
        /// <param name="columnName">Name of the column.</param>
        /// <param name="filterColumn">The filter column.</param>
        /// <param name="filterID">The filter identifier.</param>
        /// <param name="hasDependency">if set to <c>true</c> [has dependency].</param>
        /// <param name="required">if set to <c>true</c> [required].</param>
        /// <returns></returns>
        public List<CascadeDropdownValue> GetItems(string listID, string columnName, string filterColumn, string filterID, bool hasDependency, bool required, string selectedItemId)
        {
            List<CascadeDropdownValue> values = new List<CascadeDropdownValue>();

            // get the list
            SPList list = SPContext.Current.Web.Lists[new Guid(listID)];
            if (list == null)
                return null;

            // get all possible values from list
            SPQuery query = new SPQuery()
            {
                Query = hasDependency
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
            if (!required && items.Count > 0)
                values.Add(new CascadeDropdownValue()
                    {
                        label = Framework.SharePoint.Resources.GetLocalizedString("EditorEmptyCascadeItem",
                    "CascadeLookupResources", (uint)SPContext.Current.Web.UICulture.LCID),
                        value = string.Empty
                    });

            // add the rest of items
            foreach (SPListItem item in items)
                values.Add(new CascadeDropdownValue()
                {
                    label = (string)item[columnName],
                    value = item.ID.ToString(),
                    selected = item.ID.ToString() == selectedItemId ? true : false

                });

            return values;
        }
    }
}
