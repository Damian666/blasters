using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BlastersShared.Game.Components.PowerUp;

namespace BlastersShared.Game.Components
{
    /// <summary>
    /// A powerup collection is just a collection of poweurps that a given entity may be containing.
    /// These collections can be collectied, unpacked and applies to a given entity.
    /// </summary>
    public class PowerUpCollectionComponent
    {

        public List<PowerUpComponent> PowerUps { get; set; } 

        public PowerUpCollectionComponent(List<PowerUpComponent> powerUpComponents )
        {
            PowerUps = powerUpComponents;
        }

    }
}
