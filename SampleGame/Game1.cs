﻿using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SampleGame
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont font;
        Model ship, destroyer;
        //Texture2D arrow; // noise
        int count;
        double cpu;
        double totalSeconds;

        private Matrix world = Matrix.Identity;
        double[] W =
        {
            2657.0/7919,
            1709.0/3517,
            3847.0/7457,
            5669.0/6029,
            4127.0/4691,
            3331.0/3343,
        };

        private Random random = new Random();
        Vector3 RandomVector(float a = 1)
        {
            return new Vector3
            {
                X = a * (float)(random.NextDouble() - 0.5),
                Y = a * (float)(random.NextDouble() - 0.5),
                Z = a * (float)(random.NextDouble() - 0.5),
            };
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
            IsMouseVisible = true;
            IsFixedTimeStep = false;
            Window.AllowAltF4 = true;
            Window.AllowUserResizing = true;
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

            // TODO: use this.Content to load your game content here
            font = Content.Load<SpriteFont>("arial");
            ship = Content.Load<Model>("Ship2");
            destroyer = Content.Load<Model>("Destroyer");
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

            // TODO: Add your update logic here

            totalSeconds = gameTime.TotalGameTime.TotalSeconds;
            var t = gameTime.TotalGameTime.TotalSeconds;

            var position = 6*new Vector3
            {
                X = (float)(Math.Sin(W[0] * t) + Math.Sin(W[3] * t)),
                Y = (float)(Math.Sin(W[1] * t) + Math.Sin(W[4] * t)),
                Z = (float)(Math.Sin(W[2] * t) + Math.Sin(W[5] * t)),
            };

            if (totalSeconds > 0.1)
                cpu = gameTime.ElapsedGameTime.TotalSeconds / totalSeconds;

            //position += RandomVector(0.1F);

            world = Matrix.CreateRotationY((float)t) * Matrix.CreateTranslation(position);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            count++;
            GraphicsDevice.Clear(Color.MidnightBlue);

            // TODO: Add your drawing code here
            Matrix view = Matrix.CreateLookAt(new Vector3(0, 0, 20), new Vector3(0, 0, 0), Vector3.UnitY);
            Matrix projection = Matrix.CreatePerspectiveFieldOfView((float)Math.PI/6, GraphicsDevice.Viewport.AspectRatio, 0.1f, 1000f);

            GraphicsDevice.BlendState = BlendState.Opaque;
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;

            DrawModel(destroyer, world, view, projection);

            DrawSprites();
            base.Draw(gameTime);
        }

        private void DrawSprites()
        {
            spriteBatch.Begin();
            spriteBatch.DrawString(font, "Mad Twience", new Vector2(50, 50), Color.Purple);

            if (totalSeconds > 1)
            {
                var fps = "FPS: " + (count / totalSeconds).ToString("F0") + " CPU: " + (100 * cpu).ToString("F0") + "%";
                spriteBatch.DrawString(font, fps, new Vector2(0, 0), Color.White);
            }

            spriteBatch.End();
        }

        private void DrawModel(Model model, Matrix world, Matrix view, Matrix projection)
        {
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    //effect.DirectionalLight0.DiffuseColor = new Vector3(1f, 0.7f, 0.7f); // a reddish light
                    //effect.DirectionalLight0.Direction = new Vector3(1, 0, 0);  // coming along the x-axis
                    //effect.DirectionalLight0.SpecularColor = new Vector3(0, 1, 0); // with green highlights

                    //effect.AmbientLightColor = new Vector3(0.2f, 0.2f, 0.2f); // Add some overall ambient light.
                    //effect.EmissiveColor = new Vector3(1, 0, 0); // Sets some strange emmissive lighting.  This just looks weird.

                    //effect.LightingEnabled = true; // turn on the lighting subsystem.
                    //effect.AmbientLightColor = new Vector3(0.4F, 0.4F, 0.74F);
                    //                    effect.DirectionalLight0.DiffuseColor = new Vector3(0.5f, 0, 0); // a red light
                    //                  effect.DirectionalLight0.Direction = new Vector3(1, 0, 0);  // coming along the x-axis
                    //                effect.DirectionalLight0.SpecularColor = new Vector3(0, 1, 0); // with green highlights

                    effect.World = world;
                    effect.View = view;
                    effect.Projection = projection;
                }

                mesh.Draw();
            }
        }
    }
}
