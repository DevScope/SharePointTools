using DevScope.CascadeLookup.Framework.Loggers;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;

namespace DevScope.CascadeLookup.Framework.SharePoint
{
    public static class DataAccess
    {
        #region QueryList

        /// <summary>
        /// Queries the list.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="fieldValue">The field value.</param>
        /// <param name="fieldType">Type of the field.</param>
        /// <returns></returns>
        public static SPListItemCollection QueryList(SPList list, string fieldName, string fieldValue,
                                                     SPFieldType fieldType)
        {
            SPQuery queryToList = new SPQuery
            {
                Query = string.Format(@"<Where>
                                                <Eq>
                                                    <FieldRef Name='{0}' />
                                                    <Value Type='{1}'>{2}</Value>
                                                </Eq>
                                            </Where>"
                                      , fieldName
                                      , GetFieldType(fieldType)
                                      , fieldValue)
            };

            return list.GetItems(queryToList);
        }

        #endregion

        #region QueryList

        /// <summary>
        /// Queries the list.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <param name="queryClause">The query clause.</param>
        /// <param name="viewFields">The view fields. If null returns all fields.</param>
        /// <param name="rowLimit">The row limit. If 0 does not create RowLimit</param>
        /// <returns></returns>
        public static SPListItemCollection QueryList(SPList list, string queryClause, string viewFields, int rowLimit)
        {
            return QueryList(list, queryClause, viewFields, rowLimit, null);
        }

        #endregion

        #region QueryList

        /// <summary>
        /// Queries the list.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <param name="queryClause">The query clause.</param>
        /// <param name="viewFields">The view fields.</param>
        /// <param name="rowLimit">The row limit.</param>
        /// <param name="folder">The folder.</param>
        /// <returns></returns>
        public static SPListItemCollection QueryList(SPList list, string queryClause, string viewFields, int rowLimit,
                                                     SPFolder folder)
        {
            return QueryList(list, queryClause, viewFields, rowLimit, folder, null);
        }

        #endregion

        #region QueryList

        /// <summary>
        /// Queries the list.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <param name="queryClause">The query clause.</param>
        /// <param name="viewFields">The view fields.</param>
        /// <param name="rowLimit">The row limit.</param>
        /// <param name="folder">The folder.</param>
        /// <param name="viewAttributes">The view attributes.</param>
        /// <returns></returns>
        public static SPListItemCollection QueryList(SPList list, string queryClause, string viewFields, int rowLimit,
                                                     SPFolder folder, string viewAttributes)
        {
            SPQuery queryToList = new SPQuery();
            if (rowLimit != 0)
                queryToList.RowLimit = (uint)rowLimit;
            queryToList.Query = queryClause;
            if (folder != null)
            {
                queryToList.Folder = folder;
            }
            if (viewFields != null)
            {
                queryToList.ViewFields = viewFields;
                queryToList.ViewFieldsOnly = true;
            }
            if (viewFields != null)
            {
                queryToList.ViewAttributes = viewAttributes;
            }

            return list.GetItems(queryToList);
        }

        /// <summary>
        /// Queries the list data table.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <param name="queryClause">The query clause.</param>
        /// <param name="viewFields">The view fields.</param>
        /// <returns></returns>
        public static DataTable QueryListDataTable(SPList list, string queryClause, string viewFields)
        {
            SPQuery queryToList = new SPQuery();
            queryToList.Query = queryClause;

            if (viewFields != null)
            {
                queryToList.ViewFields = viewFields;
                queryToList.ViewFieldsOnly = true;
            }

            return list.GetItems(queryToList).GetDataTable();
        }

        #endregion

        #region QueryListPosition

        /// <summary>
        /// Queries the list.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <param name="queryClause">The query clause.</param>
        /// <param name="viewFields">The view fields. If null returns all fields.</param>
        /// <param name="rowLimit">The row limit. If 0 does not create RowLimit</param>
        /// <param name="next">if set to <c>true</c> [next].</param>
        /// <param name="positionID">The position identifier.</param>
        /// <param name="positionOrderValue">The position order value.</param>
        /// <returns></returns>
        public static SPListItemCollection QueryListWithPosition(SPList list, string queryClause, string viewFields, int rowLimit,
            bool next, int positionID, KeyValuePair<string, string> positionOrderValue)
        {
            return QueryListWithPosition(list, queryClause, viewFields, rowLimit, next, positionID, positionOrderValue, null);
        }

        /// <summary>
        /// Queries the list.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <param name="queryClause">The query clause.</param>
        /// <param name="viewFields">The view fields.</param>
        /// <param name="rowLimit">The row limit.</param>
        /// <param name="next">if set to <c>true</c> [next].</param>
        /// <param name="positionID">The position identifier.</param>
        /// <param name="positionOrderValue">The position order value.</param>
        /// <param name="folder">The folder.</param>
        /// <returns></returns>
        public static SPListItemCollection QueryListWithPosition(SPList list, string queryClause, string viewFields, int rowLimit,
                                                     bool next, int positionID, KeyValuePair<string, string> positionOrderValue, SPFolder folder)
        {
            return QueryListWithPosition(list, queryClause, viewFields, rowLimit, next, positionID, positionOrderValue, folder, null);
        }

        /// <summary>
        /// Queries the list with position.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <param name="queryClause">The query clause.</param>
        /// <param name="viewFields">The view fields.</param>
        /// <param name="rowLimit">The row limit.</param>
        /// <param name="next">if set to <c>true</c> [next].</param>
        /// <param name="positionID">The position identifier.</param>
        /// <param name="positionOrderValue">The position order value.</param>
        /// <param name="folder">The folder.</param>
        /// <param name="viewAttributes">The view attributes.</param>
        /// <returns></returns>
        public static SPListItemCollection QueryListWithPosition(SPList list, string queryClause, string viewFields, int rowLimit,
            bool next, int positionID, KeyValuePair<string, string> positionOrderValue, SPFolder folder, string viewAttributes)
        {
            SPQuery queryToList = new SPQuery();
            if (rowLimit != 0)
                queryToList.RowLimit = (uint)rowLimit;
            queryToList.Query = queryClause;
            if (folder != null)
            {
                queryToList.Folder = folder;
            }
            if (viewFields != null)
            {
                queryToList.ViewFields = viewFields;
                queryToList.ViewFieldsOnly = true;
            }
            if (viewFields != null)
            {
                queryToList.ViewAttributes = viewAttributes;
            }

            string positionOrder = !String.IsNullOrEmpty(positionOrderValue.Key)
            ? string.Format("&p_{0}={1}", positionOrderValue.Key, positionOrderValue.Value)
            : string.Empty;

            SPListItemCollectionPosition position = null;
            // go to next page
            if (next)
                position = !String.IsNullOrEmpty(positionOrder)
            ? new SPListItemCollectionPosition(string.Format("Paged=TRUE&p_FSObjType=0{0}&p_ID={1}", positionOrder, positionID))
            : new SPListItemCollectionPosition(string.Format("Paged=TRUE&p_ID={0}", positionID));
            // go to previous page
            else
                position = !String.IsNullOrEmpty(positionOrder)
            ? new SPListItemCollectionPosition(string.Format("Paged=TRUE&p_FSObjType=0&PagedPrev=TRUE{0}&p_ID={1}", positionOrder, positionID))
            : new SPListItemCollectionPosition(string.Format("Paged=TRUE&PagedPrev=TRUE&p_ID={0}", positionID));
            queryToList.ListItemCollectionPosition = position;

            return list.GetItems(queryToList);
        }

        #endregion

        #region Query Paged List

        /// <summary>
        /// Queries the paged list.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <param name="queryClause">The query clause.</param>
        /// <param name="viewFields">The view fields.</param>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="orderField">The order field.</param>
        /// <param name="itemsCount">The items count.</param>
        /// <returns></returns>
        public static SPListItemCollection QueryPagedList(SPList list, string queryClause, string viewFields, int pageIndex, int pageSize, string orderField, ref int itemsCount)
        {
            return QueryPagedList(list, queryClause, viewFields, pageIndex, pageSize, orderField, null, ref itemsCount);
        }

        /// <summary>
        /// Queries the paged list.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <param name="queryClause">The query clause.</param>
        /// <param name="viewFields">The view fields.</param>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="orderField">The order field.</param>
        /// <param name="folder">The folder.</param>
        /// <param name="itemsCount">The items count.</param>
        /// <returns></returns>
        public static SPListItemCollection QueryPagedList(SPList list, string queryClause, string viewFields, int pageIndex, int pageSize, string orderField,
           SPFolder folder, ref int itemsCount)
        {
            return QueryPagedList(list, queryClause, viewFields, pageIndex, pageSize, orderField, folder, null, ref itemsCount);
        }

        /// <summary>
        /// Queries the paged list.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <param name="queryClause">The query clause.</param>
        /// <param name="viewFields">The view fields.</param>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="orderField">The order field.</param>
        /// <param name="folder">The folder.</param>
        /// <param name="viewAttributes">The view attributes.</param>
        /// <param name="itemsCount">The items count.</param>
        /// <returns></returns>
        public static SPListItemCollection QueryPagedList(SPList list, string queryClause, string viewFields, int pageIndex, int pageSize, string orderField,
           SPFolder folder, string viewAttributes, ref int itemsCount)
        {
            // get only the id's and order fields
            string viewPositionFields = "<FieldRef Name='ID' />";
            if (!String.IsNullOrEmpty(orderField))
                viewPositionFields += string.Format("<FieldRef Name='{0}' />", orderField);

            SPListItemCollection allIds = DataAccess.QueryList(list, queryClause, viewPositionFields, 0);

            // set records count
            itemsCount = allIds != null
            ? allIds.Count
            : 0;

            // only if page index is more than 0
            if (pageIndex > 0)
            {
                if (allIds != null && allIds.Count > 0)
                {
                    // get last index before the start id
                    int index = (pageIndex * pageSize) - 1;
                    SPListItem lastItem = allIds[index];
                    KeyValuePair<string, string> positionOrderValue = new KeyValuePair<string, string>();
                    if (!String.IsNullOrEmpty(orderField))
                    {
                        SPFieldType fieldType = lastItem.Fields.GetFieldByInternalName(orderField).Type;
                        string fieldValue = string.Empty;

                        switch (fieldType)
                        {
                            case SPFieldType.DateTime:
                                DateTime dateValue = Convert.ToDateTime(lastItem[orderField]).ToUniversalTime();
                                fieldValue = SPEncode.UrlEncode(dateValue.ToString("yyyyMMdd HH:mm:ss"));
                                break;
                            default:
                                SPEncode.UrlEncode(lastItem[orderField].ToString());
                                break;
                        }

                        positionOrderValue = new KeyValuePair<string, string>(orderField, fieldValue);
                    }

                    // get paged items
                    return DataAccess.QueryListWithPosition(list, queryClause, viewFields, pageSize, true, lastItem.ID, positionOrderValue, folder, viewAttributes);
                }

                return null;
            }
            else
                return DataAccess.QueryList(list, queryClause, viewFields, pageSize, folder, viewAttributes);
        }

        #endregion

        #region GetFieldType

        /// <summary>
        /// Gets the type of the field.
        /// </summary>
        /// <param name="fieldType">Type of the field.</param>
        /// <returns></returns>
        public static string GetFieldType(SPFieldType fieldType)
        {
            //Uso da técnica devido a falta de performance no convert de Enum para string
            //You should not use enum.ToString(), enum.GetNames(), enum.GetName(), enum.Format() or enum.Parse() to convert an enum to a string. 
            //Instead, use a switch statement, and also internationalize the names if necessary.

            switch (fieldType)
            {
                case SPFieldType.Text:
                    return "Text";
                case SPFieldType.Number:
                    return "Number";
                case SPFieldType.Note:
                    return "Note";
                case SPFieldType.Choice:
                    return "Choice";
                case SPFieldType.Boolean:
                    return "Boolean";
                case SPFieldType.Integer:
                    return "Integer";
                case SPFieldType.Counter:
                    return "Counter";
                default:
                    return "Text";
            }
        }

        /// <summary>
        /// Gets the item by URL.
        /// </summary>
        /// <param name="spWeb">The sp web.</param>
        /// <param name="url">The URL.</param>
        /// <returns></returns>
        public static SPListItem GetItemByUrl(SPWeb spWeb, string url)
        {
            try
            {
                // get the position of the last slash so that the string can be split
                int lastSlashPosition = url.LastIndexOf('/');

                // folder url
                string folderUrl = url.Substring(0, lastSlashPosition);

                // fileUrl
                string fileUrl = url.Substring(lastSlashPosition + 1);

                // get folder object
                SPFolder folder = spWeb.GetFolder(folderUrl);

                // get file object
                SPFile file = folder.Files[fileUrl];

                // get the list item
                SPListItem item = file.Item;
                return item;
            }
            catch (Exception ex)
            {
                SharePointLogger.LogError(ex);
                return null;
            }
        }

        #endregion

        #region RunWithAdminDelegate

        /// <summary>
        /// Runs With Admin Delegate
        /// </summary>
        /// <param name="site">The site.</param>
        /// <param name="web">The web.</param>
        public delegate void RunWithAdminDelegate(SPSite site, SPWeb web);

        /// <summary>
        /// Runs as admin.
        /// </summary>
        /// <param name="siteID">The site ID.</param>
        /// <param name="webID">The web ID.</param>
        /// <param name="myDelegate">My delegate.</param>
        public static void RunAsAdmin(Guid siteID, Guid webID, RunWithAdminDelegate myDelegate)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate
            {
                using (SPSite site = new SPSite(siteID))
                {
                    site.AllowUnsafeUpdates = true;
                    using (SPWeb web = site.OpenWeb())
                    {
                        web.AllowUnsafeUpdates = true;
                        myDelegate.Invoke(site, web);
                        web.AllowUnsafeUpdates = false;
                    }
                    site.AllowUnsafeUpdates = false;
                }
            });
        }

        /// <summary>
        /// Runs as admin.
        /// </summary>
        /// <param name="contextUrl">The context URL.</param>
        /// <param name="myDelegate">My delegate.</param>
        public static void RunAsAdmin(string contextUrl, RunWithAdminDelegate myDelegate)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate
            {
                using (SPSite site = new SPSite(contextUrl))
                {
                    site.AllowUnsafeUpdates = true;
                    using (SPWeb web = site.OpenWeb())
                    {
                        web.AllowUnsafeUpdates = true;
                        myDelegate.Invoke(site, web);
                        web.AllowUnsafeUpdates = false;
                    }
                    site.AllowUnsafeUpdates = false;
                }
            });
        }

        #endregion

        #region RunAsUser
        /// <summary>
        /// Runs With Admin Delegate
        /// </summary>
        /// <param name="site">The site.</param>
        /// <param name="web">The web.</param>
        public delegate void RunWithUserDelegate(SPSite site, SPWeb web);

        /// <summary>
        /// Runs as user.
        /// </summary>
        /// <param name="contextUrl">The context URL.</param>
        /// <param name="myDelegate">My delegate.</param>
        public static void RunAsUser(string contextUrl, RunWithUserDelegate myDelegate)
        {
            using (SPSite site = new SPSite(contextUrl))
            {
                site.AllowUnsafeUpdates = true;
                using (SPWeb web = site.OpenWeb())
                {
                    web.AllowUnsafeUpdates = true;
                    myDelegate.Invoke(site, web);
                    web.AllowUnsafeUpdates = false;
                }
                site.AllowUnsafeUpdates = false;
            }
        }
        #endregion

        #region CAML Builder
        // Too short to create a new class

        public static string CreateCAMLQueryOR(String template, String fieldName, List<string> values)
        {
            if (values.Count == 0) return "";

            var query = new StringBuilder();
            if (values.Count == 1)
                query.Append(string.Format(template, fieldName, values[0]));
            if (values.Count > 1)
                query.AppendFormat("<Or>{0}{1}</Or>", string.Format(template, fieldName, values[0]), CreateCAMLQueryOR(template, fieldName, values.Skip(1).ToList()));
            return query.ToString();
        }

        public static string CreateCAMLQueryAND(String template, String fieldName, List<string> values)
        {
            if (values.Count == 0) return "";

            var query = new StringBuilder();
            if (values.Count == 1)
                query.Append(string.Format(template, fieldName, values[0]));
            if (values.Count > 1)
                query.AppendFormat("<And>{0}{1}</And>", string.Format(template, fieldName, values[0]), CreateCAMLQueryAND(template, fieldName, values.Skip(1).ToList()));
            return query.ToString();
        }

        #endregion

        #region LIST CRUD OPS

        public static void DeleteListItemAsAdmin(String webUrl, String listUrl, String query)
        {
            DataAccess.RunAsAdmin(webUrl, (site, web) =>
            {
                SPList list = web.GetList(web.Url + listUrl);
                var items = DataAccess.QueryList(list, query, null, 100);
                for (int i = items.Count - 1; i >= 0; i--)
                {
                    items[i].Delete();
                }
                list.Update();
            }
            );
        }

        public static void DeleteListItemAsAdmin(String webUrl, String listUrl, int id)
        {
            DataAccess.RunAsAdmin(webUrl, (site, web) =>
            {
                SPList list = web.GetList(web.Url + listUrl);
                list.Items.DeleteItemById(id);
            }
            );
        }

        public static List<T> GetListItemAsAdmin<T>(String url, String listView, String query, Func<SPListItem, T> map)
        {
            return GetListItemAsAdmin<T>(url, listView, query, map, null, 100);
        }

        public static List<T> GetListItemAsAdmin<T>(String url, String listView, String query, Func<SPListItem, T> map, String viewFields)
        {
            return GetListItemAsAdmin<T>(url, listView, query, map, viewFields, 100);
        }

        public static List<T> GetListItemAsAdmin<T>(String url, String listView, String query, Func<SPListItem, T> map, String viewFields, uint rowLimit)
        {
            List<T> result = new List<T>(); ;
            DataAccess.RunAsAdmin(url, (site, web) =>
            {
                SPList list = web.GetList(web.Url + listView);
                var items = DataAccess.QueryList(list, query, null, 100);
                result = items.OfType<SPListItem>().Select(spItem => map(spItem)).ToList();
            }
            );
            return result;
        }

        public static void UpdateItemAsUser(String webUrl, String listView, int itemId, Dictionary<String, object> values, bool useSystemUpdate)
        {
            DataAccess.RunAsUser(webUrl, (site, web) =>
            {
                SPList list = web.GetList(web.Url + listView);
                SPListItem spListItem = list.GetItemById(itemId);
                values.ToList().ForEach(kv => spListItem[kv.Key] = kv.Value);
                if (useSystemUpdate)
                    spListItem.SystemUpdate();
                else
                    spListItem.Update();
            });
        }

        public static void UpdateItemAsAdmin(String webUrl, String listView, int itemId, Dictionary<String, object> values, bool useSystemUpdate)
        {
            DataAccess.RunAsAdmin(webUrl, (site, web) =>
            {
                SPList list = web.GetList(web.Url + listView);
                SPListItem spListItem = list.GetItemById(itemId);
                values.ToList().ForEach(kv => spListItem[kv.Key] = kv.Value);
                if (useSystemUpdate)
                    spListItem.SystemUpdate();
                else
                    spListItem.Update();
            });
        }

        public static int InsertItemAsUser(String webUrl, String listView, Func<SPListItem, SPListItem> map, bool useSystemUpdate = false)
        {
            var id = 0;
            DataAccess.RunAsUser(webUrl, (site, web) =>
            {
                SPList list = web.GetList(web.Url + listView);
                SPListItem spListItem = list.Items.Add();
                spListItem = map(spListItem);
                if (useSystemUpdate)
                    spListItem.SystemUpdate();
                else
                    spListItem.Update();
                id = spListItem.ID;
            }
            );

            return id;
        }

        public static int InsertItemAsAdmin(String webUrl, String listView, Func<SPListItem, SPListItem> map)
        {
            return InsertItemAsAdmin(webUrl, listView, map, false);
        }

        public static int InsertItemAsAdmin(String webUrl, String listView, Func<SPListItem, SPListItem> map, bool useSystemUpdate)
        {
            var id = 0;
            DataAccess.RunAsAdmin(webUrl, (site, web) =>
            {
                SPList list = web.GetList(web.Url + listView);
                SPListItem spListItem = list.Items.Add();
                spListItem = map(spListItem);
                if (useSystemUpdate)
                    spListItem.SystemUpdate();
                else
                    spListItem.Update();
                id = spListItem.ID;
            }
            );

            return id;
        }

        public static void InsertItemAsAdmin(String webUrl, String listView, Func<SPWeb, SPListItem, SPListItem> map, bool useSystemUpdate = false)
        {
            DataAccess.RunAsAdmin(webUrl, (site, web) =>
            {
                SPList list = web.GetList(web.Url + listView);
                SPListItem spListItem = list.Items.Add();
                spListItem = map(web, spListItem);
                if (useSystemUpdate)
                    spListItem.SystemUpdate();
                else
                    spListItem.Update();
            }
            );
        }

        public static void UpdateItemAsAdmin(String webUrl, String view, int itemId, String key, object value, bool useSystemUpdate)
        {
            UpdateItemAsAdmin(webUrl, view, itemId, new Dictionary<String, object> { { key, value } }, useSystemUpdate);
        }

        public static void UpdateItemAsAdmin(string webUrl, string listView, List<int> itemIds, Dictionary<string, object> values, bool useSystemUpdate)
        {
            DataAccess.RunAsAdmin(webUrl, (site, web) =>
            {
                SPList list = web.GetList(web.Url + listView);
                itemIds.ForEach(itemId =>
                    {
                        SPListItem spListItem = list.GetItemById(itemId);
                        values.ToList().ForEach(kv => spListItem[kv.Key] = kv.Value);
                        if (useSystemUpdate)
                            spListItem.SystemUpdate();
                        else
                            spListItem.Update();
                    }
                );
            });
        }

        #endregion LIST CRUD OPS
    }
}
