using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using BlastersGame.Components;
using BlastersGame.Utilities;
using BlastersShared.Game.Components;
using BlastersShared.Game.Entities;
using BlastersShared.Services;
using BlastersShared.Services.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using BlastersShared.Game;
using PuzzleGame;

namespace BlastersGame.Services
{
    /// <summary>
    /// The sprite service is responsible for drawing entities that might have sprites.
    /// </summary>
    public class SpriteRenderingService : Service
    {
        // This is used to look up sprites for drawing. It's cached in memory for ease of use
        readonly Dictionary<string, SpriteDescriptor> _spriteDescriptorsLookup = new Dictionary<string, SpriteDescriptor>();
        private SpriteFont _entityFont;
        private float _lastAnimationTimer;

        public override void Initialize()
        {
            LoadDescriptors();

            // Load fonts
            _entityFont = ContentManager.Load<SpriteFont>(@"Fonts\Kootenay");

            // Listen for when an entity might hav ebeen added on
            ServiceManager.EntityAdded += ServiceManagerOnEntityAdded;
        }

        private void ServiceManagerOnEntityAdded(Entity entity)
        {
            // Attempt to add the sprite component
            AddSpriteComponent(entity);
        }

        private void LoadDescriptors()
        {
            // It's much easier to grab all the descriptors in one go
            // Then, they're all available in memory and there's nothing to worry about
            foreach (var file in Directory.GetFiles(PathUtility.SpriteDescriptorPath))
            {
                var descriptor = SpriteDescriptor.FromFile(file);
                _spriteDescriptorsLookup.Add(descriptor.Name, descriptor);
            }

            foreach (var entity in ServiceManager.Entities)
            {
                AddSpriteComponent(entity);
            }
        }

        private void AddSpriteComponent(Entity entity)
        {
            var skinComponent = (SkinComponent)entity.GetComponent(typeof(SkinComponent));

            if (skinComponent != null)
            {
                var spriteComponent = new SpriteComponent
                {
                    Texture =
                        ContentManager.GetTexture(_spriteDescriptorsLookup[skinComponent.SpriteDescriptorName].SpritePath,
                                                  ServiceManager.GraphicsDevice)           
                };

                entity.AddComponent(spriteComponent);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            foreach (var entity in ServiceManager.Entities)
            {
                var spriteComponent = (SpriteComponent)entity.GetComponent(typeof(SpriteComponent));
                var nameComponent = (NameComponent)entity.GetComponent(typeof(NameComponent));
                var transformComponent = (TransformComponent)entity.GetComponent(typeof(TransformComponent));

                if (spriteComponent != null)
                {
                    var skinComponent = (SkinComponent)entity.GetComponent(typeof(SkinComponent));
                    var descriptor = _spriteDescriptorsLookup[skinComponent.SpriteDescriptorName];
                    var sourceRectangle = new Rectangle((int)descriptor.FrameSize.X * spriteComponent.AnimationFrame, (int)(descriptor.FrameSize.Y * descriptor.Animations[(int)transformComponent.DirectionalCache].Row), (int)descriptor.FrameSize.X, (int)descriptor.FrameSize.Y);

                    spriteBatch.Draw(spriteComponent.Texture, transformComponent.LocalPosition, sourceRectangle, Color.White);

                    // If this sprite has a name
                    if (nameComponent != null)
                    {
                        var font = _entityFont;
                        var size = font.MeasureString(nameComponent.Name);
                        var namePos = transformComponent.LocalPosition;

                        Vector2 pos = namePos - new Vector2(0, 0);

                        pos = pos + new Vector2((int)(transformComponent.Size.X / 2), -20);
                        pos = pos - new Vector2((int)(size.X / 2), 0);

                        pos = new Vector2((float)Math.Round(pos.X), (float)Math.Round(pos.Y));

                        //Draw stroke
                        spriteBatch.DrawString(font, nameComponent.Name, pos + new Vector2(1, 0), Color.DarkRed);

                        spriteBatch.DrawString(font, nameComponent.Name, pos + new Vector2(-1, 0), Color.DarkRed);

                        spriteBatch.DrawString(font, nameComponent.Name, pos + new Vector2(0, 1), Color.DarkRed);
                        spriteBatch.DrawString(font, nameComponent.Name, pos + new Vector2(0, -1), Color.DarkRed);

                        spriteBatch.DrawString(font, nameComponent.Name, pos, Color.DarkBlue);
                    }

                }

            }

            spriteBatch.End();
        }

        private void UpdateAnimation() 
        {
            foreach (var entity in ServiceManager.Entities)
            {
                var spriteComponent = (SpriteComponent)entity.GetComponent(typeof(SpriteComponent));

                // Make sure the component exists
                if (spriteComponent != null)
                {
                    var transformComponent = (TransformComponent) entity.GetComponent(typeof (TransformComponent));
                    var skinComponent = (SkinComponent) entity.GetComponent(typeof (SkinComponent));
                    var descriptor = _spriteDescriptorsLookup[skinComponent.SpriteDescriptorName];

                    // Total amount of frames
                    var frameCount = ContentManager.GetTexture(_spriteDescriptorsLookup[skinComponent.SpriteDescriptorName].SpritePath,
                                                  ServiceManager.GraphicsDevice).Width / descriptor.FrameSize.X;

                    // We don't need to animate if there's only one frame
                    if ((int)frameCount == 1)
                    {
                        return;
                    }

                    // Don't animate if the player isn't moving
                    if ((int)transformComponent.Velocity.X != 0 | (int)transformComponent.Velocity.Y != 0)
                    {
                        // Change animation frame every 1/4 of a second
                        if (_lastAnimationTimer >= .25)
                        {
                            for (var i = 0; i <= frameCount - 1; i++)
                            {
                                if (spriteComponent.AnimationFrame < i)
                                {
                                    spriteComponent.AnimationFrame++;
                                    break;
                                }

                                if (i == (int)frameCount - 1)
                                {
                                    spriteComponent.AnimationFrame = 0;
                                    break;
                                }
                            }
     
                            _lastAnimationTimer = 0;
                        }
                    }
                    else
                        // Reset animation frame if they stop moving; default for moving objects
                        spriteComponent.AnimationFrame = 1;
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            _lastAnimationTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Update animation frames
            UpdateAnimation();

            //throw new NotImplementedException();
        }

        public override void HandleInput(InputState inputState)
        {
            //throw new NotImplementedException();
        }
    }
}
