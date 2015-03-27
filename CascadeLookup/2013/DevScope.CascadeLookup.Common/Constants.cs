using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DevScope.CascadeLookup.Common
{
    /// <summary>
    /// This class contains the constants to be used
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// The cascade lookup field type
        /// </summary>
        public const string CascadeLookupFieldType = "CascadeLookup";

        #region Editor

        /// <summary>
        /// The editor list guid property
        /// </summary>
        public const string EditorListGuidProperty = "ListGuid";
        /// <summary>
        /// The editor list column property
        /// </summary>
        public const string EditorListColumnProperty = "ListColumn";
        /// <summary>
        /// The editor dependencies property
        /// </summary>
        public const string EditorDependenciesProperty = "Dependencies";
        /// <summary>
        /// The editor dependency column property
        /// </summary>
        public const string EditorDependencyColumnProperty = "DependencyColumn";
        /// <summary>
        /// The editor dependency list column property
        /// </summary>
        public const string EditorDependencyListColumnProperty = "DependencyListColumn";

        #endregion

        #region Property Bags

        /// <summary>
        /// The cascade mode property bag
        /// </summary>
        public const string CascadeModePropertyBag = "dvcascadelookpmode";

        #endregion
    }
}
