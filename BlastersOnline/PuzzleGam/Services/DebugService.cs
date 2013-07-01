using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BlastersShared.Game.Components;
using BlastersShared.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using BlastersGame;
using C3.XNA;

namespace BlastersGame.Services
{
    /// <summary>
    /// The debug service provides utilties for displaying debug information about entities. 
    /// </summary>
    public class DebugService : Service
    {

        public DebugService()
        {
            Visible = false;
        }

        /// <summary>
        /// Is the debug service active?
        /// </summary>
        public bool Visible { get; set; }

        public override void Draw(SpriteBatch spriteBatch)
        {

            if(!Visible)
                return;
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, null, null, null, null,
                              ServiceManager.Camera.GetTransformation());

            foreach (var entity in ServiceManager.Entities)
            {
                var explosiveComponent = (ExplosiveComponent) entity.GetComponent(typeof (ExplosiveComponent));

                if (explosiveComponent != null)
                {
                    var explosiveRectangles = DetonationHelper.GetBlastRadiusFrom(ServiceManager.Map.TmxMap,entity);

                  

                    foreach (var explosiveRectangle in explosiveRectangles)
                    {
                        spriteBatch.DrawRectangle(explosiveRectangle, Color.Red, 4f);
                    }

               
                }

            }

            spriteBatch.End();

        }

        public override void Update(GameTime gameTime)
        {
         
        }

        public override void HandleInput(InputState inputState)
        {
          
        }

        public override void Initialize()
        {
        
        }
    }
}
