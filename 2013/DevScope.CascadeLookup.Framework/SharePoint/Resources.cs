using Microsoft.SharePoint.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DevScope.CascadeLookup.Framework.SharePoint
{
    public static class Resources
    {
        /// <summary>
        /// Gets the localized string.
        /// </summary>
        /// <param name="resource">The resource.</param>
        /// <param name="resourceFile">The resource file.</param>
        /// <param name="lcid">The lcid.</param>
        /// <returns></returns>
        public static string GetLocalizedString(string resource, string resourceFile, uint lcid)
        {
            return SPUtility.GetLocalizedString(string.Format("$Resources:{0}", resource)
                , resourceFile, lcid);
        }
    }
}
