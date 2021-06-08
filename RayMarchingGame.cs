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

            base.Update(gameTime);
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
