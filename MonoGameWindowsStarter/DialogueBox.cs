using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameWindowsStarter
{
    class DialogueBox
    {
        Game1 game;
        Texture2D texture;
        BoundingRectangle Bounds;

        public DialogueBox( Game1 g, Texture2D t )
        {
            game = g;
            texture = t;

            Bounds.Width = 1000;
            Bounds.Height = 200;
            Bounds.X = ( game.GraphicsDevice.Viewport.Width / 2 ) - ( Bounds.Width / 2 );
            Bounds.Y = game.GraphicsDevice.Viewport.Height - Bounds.Height - 10;
        }

        public void Draw( SpriteBatch sb )
        {
            sb.Draw(texture, Bounds, Color.White);
        }
    }
}
