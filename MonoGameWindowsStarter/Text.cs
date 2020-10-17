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

        // StreamReader to read the file and a string to store the line that was read
        System.IO.StreamReader file;
        string line;

        // Strings to store the currently displaying line
        string displayline1;
        string displayline2;
        string displayline3;

        // Variable that tells us how many characters a line can have (here for easy changing)
        int maxlinelength;

        // Boolean variable to signal when we are ready for the next line to be read.
        bool readready;

        // Boolean variables that signal when a line is done scrolling
        bool line1drawdone;
        bool line2drawdone;
        bool line3drawdone;
        int line1drawprogress;
        int line2drawprogress;
        int line3drawprogress;

        public Text( Game1 g )
        {
            game = g;
            file = new System.IO.StreamReader(@"..\..\..\..\dialogue.txt");
            readready = true;
            line1drawdone = false;
            line2drawdone = false;
            line3drawdone = false;
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
        }

        public void Update( GameTime gt )
        {
            // If readready is true, it means we have a new line ready to read (the player has advanced the dialogue)
            if ( readready )
            {
                // Reset line draw variables, as this signal means we are going to start scrolling the lines again.
                readready = false;
                line1drawdone = false;
                line2drawdone = false;
                line3drawdone = false;
                line1drawprogress = 0;
                line2drawprogress = 0;
                line3drawprogress = 0;

                line = file.ReadLine();

                if ( line != null )
                {
                    // Create a variable to use when displaying text and reading arguments that leaves out the initial command character.
                    string linetrim = line.Substring(1);

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
                                    goto restart;
                                }
                                else
                                {
                                    sb3.Append(word + " ");
                                }
                            }
                        }
                        line1full = false;
                        line2full = false;
                        line3full = false;

                        displayline1 = sb1.ToString();
                        displayline2 = sb2.ToString();
                        displayline3 = sb3.ToString();
                    }
                }
            }
        }

        public void Draw( SpriteBatch sb )
        {
            restart:
            if ( !line1drawdone )
            {
                if( line1drawprogress >= displayline1.Length )
                {
                    if (line1drawprogress >= displayline1.Length)
                        line1drawprogress = displayline1.Length;

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
                    if (line2drawprogress >= displayline2.Length)
                        line2drawprogress = displayline2.Length;

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
                    if (line3drawprogress >= displayline3.Length)
                        line3drawprogress = displayline3.Length;

                    line3drawdone = true;
                    goto restart;
                }
                else
                {
                    line3drawprogress += 2;
                }
            }

            sb.DrawString(font, displayline1.Substring(0, line1drawprogress), new Vector2(200, 550), Color.White);
            sb.DrawString(font, displayline2.Substring(0, line2drawprogress), new Vector2(200, 600), Color.White);
            sb.DrawString(font, displayline3.Substring(0, line3drawprogress), new Vector2(200, 650), Color.White);
        }
    }
}
