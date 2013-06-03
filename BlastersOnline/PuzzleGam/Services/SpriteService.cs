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
    public class SpriteService : Service
    {
        // This is used to look up sprites for drawing. It's cached in memory for ease of use
        readonly Dictionary<string, SpriteDescriptor> _spriteDescriptorsLookup = new Dictionary<string, SpriteDescriptor>();
        private SpriteFont _entityFont;

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

                    var sourceRectangle = new Rectangle(0, (int)(descriptor.FrameSize.Y * descriptor.Animations[(int)transformComponent.DirectionalCache].Row), (int)descriptor.FrameSize.X, (int)descriptor.FrameSize.Y);
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

        public override void Update(GameTime gameTime)
        {
            //throw new NotImplementedException();
        }

        public override void HandleInput(InputState inputState)
        {
            //throw new NotImplementedException();
        }
    }
}
