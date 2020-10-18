﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameWindowsStarter
{
    class Background
    {
        Game1 game;
        Texture2D texture;
        BoundingRectangle Bounds;

        public Background( Game1 g, Texture2D t )
        {
            game = g;
            texture = t;

            Bounds.Width = 1280;
            Bounds.Height = 720;
            Bounds.X = 0;
            Bounds.Y = 0;
        }

        public void Draw( SpriteBatch sb )
        {
            sb.Draw(texture, Bounds, Color.White);
        }
    }
}
