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
    class Text
    {
        Game1 game;

        // font to be used by the game
        private SpriteFont font;

        // Character textures
        Texture2D textureWaifu1;
        Texture2D textureHusbando1;
        Texture2D textureProtag;
        Texture2D textureWaifu1Dark;
        Texture2D textureHusbando1Dark;
        Texture2D textureProtagDark;

        Portrait waifu1;
        Portrait husbando1;
        Portrait protag;
        bool drawWaifu1;
        bool drawHusbando1;
        bool drawProtag;

        DialogueBox dialoguebox;

        // StreamReader to read the file and a string to store the line that was read
        System.IO.StreamReader file;
        string line;

        // Strings to store the currently displaying line
        string displayline1;
        string displayline2;
        string displayline3;

        // Variable that tells us how many characters a line can have (here for easy changing)
        int maxlinelength;

        // public Boolean variable to signal when we are ready for the next line to be read, and one for when we are still writing.
        public bool readready;
        public bool writing;

        // Boolean variables that signal when a line is done scrolling
        public bool line1drawdone;
        public bool line2drawdone;
        public bool line3drawdone;
        int line1drawprogress;
        int line2drawprogress;
        int line3drawprogress;

        public Text( Game1 g, DialogueBox db )
        {
            game = g;
            dialoguebox = db;
            file = new System.IO.StreamReader(@"..\..\..\..\dialogue.txt");
            readready = true;
            line1drawdone = false;
            line2drawdone = false;
            line3drawdone = false;
            writing = true;
            line1drawprogress = 0;
            line2drawprogress = 0;
            line3drawprogress = 0;

            displayline1 = "";
            displayline2 = "";
            displayline3 = "";

            maxlinelength = 70;
        }

        public void LoadContent( ContentManager content )
        {
            font = content.Load<SpriteFont>( "defaultfont" );

            // We need to control the character portraits from here too, because they are controlled from the file as well.
            textureWaifu1 = content.Load<Texture2D>("waifu1crop");
            textureHusbando1 = content.Load<Texture2D>("husbando1crop");
            textureProtag = content.Load<Texture2D>("protagcrop");
            textureWaifu1Dark = content.Load<Texture2D>("waifu1cropdark");
            textureHusbando1Dark = content.Load<Texture2D>("husbando1cropdark");
            textureProtagDark = content.Load<Texture2D>("protagcropdark");
            waifu1 = new Portrait( game, textureWaifu1 );
            husbando1 = new Portrait( game, textureHusbando1 );
            protag = new Portrait( game, textureProtag );
        }

        public void Update( GameTime gt )
        {
        begin:
            // If readready is true, it means we have a new line ready to read (the player has advanced the dialogue)
            if ( readready )
            {
                // Reset line draw variables, as this signal means we are going to start scrolling the lines again.
                readready = false;
                line1drawdone = false;
                line2drawdone = false;
                line3drawdone = false;
                writing = true;
                line1drawprogress = 0;
                line2drawprogress = 0;
                line3drawprogress = 0;

                line = file.ReadLine();

                if ( line != null )
                {
                    // Create a variable to use when displaying text and reading arguments that leaves out the initial command character.
                    string linetrim = line.Substring(1);

                    // If the line begins with '!' we are going to move a character portrait.
                    if (line[0] == '!')
                    {
                        if (line[1] == '1')
                        {
                            drawWaifu1 = true;

                            if (line[2] == 'l')
                                waifu1.Bounds.X = 0;
                            if (line[2] == 'r')
                                waifu1.Bounds.X = game.graphics.PreferredBackBufferWidth - waifu1.Bounds.Width;
                            if (line[2] == 'x')
                                drawWaifu1 = false;
                        }
                        else if (line[1] == '2')
                        {
                            drawHusbando1 = true;

                            if (line[2] == 'l')
                                husbando1.Bounds.X = 0;
                            if (line[2] == 'r')
                                husbando1.Bounds.X = game.graphics.PreferredBackBufferWidth - husbando1.Bounds.Width;
                            if (line[2] == 'x')
                                drawHusbando1 = false;
                        }
                        else if (line[1] == '3')
                        {
                            drawProtag = true;

                            if (line[2] == 'l')
                                protag.Bounds.X = 0;
                            if (line[2] == 'r')
                                protag.Bounds.X = game.graphics.PreferredBackBufferWidth - protag.Bounds.Width;
                            if (line[2] == 'x')
                                drawProtag = false;
                        }

                        readready = true;
                        goto begin;
                    }

                    // If the line begins with '<' we have standard dialogue. Split the line into 3 lines for display.
                    if ( line[0] == '<' )
                    {
                        //displayline1 = line.Substring( 1 );
                        int len = line.Length;
                        int sb1count = 0;
                        int sb2count = 0;
                        int sb3count = 0;
                        bool line1full = false;
                        bool line2full = false;
                        bool line3full = false;
                        StringBuilder sb1 = new StringBuilder("", maxlinelength);
                        StringBuilder sb2 = new StringBuilder("", maxlinelength);
                        StringBuilder sb3 = new StringBuilder("", maxlinelength);

                        // Go through the string looking at each individual word, then place it into the correct line top-to-bottom.
                        foreach( string word in linetrim.Split(' '))
                        {
                        restart:
                            if ( !line1full )
                            {
                                sb1count += word.Length + 1;
                                if ( sb1count > maxlinelength)
                                {
                                    line1full = true;
                                    goto restart;
                                }
                                else
                                {
                                    sb1.Append( word + " " );
                                }
                            }
                            else if ( !line2full )
                            {
                                sb2count += word.Length + 1;
                                if (sb2count > maxlinelength)
                                {
                                    line2full = true;
                                    goto restart;
                                }
                                else
                                {
                                    sb2.Append(word + " ");
                                }
                            }
                            else if ( !line3full )
                            {
                                sb3count += word.Length + 1;
                                if (sb3count > maxlinelength)
                                {
                                    line3full = true;
                                }
                                else
                                {
                                    sb3.Append(word + " ");
                                }
                            }
                        }

                        displayline1 = sb1.ToString();
                        displayline2 = sb2.ToString();
                        displayline3 = sb3.ToString();
                    }
                }
            }
        }

        public void Draw( SpriteBatch sb )
        {
            // Draw each line of text, a few letters at a time to create a scrolling effect.
        restart:
            if ( !line1drawdone )
            {
                if( line1drawprogress >= displayline1.Length )
                {
                    line1drawdone = true;
                    goto restart;
                }
                else
                {
                    line1drawprogress += 2;
                }
            }
            else if ( !line2drawdone )
            {
                if (line2drawprogress >= displayline2.Length)
                {
                    line2drawdone = true;
                    goto restart;
                }
                else
                {
                    line2drawprogress += 2;
                }
            }
            else if (!line3drawdone)
            {
                if (line3drawprogress >= displayline3.Length)
                {
                    line3drawdone = true;
                    writing = false;
                }
                else
                {
                    line3drawprogress += 2;
                }
            }

            // Draw characters
            if ( drawWaifu1 )
                waifu1.Draw( sb );
            if ( drawHusbando1 )
                husbando1.Draw( sb );
            if ( drawProtag )
                protag.Draw( sb );

            // Draw Dialogue Box
            dialoguebox.Draw( sb );

            // Ensure line progress marker does not go out of bounds
            if (line1drawprogress >= displayline1.Length || line1drawdone )
                line1drawprogress = displayline1.Length;
            if (line2drawprogress >= displayline2.Length || line2drawdone )
                line2drawprogress = displayline2.Length;
            if (line3drawprogress >= displayline3.Length || line3drawdone )
                line3drawprogress = displayline3.Length;

            // Draw all 3 lines of text
            sb.DrawString(font, displayline1.Substring(0, line1drawprogress), new Vector2(200, 550), Color.White);
            sb.DrawString(font, displayline2.Substring(0, line2drawprogress), new Vector2(200, 600), Color.White);
            sb.DrawString(font, displayline3.Substring(0, line3drawprogress), new Vector2(200, 650), Color.White);
        }
    }
}
