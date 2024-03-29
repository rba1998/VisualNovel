﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Runtime.InteropServices;

namespace MonoGameWindowsStarter
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        public GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        // Textures
        Texture2D textureDialogueBox;
        Texture2D textureDialogueTick;
        Texture2D textureNameBox;

        // Background textures
        Texture2D bgLibrary;
        Texture2D bgBlack;
        Texture2D cgPinky;

        // Music and sfx
        Song ScaryTheme;
        Song AbstractVision;
        SoundEffect sfxHeartbeat;

        // Onscreen objects
        DialogueBox dialoguebox;
        NameBox namebox;
        Text text;
        DialogueTick dialoguetick;
        Background bg;

        // Mouse states
        MouseState currentMouseState;
        MouseState lastMouseState;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            currentMouseState = Mouse.GetState();
            lastMouseState = Mouse.GetState();
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
            graphics.ApplyChanges();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // Load Textures
            textureDialogueBox = Content.Load<Texture2D>( "dialogueTEMP" );
            textureNameBox = Content.Load<Texture2D>( "nameTEMP" );
            textureDialogueTick = Content.Load<Texture2D>( "dialogueAnimationTiny" );
            bgLibrary = Content.Load<Texture2D>( "Library Background color" );
            bgBlack = Content.Load<Texture2D>( "bgBlack" );
            cgPinky = Content.Load<Texture2D>( "PinkyCG" );

            // Load Sound
            ScaryTheme = Content.Load<Song>("ScaryTheme5v2");
            AbstractVision = Content.Load<Song>("AbstractVision5");
            sfxHeartbeat = Content.Load<SoundEffect>("heartbeat");
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Volume = 0.5f;

            // Create new instances of necessary objects and controllers
            dialoguebox = new DialogueBox( this, textureDialogueBox );
            namebox = new NameBox( this, textureNameBox );
            text = new Text( this, dialoguebox, namebox );
            dialoguetick = new DialogueTick( this, textureDialogueTick );
            bg = new Background( this, bgLibrary );
            text.LoadContent( Content );
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            lastMouseState = currentMouseState;
            currentMouseState = Mouse.GetState();

            // If the mouse is clicked, finish writing out the current line. If the line is already done writing, advance to the next line of text.
            if ( currentMouseState.LeftButton == ButtonState.Pressed && lastMouseState.LeftButton == ButtonState.Released )
            {
                if ( !text.writing )
                {
                    text.readready = true;
                }
                else
                {
                    text.writing = false;
                    text.line1drawdone = true;
                    text.line2drawdone = true;
                    text.line3drawdone = true;
                }
            }

            // Change background if the text reader has read a background change command
            if ( text.changeBackground )
            {
                text.changeBackground = false;

                switch ( text.newBackground )
                {
                    case "Library":
                        bg.texture = bgLibrary;
                        break;
                    case "Black":
                        bg.texture = bgBlack;
                        break;
                    case "Pinky":
                        bg.texture = cgPinky;
                        break;
                    default:
                        bg.texture = bgBlack;
                        break;
                }
            }

            if ( text.changeMusic )
            {
                text.changeMusic = false;
                switch ( text.newMusic )
                {
                    case "Abstract":
                        MediaPlayer.Play( AbstractVision );
                        break;
                    case "Scary":
                        MediaPlayer.Play( ScaryTheme );
                        break;
                    default:
                        break;
                }
            }

            if ( text.playSfx )
            {
                text.playSfx = false;
                switch ( text.sfx )
                {
                    case "Heartbeat":
                        sfxHeartbeat.Play();
                        break;
                    default:
                        break;
                }
            }

            // TODO: Add your update logic here
            text.Update( gameTime );
            dialoguetick.Update( gameTime );

            base.Update( gameTime );
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();

            bg.Draw( spriteBatch );
            text.Draw( spriteBatch );
            if( !text.writing && !text.readready )
            {
                dialoguetick.Draw( spriteBatch );
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
