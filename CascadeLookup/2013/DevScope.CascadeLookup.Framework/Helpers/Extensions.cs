using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace DevScope.CascadeLookup.Framework.Helpers
{
    public static class Extensions
    {
        #region Page - CSS

        /// <summary>
        /// Registers a css on the page header
        /// </summary>
        /// <param name="page">The page.</param>
        /// <param name="rootPath">The root path for css folder.</param>
        /// <param name="id">The id.</param>
        /// <param name="filename">The filename.</param>
        /// <param name="forceMin">if set to <c>true</c> [force minimum].</param>
        public static void RegisterCSS(this Page page, Uri rootPath, string id, string filename, bool forceMin)
        {
            RegisterCSS(page, rootPath, id, filename, null, forceMin);
        }

        /// <summary>
        /// Registers a css on the page header
        /// </summary>
        /// <param name="page">The page.</param>
        /// <param name="rootPath">The root path for css folder.</param>
        /// <param name="id">The id.</param>
        /// <param name="filename">The filename.</param>
        /// <param name="folder">The folder.</param>
        /// <param name="forceMin">if set to <c>true</c> [force minimum].</param>
        public static void RegisterCSS(this Page page, Uri rootPath, string id, string filename, string folder, bool forceMin)
        {
            if (page.Header.FindControl(id) == null)
            {
                HtmlGenericControl csslink = new HtmlGenericControl("link");
                csslink.ID = id;
                csslink.Attributes.Add("href", GenerateCSSUrl(rootPath, folder, filename, forceMin));
                csslink.Attributes.Add("type", "text/css");
                csslink.Attributes.Add("rel", "stylesheet");
                page.Header.Controls.Add(csslink);
            }
        }

        /// <summary>
        /// Generates the css url
        /// </summary>
        /// <param name="rootPath">The root path.</param>
        /// <param name="folder">The folder.</param>
        /// <param name="fileName">The filename.</param>
        /// <param name="forceMin">if set to <c>true</c> [force minimum].</param>
        /// <returns></returns>
        private static string GenerateCSSUrl(Uri rootPath, string folder, string fileName, bool forceMin)
        {
            string cssExtension = ".min.css";
#if DEBUG
            cssExtension = ".css";
#endif
            //Força os Min porque só existem ficheiros min
            if(forceMin)
                cssExtension = ".min.css";

            return string.Format("{0}{1}{2}{3}",
               rootPath,
               !String.IsNullOrEmpty(folder) ? folder + "/" : string.Empty,
               fileName,
               cssExtension);
        }

        #endregion

        #region Script Manager

        /// <summary>
        /// Checks whether a script block was registered on the page
        /// </summary>
        /// <param name="sm">The script manager.</param>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public static bool IsClientScriptBlockRegistered(this ScriptManager sm, string key)
        {
            ReadOnlyCollection<RegisteredScript> scriptBlocks = sm.GetRegisteredClientScriptBlocks();

            foreach (RegisteredScript rs in scriptBlocks)
            {
                if (rs.Key == key)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Registers script file and checks if is already on page
        /// </summary>
        /// <param name="sm">The script manager.</param>
        /// <param name="rootPath">The root path.</param>
        /// <param name="key">The key.</param>
        /// <param name="fileName">The filename.</param>
        /// <param name="forceMin">if set to <c>true</c> [force minimum].</param>
        public static void RegisterScriptFile(this ScriptManager sm, Uri rootPath, string key, string fileName, bool forceMin)
        {
            RegisterScriptFile(sm, rootPath, key, fileName, null, forceMin);
        }

        /// <summary>
        /// Registers script file and checks if is already on page
        /// </summary>
        /// <param name="sm">The script manager.</param>
        /// <param name="rootPath">The root path for js folder.</param>
        /// <param name="control">The control.</param>
        /// <param name="key">The key.</param>
        /// <param name="fileName">The filename.</param>
        public static void RegisterScriptFile(this ScriptManager sm, Uri rootPath, string key, string fileName, string folder, bool forceMin)
        {
            if (!sm.IsClientScriptBlockRegistered(key))
                ScriptManager.RegisterClientScriptBlock(
                    sm.Page,
                    sm.Page.GetType(),
                    key,
                    GenerateJSInclude(rootPath, folder, fileName, forceMin),
                    false);
        }

        /// <summary>
        /// Generates the JS block to include a JS file on layouts
        /// </summary>
        /// <param name="rootPath">The root path for js folder.</param>
        /// <param name="folder">The folder.</param>
        /// <param name="fileName">The filename.</param>
        /// <param name="forceMin">if set to <c>true</c> [force minimum].</param>
        /// <returns></returns>
        private static string GenerateJSInclude(Uri rootPath, string folder, string fileName, bool forceMin)
        {
            string jsExtension = ".min.js";
#if DEBUG
            jsExtension = ".js";
#endif
            //Para forçar o Min mesmo no debug
            if(forceMin)
                jsExtension = ".min.js";

            return string.Format("<script type=\"text/javascript\" src=\"{0}{1}{2}{3}\"></script>",
               rootPath,
               !String.IsNullOrEmpty(folder) ? folder + "/" : string.Empty,
               fileName,
               jsExtension);
        }

        #endregion
    }
}
