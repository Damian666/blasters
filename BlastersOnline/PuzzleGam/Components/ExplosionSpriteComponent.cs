using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BlastersShared.Game;

namespace BlastersGame.Components
{

    public enum ExplosiveType
    {
        Center,
        Left,
        Right,
        Up,
        Down,
        LeftE,
        RightE,
        UpE,
        DownE
    }

    /// <summary>
    /// The explosion sprite component
    /// </summary>
    public class ExplosionSpriteComponent : Component 
    {
        public int TimeRemaining { get; set; }

        public ExplosiveType ExplosiveType { get; set;  }


    }
}
