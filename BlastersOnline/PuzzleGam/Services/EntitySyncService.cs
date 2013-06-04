using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BlastersShared.Network.Packets.AppServer;
using BlastersShared.Network.Packets.AppServer.BlastersShared.Network.Packets.AppServer;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using BlastersGame;
using BlastersGame.Network;

namespace BlastersGame.Services
{
    /// <summary>
    /// The entity sync service is responsbile for creating and removing entities in this particular service container instance.
    /// </summary>
    public class EntitySyncService : Service
    {
        public override void Draw(SpriteBatch spriteBatch)
        {
    
        }

        public override void Update(GameTime gameTime)
        {

        }

        public override void HandleInput(InputState inputState)
        {
          
        }

        public override void Initialize()
        {
            PacketService.RegisterPacket<EntityAddPacket>(HandleEntityAdd);
            PacketService.RegisterPacket<EntityRemovePacket>(HandleEntityRemove);
        }

        private void HandleEntityRemove(EntityRemovePacket entityRemovePacket)
        {
            ServiceManager.RemoveEntityByID(entityRemovePacket.EntityID);
        }


        /// <summary>
        /// Responsible for recieving and piping entities into the game network.
        /// </summary>
        /// <param name="entityAddPacket"></param>
        private void HandleEntityAdd(EntityAddPacket entityAddPacket)
        {
            ServiceManager.AddEntity(entityAddPacket.Entity);     
        }


    }
}
