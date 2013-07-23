using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlastersShared;
using BlastersShared.Game.Components;
using BlastersShared.Game.Components.PowerUp;
using BlastersShared.Game.Entities;
using BlastersShared.Models;
using Microsoft.Xna.Framework;

namespace AppServer.Services.Simulation
{
    /// <summary>
    /// An entity factory is used for creating entities quickly and effectively from a set of blueprints.
    /// This makes generation of multiple objects incredibly simple.
    /// </summary>
    public static class EntityFactory
    {
        /// <summary>
        /// Creates a player from a given user object - this is an entity ready to be sent into a simulation game.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static Entity CreatePlayer(User user, Vector2 location)
        {
            var entity = new Entity();

            var transformComponent = new TransformComponent(location, new Vector2(50, 70));
            var nameComponent = new NameComponent(user.Name);
            var skinComponent = new SkinComponent(user.SessionConfig.Skin);
            var playerComponent = new PlayerComponent();
            var bombModifier = new BombCountModifierComponent();
            var rangeModifier = new RangeModifierComponent();
            var movementModifier = new MovementModifierComponent();

            // Add modifier components to the sprite

            playerComponent.Connection = user.Connection;
            playerComponent.SecureToken = user.SecureToken;

            entity.AddComponent(transformComponent);
            entity.AddComponent(nameComponent);
            entity.AddComponent(skinComponent);
            entity.AddComponent(playerComponent);
            entity.AddComponent(bombModifier);
            entity.AddComponent(rangeModifier);
            entity.AddComponent(movementModifier);

            return entity;
        }

        public static Entity CreateBomb(Vector2 location, Entity user)
        {
            var entity = new Entity();

            var rangeModifier = (RangeModifierComponent)user.GetComponent(typeof(RangeModifierComponent));

            var transformComponent = new TransformComponent(location, new Vector2(32, 32));
            var skinComponent = new SkinComponent("BombSprite");
            var explosiveComponent = new ExplosiveComponent(3f, rangeModifier.Amount, user.ID);

            entity.AddComponent(transformComponent);
            entity.AddComponent(skinComponent);
            entity.AddComponent(explosiveComponent);

            return entity;
        }

        public static Entity CreateRangeModifierPowerupPackage(Vector2 location)
        {
            var entity = new Entity();

            var rangeModifier = new RangeModifierComponent();
            var list = new List<PowerUpComponent>();
            var transform = new TransformComponent(location, new Vector2(32, 32));
            list.Add(rangeModifier);
            var powerupPackage = new PowerUpCollectionComponent(list);
            var skinComponent = new SkinComponent(rangeModifier.SkinName);

            entity.AddComponent(powerupPackage);
            entity.AddComponent(skinComponent);
            entity.AddComponent(transform);

            return entity;
        }


        public static Entity CreateBombCountUpPackage(Vector2 location)
        {
            var entity = new Entity();

            var rangeModifier = new BombCountModifierComponent();
            var list = new List<PowerUpComponent>();
            var transform = new TransformComponent(location, new Vector2(32, 32));
            list.Add(rangeModifier);
            var powerupPackage = new PowerUpCollectionComponent(list);
            var skinComponent = new SkinComponent(rangeModifier.SkinName);

            entity.AddComponent(powerupPackage);
            entity.AddComponent(skinComponent);
            entity.AddComponent(transform);

            return entity;
        }

        public static Entity CreateBombCountMaxPackage(Vector2 location)
        {
            var entity = new Entity();

            var rangeModifier = new BombCountModifierComponent();
            rangeModifier.Strength = 50;
            var list = new List<PowerUpComponent>();
            var transform = new TransformComponent(location, new Vector2(32, 32));
            list.Add(rangeModifier);
            var powerupPackage = new PowerUpCollectionComponent(list);
            var skinComponent = new SkinComponent(rangeModifier.SkinName.Replace("Up", "Max"));

            entity.AddComponent(powerupPackage);
            entity.AddComponent(skinComponent);
            entity.AddComponent(transform);

            return entity;
        }

        public static Entity CreateRangeModifierMaxPowerupPackage(Vector2 location)
        {
            var entity = new Entity();

            var rangeModifier = new RangeModifierComponent();
            rangeModifier.Strength = 50;
            var list = new List<PowerUpComponent>();
            var transform = new TransformComponent(location, new Vector2(32, 32));
            list.Add(rangeModifier);
            var powerupPackage = new PowerUpCollectionComponent(list);
            var skinComponent = new SkinComponent(rangeModifier.SkinName.Replace("Up", "Max"));

            entity.AddComponent(powerupPackage);
            entity.AddComponent(skinComponent);
            entity.AddComponent(transform);

            return entity;
        }

        public static Entity CreateMovementModifierPackage(Vector2 location)
        {
            var entity = new Entity();

            var movementModifier = new MovementModifierComponent();
            movementModifier.Strength = 1;

            var list = new List<PowerUpComponent>();
            var transform = new TransformComponent(location, new Vector2(32, 32));
            list.Add(movementModifier);
            var movementPackage = new PowerUpCollectionComponent(list);
            var skinComponent = new SkinComponent(movementModifier.SkinName);

            entity.AddComponent(movementPackage);
            entity.AddComponent(skinComponent);
            entity.AddComponent(transform);

            return entity;
        }

    }
}
