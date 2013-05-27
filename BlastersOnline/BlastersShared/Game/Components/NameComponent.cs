using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlastersShared.Game.Components
{
    /// <summary>
    /// The name component is a simple container for data pertaining to entities that have a visible name.
    /// </summary>
    public class NameComponent : Component
    {
        public string Name { get; set; }

        public NameComponent(string name)
        {
            Name = name;
        }


        public NameComponent()
        {
            
        }
    }
}
