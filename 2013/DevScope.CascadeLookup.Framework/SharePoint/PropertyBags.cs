using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;

namespace DevScope.CascadeLookup.Framework.SharePoint
{
    public static class PropertyBags
    {
        #region SetWebPropertyBagValue

        /// <summary>
        /// Sets the web property bag value.
        /// </summary>
        /// <param name="spWeb">The sp web.</param>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public static void SetWebPropertyBagValue(SPWeb spWeb, string key, string value)
        {
            string contextUrl = spWeb.Url;

            DataAccess.RunAsAdmin(contextUrl, (site, web) =>
            {
                if (web.Properties.ContainsKey(key))
                    web.Properties[key] = value;
                else
                    web.Properties.Add(key, value);

                web.Properties.Update();
                web.Update();
            });
        }

        #endregion

        #region GetWebPropertyBagValue

        /// <summary>
        /// Sets the web property bag value.
        /// </summary>
        /// <param name="spWeb">The sp web.</param>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public static string GetWebPropertyBagValue(SPWeb spWeb, string key)
        {
            string contextUrl = spWeb.Url;
            string value = string.Empty;

            DataAccess.RunAsAdmin(contextUrl, (site, web) =>
            {
                if (web.Properties.ContainsKey(key))
                    value = web.Properties[key];
            });

            return value;
        }

        #endregion

        #region SetSiteCollectionPropertyBagValue

        /// <summary>
        /// Sets the SPSite property bag value.
        /// </summary>
        /// <param name="spSite">The spSite.</param>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public static void SetSiteCollectionPropertyBagValue(SPSite spSite, string key, string value)
        {
            SetWebPropertyBagValue(spSite.RootWeb, key, value);
        }

        #endregion

        #region GetSiteCollectionPropertyBagValue

        /// <summary>
        /// Sets the sitecollection property bag value.
        /// </summary>
        /// <param name="spSite">The spSite.</param>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public static string GetWebPropertyBagValue(SPSite spSite, string key)
        {
            string contextUrl = spSite.RootWeb.Url;
            string value = string.Empty;

            DataAccess.RunAsAdmin(contextUrl, (site, web) =>
            {
                if (web.AllProperties.ContainsKey(key))
                    value = web.AllProperties[key] + string.Empty;
            });

            return value;
        }

        #endregion

        #region SetWebApplicationPropertyBagValue

        /// <summary>
        /// Sets the web property bag value.
        /// </summary>
        /// <param name="spWeb">The sp web.</param>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public static void SetWebApplicationPropertyBagValue(SPSite spSite, string key, string value)
        {
            string contextUrl = spSite.Url;

            DataAccess.RunAsAdmin(contextUrl, (site, web) =>
            {
                if (site.WebApplication.Properties.ContainsKey(key))
                    site.WebApplication.Properties[key] = value;
                else
                    site.WebApplication.Properties.Add(key, value);

                site.WebApplication.Update();
            });
        }

        #endregion

        #region GetWebApplicationPropertyBagValue

        /// <summary>
        /// Sets the web property bag value.
        /// </summary>
        /// <param name="spWeb">The sp web.</param>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public static string GetWebApplicationPropertyBagValue(SPSite spSite, string key)
        {
            string contextUrl = spSite.Url;
            string value = string.Empty;

            DataAccess.RunAsAdmin(contextUrl, (site, web) =>
            {
                if (site.WebApplication.Properties.ContainsKey(key))
                    value = site.WebApplication.Properties[key].ToString();
            });

            return value;
        }

        #endregion
    }
}
