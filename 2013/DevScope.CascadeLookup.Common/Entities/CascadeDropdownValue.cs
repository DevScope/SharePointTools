using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace DevScope.CascadeLookup.Common.Entities
{
    [Serializable]
    [DataContract]
    public class CascadeDropdownValue
    {
        /// <summary>
        /// Gets or sets the label.
        /// </summary>
        /// <value>
        /// The label.
        /// </value>
        [DataMember]
        public string label { get; set; }
        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        [DataMember]
        public string value { get; set; }
        /// <summary>
        /// Gets or sets the selected
        /// </summary>
        [DataMember]
        public bool selected { get; set; }
    }
}
