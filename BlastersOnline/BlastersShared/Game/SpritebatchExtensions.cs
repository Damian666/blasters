using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BlastersShared.Game
{
    public static class SpriteBatchExtensions
    {
        public static void DrawStringCentered(this SpriteBatch spriteBatch, SpriteFont spriteFont, String text, float x, Single y, Color color)
        {
            Vector2 textBounds = spriteFont.MeasureString(text);
            Single centerX = x * 0.5f - textBounds.X * 0.5f;

            spriteBatch.DrawString(spriteFont, text, new Vector2(centerX, y), color);
        }
    }
}
