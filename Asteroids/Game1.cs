using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using BEPUphysics;
using Microsoft.Xna.Framework.Audio;
using System;

namespace Asteroids
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public static Vector3 CameraPosition
        {
            get;
            set;
        }

        public static Vector3 CameraDirection
        {
            get;
            set;
        }

        public static Vector3 CameraUp
        {
            get;
            set;
        }

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
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
            //Make our BEPU Physics space a service
            Services.AddService<Space>(new Space());

            new Fighter(this, new Vector3(0, 0, 0), 1, new Vector3(0, 0, 0), new Vector3(0f, 0, 0));
            new Asteroid(this, new Vector3(-10, 0, 0), 2, new Vector3(0, 0, 0), new Vector3(0f, 0f, 0f));
            new Asteroid(this, new Vector3(10, 0, 0), 1, new Vector3(0, 0, 0), new Vector3(0f, 0f, 0));
            new Skybox(this);

            CameraPosition = new Vector3(0.0f, 0, 50.0f);
            CameraDirection = new Vector3(0, 0, -1);
            CameraUp = Vector3.Up;

            //CameraPosition = new Vector3(0.0f, 50.0f, (3.125f/1024.0f));
            //CameraDirection = new Vector3(0, -1, (-0.065f/1024.0f));

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            graphics.PreferredBackBufferWidth = GraphicsDevice.DisplayMode.Width * 9/10;
            graphics.PreferredBackBufferHeight = GraphicsDevice.DisplayMode.Height * 9/10;
            graphics.ApplyChanges();

            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            
            // TODO: use this.Content to load your game content here
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
            //Allow game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            //the time since Update was called last
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            Services.GetService<Space>().Update((float)gameTime.ElapsedGameTime.TotalSeconds);

            base.Update(gameTime);
        }

        

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
