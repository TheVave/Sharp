using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharpPhysics;
using System.Linq;

namespace SharpInterface
{
	public class Game1 : Game
	{
		private GraphicsDeviceManager _graphics;
		private SpriteBatch _spriteBatch;

		Texture2D ballTexture;
		Vector2 ballPosition;
		float ballSpeed;
		int updateTick = 0;

		private _2dSimulatedObject[] VObjects;

		public Game1()
		{
			_graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
			IsMouseVisible = true;
		}

		protected override void Initialize()
		{
			// TODO: Add your initialization logic here
			ballPosition = new Vector2(_graphics.PreferredBackBufferWidth / 2,
			_graphics.PreferredBackBufferHeight / 2);
			ballSpeed = 100f;
			Window.AllowUserResizing = true;

			VObjects = new _2dSimulatedObject[]
			{
				new(new(new double[] { 35, -35, -35, 35 }, new double[] { 35, 35, -35, -35 }), new(), new()),
				new(new(new double[] { 35, -35, -35, 35 }, new double[] { 35, 35, -35, -35 }), new(), new())
			};
			VObjects[0].Translation.ObjectPosition.xPos = 100;
			VObjects[1].Translation.ObjectPosition.xPos = 100;


			VObjects[0].ObjectPhysicsParams.SpeedResistance = 0.1;
			VObjects[1].ObjectPhysicsParams.SpeedResistance = 0.1;

			VObjects[0].ObjectPhysicsParams.GravityMultiplier = 0;
			VObjects[1].ObjectPhysicsParams.GravityMultiplier = 0;

			VObjects[1].RegisterToScene();
			VObjects[0].ObjectPhysicsParams.CollidableObjects = VObjects[0].ObjectPhysicsParams.CollidableObjects.Append(VObjects[1]).ToArray();
			VObjects[0].StartPhysicsSimulation();
			VObjects[1].StartPhysicsSimulation();

			base.Initialize();
		}

		protected override void LoadContent()
		{
			// Create a new SpriteBatch, which can be used to draw textures.
			_spriteBatch = new SpriteBatch(GraphicsDevice);

			// TODO: use this.Content to load your game content here
			ballTexture = Content.Load<Texture2D>("Secondary_Test_Image");
		}

		protected override void Update(GameTime gameTime)
		{
			//if (updateTick == 50) { VObjects[0].ObjectPhysicsParams.Momentum[0] = 0.1; }
			updateTick++;

			var kstate = Keyboard.GetState();

			if (kstate.IsKeyDown(Keys.Up))
			{
				VObjects[0].ObjectPhysicsParams.Momentum[1] = 1;
			}

			if (kstate.IsKeyDown(Keys.Down))
			{
				VObjects[0].ObjectPhysicsParams.Momentum[1] = -1;
			}

			if (kstate.IsKeyDown(Keys.Left))
			{
				VObjects[0].ObjectPhysicsParams.Momentum[0] = 1;
			}

			if (kstate.IsKeyDown(Keys.Right))
			{
				VObjects[0].ObjectPhysicsParams.Momentum[0] = -1;
			}

			if (kstate.IsKeyDown(Keys.Q))
			{
				VObjects[0].ObjectPhysicsParams.RotationalMomentum = 1;
			}

			if (kstate.IsKeyDown(Keys.E))
			{
				VObjects[0].ObjectPhysicsParams.RotationalMomentum = -1;
			}


			if (kstate.IsKeyDown(Keys.Z))
			{
				VObjects[1].ObjectPhysicsParams.RotationalMomentum = 1;
			}

			if (kstate.IsKeyDown(Keys.C))
			{
				VObjects[1].ObjectPhysicsParams.RotationalMomentum = -1;
			}

			if (kstate.IsKeyDown(Keys.W))
			{
				VObjects[1].ObjectPhysicsParams.Momentum[1] = 1;
			}

			if (kstate.IsKeyDown(Keys.S))
			{
				VObjects[1].ObjectPhysicsParams.Momentum[1] = -1;
			}

			if (kstate.IsKeyDown(Keys.A))
			{
				VObjects[1].ObjectPhysicsParams.Momentum[0] = 1;
			}

			if (kstate.IsKeyDown(Keys.D))
			{
				VObjects[1].ObjectPhysicsParams.Momentum[0] = -1;
			}

			base.Update(gameTime);
		}

		protected override void Draw(GameTime gameTime)
		{
			_graphics.GraphicsDevice.Clear(Microsoft.Xna.Framework.Color.Blue);
			_spriteBatch.Begin();
			foreach (_2dSimulatedObject obj in VObjects)
			{
				_spriteBatch.Draw(
				ballTexture,
				ballPosition,
				null,
				Microsoft.Xna.Framework.Color.White,
				obj.Translation.ObjectRotation.xRot,
				new Vector2((float)obj.Translation.ObjectPosition.xPos, (float)obj.Translation.ObjectPosition.yPos),
				Vector2.One,
				SpriteEffects.None,
				0f
				);
			}
			_spriteBatch.End();

			// TODO: Add your drawing code here

			base.Draw(gameTime);
		}
	}
}
