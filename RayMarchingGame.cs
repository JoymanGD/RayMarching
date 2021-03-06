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
        private Vector3 CameraPosition;
        private const float PlayerSpeed = .06f;
        private const float CameraFollowSpeed = 1.5f;
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
            
            Controll();

            CameraFollow((float)gameTime.ElapsedGameTime.TotalSeconds);

            SetShaderValues();

            base.Update(gameTime);
        }

        private void Controll(){
            PlayerPosition += PlayerSpeed * new Vector3(GetAxis(Keys.A, Keys.D),
                                                        GetAxis(Keys.R, Keys.F),
                                                        GetAxis(Keys.W, Keys.S));
        }

        private void CameraFollow(float deltaTime){
            CameraPosition = Vector3.Lerp(CameraPosition, PlayerPosition, deltaTime * CameraFollowSpeed);
        }

        private void SetShaderValues(){
            RayMarching.Parameters["playerPos"].SetValue(PlayerPosition);

            RayMarching.Parameters["cameraPos"].SetValue(CameraPosition);
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
