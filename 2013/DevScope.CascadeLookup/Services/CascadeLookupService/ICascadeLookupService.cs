using DevScope.CascadeLookup.Common.Entities;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace DevScope.CascadeLookup.Services
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ICascadeLookupService" in both code and config file together.
    [ServiceContract]
    public interface ICascadeLookupService
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
        [WebInvoke(Method = "GET", BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        List<CascadeDropdownValue> GetItems(string listID, string columnName, string filterColumn, string filterID, bool hasDependency, bool required, string selectedItemId);
    }
}

