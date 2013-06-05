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

        public ExplosiveComponent(float detonateTime, int range, ulong ownerID)
        {
            DetonationTime = detonateTime;
            Range = range;
            OwnerID = ownerID;
        }

        /// <summary>
        /// The amount of time until entity will detonate; used to keep track
        /// </summary>
        public double DetonationTime { get; set; }

        /// <summary>
        /// The given range of of this <see cref="ExplosiveEntity"/>. 
        /// </summary>
        public int Range { get; set;  }

        /// <summary>
        /// The ID Of the entity that owns this particular explosive; used for computing kills.
        /// Null indicates the entity is owned by noone.
        /// </summary>
        public ulong OwnerID { get; set; }

    }
}
