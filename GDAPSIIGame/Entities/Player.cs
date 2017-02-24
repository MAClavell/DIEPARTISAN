﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using GDAPSIIGame.Entities;
using System.Threading;
using System.Runtime.CompilerServices;

namespace GDAPSIIGame
{
	class Player : Entity
	{
		//Fields
		static private Player instance;
		private Weapon weapon;
		private Thread inputThread;
		private MouseState mouseState;
		private MouseState prevMouseState;
		private GameTime currentTime;
		private KeyboardState currentState;
		private KeyboardState prevState;
		private float hurting;
        private Color color;

		//Singleton

		private Player(Weapon weapon, int health, int moveSpeed, Texture2D texture, Vector2 position, Rectangle boundingBox) : base(health, moveSpeed, texture, position, boundingBox)
		{
			this.weapon = weapon;
			inputThread = null;
			mouseState = Mouse.GetState();
			prevMouseState = Mouse.GetState();
            hurting = 0;
            color = Color.White;
		}

		static public Player Instantiate(Weapon weapon, int health, int moveSpeed, Texture2D texture, Vector2 position, Rectangle boundingBox)
		{
			if (instance == null)
			{
				instance = new Player(weapon, health, moveSpeed, texture, position, boundingBox);
			}
			return instance;
		}


		//Properties

		/// <summary>
		/// The only instance of the player class
		/// </summary>
		static public Player Instance
		{
			get { return instance; }
		}

		/// <summary>
		/// The weapon the player is holding
		/// </summary>
		public Weapon Weapon
		{
			get { return weapon; }
			set { weapon = value; }
		}

        /// <summary>
        /// Whether the player is hurting or not
        /// </summary>
        public bool IsHurting
        {
            get { return hurting > 0; }
            set { if (value) { hurting = 0.5f; } }
        }

		/// <summary>
		/// The current GameTime
		/// </summary>
		public GameTime CurrentTime
		{
			get { return currentTime; }
			set { currentTime = value; }
		}

		/// <summary>
		/// The current state of the keyboard
		/// </summary>
		public KeyboardState CurrentState
		{
			[MethodImpl(MethodImplOptions.Synchronized)]
			get { return currentState; }
			set { currentState = value; }
		}

		/// <summary>
		/// The previous state of the keyboard
		/// </summary>
		public KeyboardState PrevState
		{
			[MethodImpl(MethodImplOptions.Synchronized)]
			get { return prevState; }
			set { prevState = value; }
		}

		//Methods
		public override void Update(GameTime gameTime)
        {
			currentTime = gameTime;
            base.Update(gameTime);
			weapon.Update(gameTime);

            if (inputThread == null)
            {
                inputThread = new Thread(() => parseInput(gameTime));
				inputThread.IsBackground = true;
                inputThread.Start();
            }

			//Mouse state
			prevMouseState = mouseState;
			mouseState = Mouse.GetState();

			//Current keyboard state
			PrevState = CurrentState;
			currentState = Keyboard.GetState();

			//Fire weapon only if previous frame didn't have left button being pressed
			if (mouseState.LeftButton == ButtonState.Pressed && prevMouseState.LeftButton == ButtonState.Released)
			{
				Vector2 direction = new Vector2((mouseState.X - instance.X) / 1000, (mouseState.Y - instance.Y) / 1000);
				direction.Normalize();
				this.Weapon.Fire(Position, direction);
			}

            //Find the right sprite to draw
            //Determine player direction and get the corresponding sprite
            switch (this.Dir)
            {
                case Entity_Dir.Up:
                    break;
                case Entity_Dir.UpLeft:
                    break;
                case Entity_Dir.Left:
                    break;
                case Entity_Dir.DownLeft:
                    break;
                case Entity_Dir.Down:
                    break;
                case Entity_Dir.DownRight:
                    break;
                case Entity_Dir.Right:
                    break;
                case Entity_Dir.UpRight:
                    break;
                default:
                    break;
            }

            //Determine if the player hurting color should be playing
            if (hurting > 0)
            {
                //Subtract from the hurting timer if the player is hurting
                hurting -= (float)gameTime.ElapsedGameTime.TotalSeconds;
			}
        }

		public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.Texture,
                this.Position,
                null,
                null,
                Vector2.Zero,
                0.0f,
                this.Scale,
                color,
                0);
        }

        /// <summary>
        /// Fires the player's weapon
        /// </summary>
        /// <param name="position"></param>
        /// <param name="direction"></param>
        public void Fire(Vector2 position, Vector2 direction)
        {
            weapon.Fire(position, direction);
        }

        /// <summary>
        /// Parses Input during updates
        /// </summary>
        /// <param name="kbState">KeyboardState</param>
        public void parseInput(GameTime gameTime)
        {
            float currTime = 0;
            float prevTime = 0;
            float deltaTime = 0;
			KeyboardState kbState = Keyboard.GetState();
			KeyboardState prevKbState = Keyboard.GetState();
            while (true)
            {
				//Update keyboards
				prevKbState = PrevState;
				kbState = CurrentState;

                deltaTime = (float)CurrentTime.ElapsedGameTime.TotalSeconds/200;
				//Console.WriteLine(deltaTime);
				//Console.WriteLine(CurrentTime.ElapsedGameTime.TotalSeconds);

                //Basic keyboard movement
                if (kbState.IsKeyDown(Keys.W) || kbState.IsKeyDown(Keys.Up))
                {
                    this.Y -= deltaTime;
                }
                else if (kbState.IsKeyDown(Keys.S) || kbState.IsKeyDown(Keys.Down))
                {
                    this.Y += deltaTime;
                }

                if (kbState.IsKeyDown(Keys.D) || kbState.IsKeyDown(Keys.Right))
                {
                    this.X += deltaTime;
                }
                else if (kbState.IsKeyDown(Keys.A) || kbState.IsKeyDown(Keys.Left))
                {
                    this.X -= deltaTime;
                }
				
                //Player reloading
				if (kbState.IsKeyDown(Keys.R) && prevKbState.IsKeyUp(Keys.R))
				{
					this.weapon.Reload();
				}

                //Calculates the angle between the player and the mouse
                //See below
                //   180
                //-90   90
                //    0
                float angle = MathHelper.ToDegrees((float)Math.Atan2(mouseState.X - Position.X, mouseState.Y - Position.Y));

                //Use angle to find player direction
                if ((angle < -157.5) || (angle > 157.5) && this.Dir != Entity_Dir.Up)
                {
                    this.Dir = Entity_Dir.Up;
					weapon.Dir = Weapon_Dir.Up;
                }
                else if ((angle < 157.5) && (angle > 112.5) && this.Dir != Entity_Dir.UpRight)
                {
                    this.Dir = Entity_Dir.UpRight;
					weapon.Dir = Weapon_Dir.UpRight;
				}
                else if ((angle < 112.5) && (angle > 67.5) && this.Dir != Entity_Dir.Right)
                {
                    this.Dir = Entity_Dir.Right;
					weapon.Dir = Weapon_Dir.Right;
				}
                else if ((angle < 67.5) && (angle > 22.5) && this.Dir != Entity_Dir.DownRight)
                {
                    this.Dir = Entity_Dir.DownRight;
					weapon.Dir = Weapon_Dir.DownRight;
				}
                else if ((angle < -22.5) && (angle > -67.5) && this.Dir != Entity_Dir.DownLeft)
                {
                    this.Dir = Entity_Dir.DownLeft;
					weapon.Dir = Weapon_Dir.DownLeft;
				}
                else if ((angle < -67.5) && (angle > -112.5) && this.Dir != Entity_Dir.Left)
                {
                    this.Dir = Entity_Dir.Left;
					weapon.Dir = Weapon_Dir.Left;
				}
                else if ((angle < -112.5) && (angle > -157.5) && this.Dir != Entity_Dir.UpLeft)
                {
                    this.Dir = Entity_Dir.UpLeft;
					weapon.Dir = Weapon_Dir.UpLeft;
				}
                else if ((angle < 22.5) && (angle > -22.5) && this.Dir != Entity_Dir.Down)
                {
                    this.Dir = Entity_Dir.Down;
					weapon.Dir = Weapon_Dir.Down;
				}
                //Thread.Sleep(1);
                //Console.WriteLine(angle);
                //Console.WriteLine(this.Dir);
            }
        }
    }
}
