using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BlastersGame.Components;
using BlastersShared.Game.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BlastersGame.Services
{
    public class ExplosionSpriteService : Service
    {
        public override void Draw(SpriteBatch spriteBatch)
        {
         
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.NonPremultiplied, null, null, null, null, ServiceManager.Camera.GetTransformation());

            foreach (var entity in ServiceManager.Entities)
            {
                var spriteExplosion = (ExplosionSpriteComponent) entity.GetComponent(typeof (ExplosionSpriteComponent));
                var spriteComponent = (SpriteComponent) entity.GetComponent(typeof (SpriteComponent));
                var transformComponent = (TransformComponent) entity.GetComponent(typeof (TransformComponent));

                if (spriteExplosion != null)
                {
                    var descriptor = spriteComponent.SpriteDescriptor;
                                        var sourceRectangle = new Rectangle(
                        (int)descriptor.FrameSize.X * spriteComponent.AnimationFrame, 
                        (int)(descriptor.FrameSize.Y * descriptor.Animations[(int) spriteExplosion.ExplosiveType].Row), 
                        (int)descriptor.FrameSize.X, (int)descriptor.FrameSize.Y);

                    spriteBatch.Draw(spriteComponent.Texture, transformComponent.LocalPosition, sourceRectangle, Color.White);
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
