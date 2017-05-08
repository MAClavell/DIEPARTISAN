﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using GDAPSIIGame.Map;
using Microsoft.Xna.Framework.Input;

namespace GDAPSIIGame.Weapons
{
	class Rifle : Weapon
	{
		//Fields
		private float fireRate;
		private int clipSize;
		private int clip;
		private float reloadSpeed;
		private float fired;
		private float reload;
		private Vector2 origin;
		private Vector2 bulletOffset;
		private Owners owner;
		private SpriteEffects effects;

		public Rifle(ProjectileType pT, Texture2D texture, Vector2 position, Rectangle boundingBox, float fireRate, int clipSize, float reloadSpeed, Vector2 origin, Owners owner, Range range)
			: base(pT, texture, position, boundingBox, range)
        {
			this.fireRate = fireRate; //How fast until the weapon can fire again
			this.clipSize = clipSize; //How large the clip is
			this.clip = clipSize; //The current amount of bullets in the clip
			this.reloadSpeed = reloadSpeed; //How long it takes to reload
			this.reload = 0; //Timer for whether the uesr is reloading
			this.fired = 0; //Whether the weapon has fired
			this.origin = origin; //The origin point of the weapon (where the player holds it)
			this.bulletOffset = new Vector2(-boundingBox.Width / 2, boundingBox.Height / 4);
			this.owner = owner;
			effects = SpriteEffects.None;
		}

		/// <summary>
		/// Whether the weapon is reloading or not
		/// </summary>
		public override bool Reload
		{
			get { return reload > 0; }
			set
			{
				if (value)
				{
					reload = reloadSpeed;
				}
				else reload = 0;
			}
		}

		/// <summary>
		/// Whether the weapon is firing or not
		/// </summary>
		public override bool Fired
		{
			get { return fired > 0; }
			set
			{
				if (value)
				{
					fired = fireRate;
				}
				else fired = 0;
			}
		}

		/// <summary>
		/// The maximum amont of ammo in the clip
		/// </summary>
		public override int MaxAmmo
		{
			get { return clipSize; }
		}

		/// <summary>
		/// The current amount of ammo in the clip
		/// </summary>
		public override int CurrAmmo
		{
			get { return clip; }
		}

		public override void Update(GameTime gameTime)
		{
			

			switch (Dir)
			{
				case Weapons.Weapon_Dir.UpEast:
					this.X += 20;
					this.bulletOffset = new Vector2(BoundingBox.Width / 4, BoundingBox.Height / 4);
					break;
				case Weapons.Weapon_Dir.UpWest:
					this.X -= 20;
					this.bulletOffset = new Vector2(BoundingBox.Width / 4, BoundingBox.Height / 4);
					break;
				case Weapons.Weapon_Dir.UpLeft:
					this.X -= 20;
					this.bulletOffset = new Vector2(-BoundingBox.Width / 4, BoundingBox.Height / 4);
					break;
				case Weapons.Weapon_Dir.Left:
					this.X -= 20;
					this.bulletOffset = new Vector2(-BoundingBox.Width / 2, BoundingBox.Height / 4);
					break;
				case Weapons.Weapon_Dir.DownLeft:
					this.X -= 20;
					this.bulletOffset = new Vector2(-BoundingBox.Width, BoundingBox.Height / 4);
					break;
				case Weapons.Weapon_Dir.DownWest:
					this.X -= 20;
					this.bulletOffset = new Vector2(-BoundingBox.Width / 2, BoundingBox.Height / 4);
					break;
				case Weapons.Weapon_Dir.DownEast:
					this.X += 20;
					this.bulletOffset = new Vector2(-BoundingBox.Width / 2, BoundingBox.Height / 4);
					break;
				case Weapons.Weapon_Dir.DownRight:
					this.X += 20;
					this.bulletOffset = new Vector2(-BoundingBox.Width / 4, BoundingBox.Height / 4);
					break;
				case Weapons.Weapon_Dir.Right:
					this.X += 20;
					this.bulletOffset = new Vector2(BoundingBox.Width / 2, BoundingBox.Height / 4);
					break;
				case Weapons.Weapon_Dir.UpRight:
					this.X += 20;
					this.bulletOffset = new Vector2(BoundingBox.Width, BoundingBox.Height / 4);
					break;
			}

			//Control when user can fire again after just firing
			if (Fired)
			{
				//Increment fireTimer
				fired -= (float)gameTime.ElapsedGameTime.TotalSeconds;
				//Check if fireTimer meets the threshold
				if (!Fired)
				{
					//Allow the user to fire again and reset timer
					Fired = false;
				}
			}

			if (Reload)
			{
				//Inrement reloadTimer
				reload -= (float)gameTime.ElapsedGameTime.TotalSeconds;
				//Check if reloadTimer meets the threshold
				if (!Reload)
				{
					//Reload the clip
					clip = clipSize;
				}
			}
			base.Update(gameTime);
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
            switch (Dir)
            {
                case Weapons.Weapon_Dir.UpWest:
                case Weapons.Weapon_Dir.UpLeft:
                case Weapons.Weapon_Dir.Left:
                case Weapons.Weapon_Dir.DownLeft:
                case Weapons.Weapon_Dir.DownWest:
                    effects = SpriteEffects.None;
                    break;

                case Weapons.Weapon_Dir.UpEast:
                case Weapons.Weapon_Dir.DownEast:
                case Weapons.Weapon_Dir.DownRight:
                case Weapons.Weapon_Dir.Right:
                case Weapons.Weapon_Dir.UpRight:
                    effects = SpriteEffects.FlipVertically;
                    break;
            }

            if(effects == SpriteEffects.FlipVertically)
            {
                spriteBatch.Draw(
                texture: this.Texture,
                position: new Vector2(Camera.Instance.GetViewportPosition(Player.Instance).X + 10, Camera.Instance.GetViewportPosition(Player.Instance).Y + 35),
                origin: new Vector2(0, this.Texture.Height / 2),
                rotation: Angle - 4.8f,
                effects: effects,
                color: Color.White
                );
            }
            else if(effects == SpriteEffects.None)
            {
                spriteBatch.Draw(
                texture: this.Texture,
                position: new Vector2(Camera.Instance.GetViewportPosition(Player.Instance).X + 30, Camera.Instance.GetViewportPosition(Player.Instance).Y + 35),
                origin: new Vector2(0, this.Texture.Height / 2),
                rotation: Angle - 4.8f,
                effects: effects,
                color: Color.White
                );
            }
        }

		/// <summary>
		/// Tell the weapon it is time to reload
		/// </summary>
		public override void ReloadWeapon()
		{
			if (!Reload && clip < clipSize)
			{
				Reload = true;
			}
		}

		/// <summary>
		/// Fire a bullet from the weapon
		/// </summary>
		/// <param name="direction">The speed that the bullet is moving</param>
		public override bool Fire(Vector2 direction)
		{
			ControlManager controlManager = ControlManager.Instance;
			//Check if click condition is met
			if (controlManager.ControlPressed(Control_Types.Fire))
			{
				if (controlManager.ControlReleased(Control_Types.Fire))
				{
					if (!Fired && !Reload && clip <= 0)
					{
						Reload = true;
						return false;
					}
				}
				//Check user can fire or if they need to reload
				if (!Fired && !Reload && clip > 0)
				{
					Fired = true;
					clip--;

					//Take the gun's current angle (a property) and create a rotation matrix out of it
					Matrix rotationMatrix = Matrix.CreateRotationZ(Angle);
					//Take the rotation matrix and transform the offset vector by it
					//The offset vector is an approximation of where the muzzle should be when added to the bullet's position
					//Remember the bullet's position is the top left of its bouunding box
					Vector2 bulletPosition = Vector2.Transform(bulletOffset, rotationMatrix);

					//Create the bullet at the actual position of the bullet + the rotated position
					ProjectileManager.Instance.Clone(ProjType, Position + bulletPosition, direction, Angle, owner, WeapRange);
					return true;
				}
			}
			return false;
		}

		public override void ResetWeapon()
		{
            Reload = false;
            clip = clipSize;
			Angle = 0;
		}
	}
}
