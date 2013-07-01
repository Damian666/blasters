using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BlastersGame.Components;
using BlastersShared.Game.Components;
using BlastersShared.Game.Entities;
using Microsoft.Xna.Framework;

namespace BlastersGame
{
    /// <summary>
    /// This entity factory is used for client-sided entities like explosions and the like.
    /// </summary>
    public class EntityFactory
    {

        public static Entity CreateExplosionSprite(Vector2 position, ExplosiveType explosiveType)
        {
            var entity = new Entity();

            var transformComponent = new TransformComponent(position, new Vector2(32, 32));
            var skinComponent = new SkinComponent("ExplosiveSprite");
            var explosiveSpriteComponent = new ExplosionSpriteComponent();
            explosiveSpriteComponent.ExplosiveType = explosiveType;
            explosiveSpriteComponent.TimeRemaining = 1000;

            entity.AddComponent(transformComponent);
            entity.AddComponent(skinComponent);
            entity.AddComponent(explosiveSpriteComponent);
            entity.AddComponent(new OneShotAnimationComponent());

            return entity;

        }

        public static Entity CreateBreakingBlock(Vector2 position, string spriteName)
        {
            var entity = new Entity();

            var transformComponent = new TransformComponent(position, new Vector2(32, 32));
            var skinComponent = new SkinComponent("StandardBlock");

            entity.AddComponent(transformComponent);
            entity.AddComponent(skinComponent);
            entity.AddComponent(new OneShotAnimationComponent());

            return entity;
        }



    }
}
