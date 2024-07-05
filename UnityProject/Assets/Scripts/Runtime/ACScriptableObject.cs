using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AC
{
    /// <summary>
    /// Base scriptable object for all of the game's scriptable objects.
    /// <br>Created to avoid having to generate unecesary data when accessing the object's .name by using <see cref="cachedName"/></br>
    /// </summary>
    public abstract class ACScriptableObject : ScriptableObject
    {
        /// <summary>
        /// This property should not be used as it generates garbage, use <see cref="cachedName"/> instead. If getting the name from the engine is absolutely necesary, cast to <see cref="ScriptableObject"/> first.
        /// </summary>
        [Obsolete("This property should not be used as it generates garbage, use \"cachedName\" instead. If getting the value from the engine is absolutely necessary, cast to ScriptableObject first.")]
        public new string name
        {
            get
            {
                throw new NotSupportedException();
            }
            set
            {
                throw new NotSupportedException();
            }
        }

        /// <summary>
        /// Retrieves the object's name using a cached string.
        /// </summary>
        public string cachedName
        {
            get
            {
                if(string.IsNullOrEmpty(_cachedName))
                {
                    _cachedName = base.name;
                }
                return _cachedName;
            }
            set
            {
                _cachedName = value;
                base.name = value;
            }
        }
        [NonSerialized]
        private string _cachedName;
    }
}
