//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Dream.World;
//using Dream.World.Entities;
//using Lidgren.Network;
//using Microsoft.Xna.Framework;

//namespace Dream.Network
//{


//    /// <summary>
//    /// This packet is sent when the client requests a login
//    /// </summary>
//    [Serializable]
//    public class ClientLoginRequestPacket : Packet
//    {
//        /// <summary>
//        /// The username sent for the login request
//        /// </summary>
//        public string Username { get; set; }

//        public string Password { get; set; }
//    }


//    [Serializable]
//    public class ClientMovementRequest : Packet
//    {
//        /// <summary>
//        /// The position this player last reported as
//        /// </summary>
//        public Vector2 Position { get; set; }

//        /// <summary>
//        /// The last velocity this player reported as
//        /// </summary>
//        public Vector2 Velocity { get; set; }

//        public ClientMovementRequest(Vector2 velocity, Vector2 position)
//        {
//            Position = position;
//            Velocity = velocity;
//        }
//    }

//    [Serializable]
//    public class ServerLoginRequestReply : Packet
//    {

//        public string ReturnMessage { get; set; }

//        public ServerLoginRequestReply(string returnMessage)
//        {
//            ReturnMessage = returnMessage;
//        }
//    }

//    /// <summary>
//    /// The server will send this packet when the client needs to know information about an entitys latest state
//    /// Uses include map recievement, and recovery from extreme lag
//    /// </summary>
//    [Serializable]
//    public class ServerEntitySyncPacket : Packet
//    {
//        /// <summary>
//        /// The entity that is being sent for synchronization to the client
//        /// </summary>
//        public Entity Entity { get; set; }

//        public ServerEntitySyncPacket(Entity entity)
//        {
//            this.Entity = entity;
//        }
//    }

//    [Serializable]
//    public class ServerJoinGame : Packet
//    {
//        public ServerJoinGame(Guid playerIdentifier)
//        {
//            PlayerIdentifier = playerIdentifier;
//        }

//        /// <summary>
//        /// This string contains a message that will be sent to the user upon logging in
//        /// </summary>
//        public string MessageOfTheDay { get; set; }

//        /// <summary>
//        /// The identifier of the player for this session
//        /// </summary>
//        public Guid PlayerIdentifier { get; set; }
//    }

//    [Serializable]
//    public class ServerRecieveMap : Packet
//    {

//        public ServerRecieveMap(BaseMap map)
//        {
//            this.CurrentMap = map;
//        }

//        /// <summary>
//        /// This is the current map the user resides on, entities, name and all data in tact
//        /// </summary>
//        public BaseMap CurrentMap { get; set; }
//    }

//    [Serializable]
//    public class ServerEntityMovement : Packet
//    {

//        /// <summary>
//        /// The position this entity is at as of this syncronization
//        /// </summary>
//        public Vector2 Position { get; set; }

//        /// <summary>
//        /// The velocity of this entity at the point of syncronization
//        /// </summary>
//        public Vector2 Velocity { get; set; }

//        /// <summary>
//        /// The <see cref="Guid"/> of the Entity to position sync
//        /// </summary>
//        public Guid EntityIdentity { get; set; }

//        public ServerEntityMovement(Entity entity)
//        {
//            Position = entity.Position;
//            Velocity = entity.Velocity;
//            EntityIdentity = entity.Identifier;
//        }
//    }

//}
