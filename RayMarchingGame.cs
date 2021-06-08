using System.Collections.Specialized;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace RayMarchingDirectX
{
    public class RayMarchingGame : Game
    {
        private GraphicsDeviceManager Graphics;
        private SpriteBatch SpriteBatch;
        private Effect RayMarching;
        private RenderTarget2D RenderTarget;
        private Vector3 PlayerPosition;
        private Vector2 CameraPosition;
        private const float PlayerSpeed = .06f;
        private const float CameraSpeed = .1f;
        private SpriteFont DefaultFont;

        public RayMarchingGame()
        {
            Graphics = new GraphicsDeviceManager(this);
            Graphics.GraphicsProfile = GraphicsProfile.HiDef;

            Graphics.PreferredBackBufferHeight = 800;
            Graphics.PreferredBackBufferWidth = 800;
            Graphics.ApplyChanges();

            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            RenderTarget = new RenderTarget2D(GraphicsDevice, 800, 800);//Graphics.PreferredBackBufferWidth, Graphics.PreferredBackBufferHeight);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            SpriteBatch = new SpriteBatch(GraphicsDevice);

            RayMarching = Content.Load<Effect>("Shaders/RayMarching");
            DefaultFont = Content.Load<SpriteFont>("Fonts/Default");

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            PlayerPosition += PlayerSpeed * new Vector3(GetAxis(Keys.A, Keys.D),
                                          GetAxis(Keys.R, Keys.F),
                                          GetAxis(Keys.W, Keys.S));

            RayMarching.Parameters["playerPos"].SetValue(PlayerPosition);

            CameraPosition += CameraSpeed * new Vector2(GetAxis(Keys.Left, Keys.Right),
                                          GetAxis(Keys.Up, Keys.Down));

            RayMarching.Parameters["cameraPos"].SetValue(CameraPosition);

            base.Update(gameTime);
        }

        private float GetAxis(Keys _positive, Keys _negative){
            return Convert.ToInt32(Keyboard.GetState().IsKeyDown(_positive)) - Convert.ToInt32(Keyboard.GetState().IsKeyDown(_negative));
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            SpriteBatch.Begin(effect: RayMarching);
            SpriteBatch.Draw(RenderTarget, Vector2.Zero, null, Color.White);
            SpriteBatch.End();

            SpriteBatch.Begin();
            SpriteBatch.DrawString(DefaultFont, "Movement: WASD + RF", new Vector2(30, 30), Color.IndianRed);
            SpriteBatch.End();
            
            base.Draw(gameTime);
        }
    }
}
