using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameWindowsStarter
{
    class DialogueTick
    {
        Game1 game;

        Texture2D texture;

        BoundingRectangle Bounds;
        Rectangle sheetLocation;

        public DialogueTick( Game1 g, Texture2D t )
        {
            game = g;
            texture = t;
            Bounds.Width = 146;
            Bounds.Height = 89;
            Bounds.X = 1090;
            Bounds.Y = 600;

            sheetLocation = new Rectangle( 0, 0, 146, 89 );
        }

        public void Update( GameTime gt )
        {
            int x = sheetLocation.X;

            x += 146;
            if( x >= 3650 )
            {
                int y = sheetLocation.Y;

                sheetLocation.X = 0;
                if ( y == 0 )
                {
                    sheetLocation.Y = 89;
                }
                else
                {
                    sheetLocation.Y = 0;
                }
            }
            else
            {
                sheetLocation.X = x;
            }
        }

        public void Draw( SpriteBatch sb )
        {
            sb.Draw( texture, Bounds, sheetLocation, Color.White );
        }
    }
}
