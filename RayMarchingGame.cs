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
        private const float PlayerSpeed = .06f;

        public RayMarchingGame()
        {
            Graphics = new GraphicsDeviceManager(this);
            Graphics.GraphicsProfile = GraphicsProfile.HiDef;
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            RenderTarget = new RenderTarget2D(GraphicsDevice, Graphics.PreferredBackBufferWidth, Graphics.PreferredBackBufferHeight);
            RayMarching = Content.Load<Effect>("Shaders/RayMarching");

            base.Initialize();
        }

        protected override void LoadContent()
        {
            SpriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            PlayerPosition += new Vector3(GetAxis(Keys.A, Keys.D) * PlayerSpeed,
                                          GetAxis(Keys.W, Keys.S) * PlayerSpeed,
                                          0);

            RayMarching.Parameters["playerPos"].SetValue(PlayerPosition);

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
            base.Draw(gameTime);
        }
    }
}
