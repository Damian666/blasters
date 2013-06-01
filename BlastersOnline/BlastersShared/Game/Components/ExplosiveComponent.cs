using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlastersShared.Game.Components
{
    /// <summary>
    /// A component that indiciates a given entity is explosive
    /// </summary>
    public class ExplosiveComponent : Component
    {

        public ExplosiveComponent()
        {
            
        }

        public ExplosiveComponent(float detonateTime, int range)
        {
            DetonationTime = detonateTime;
            Range = range;
        }

        /// <summary>
        /// The amount of time until entity will detonate; used to keep track
        /// </summary>
        public float DetonationTime { get; set; }

        /// <summary>
        /// The given range of of this <see cref="ExplosiveEntity"/>. 
        /// </summary>
        public int Range { get; set;  }

    }
}
