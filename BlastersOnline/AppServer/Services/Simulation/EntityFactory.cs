using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlastersShared;
using BlastersShared.Game.Components;
using BlastersShared.Game.Entities;
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

            entity.AddComponent(transformComponent);
            entity.AddComponent(nameComponent);
            entity.AddComponent(skinComponent);
            entity.AddComponent(playerComponent);

            return entity;
        }


    }
}
