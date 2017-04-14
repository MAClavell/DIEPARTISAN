﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace GDAPSIIGame
{
	class TextureManager
	{
		//Fields-----------------
		static private TextureManager instance;
		Dictionary<String, Texture2D> playerTextures;
		Dictionary<String, Texture2D> enemyTextures;
		Dictionary<String, Texture2D> bulletTextures;
		Dictionary<String, Texture2D> weaponTextures;
		Dictionary<String, Texture2D> roomTextures;
		Dictionary<String, Texture2D> mouseTextures;

		//Methods----------------

		/// <summary>
		/// Singleton Constructor
		/// </summary>
		private TextureManager()
		{
			playerTextures = new Dictionary <String, Texture2D>();
			enemyTextures = new Dictionary<String, Texture2D>();
			bulletTextures = new Dictionary<String, Texture2D>();
			weaponTextures = new Dictionary<String, Texture2D>();
			roomTextures = new Dictionary<String, Texture2D>();
			mouseTextures = new Dictionary<String, Texture2D>();
		}

		/// <summary>
		/// Singleton access
		/// </summary>
		/// <returns></returns>
		static public TextureManager Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new TextureManager();
				}
				return instance;
			}
		}

		//Properties
		public Dictionary<String, Texture2D> PlayerTextures
		{ get { return playerTextures; } }

		public Dictionary<String, Texture2D> EnemyTextures
		{ get { return enemyTextures; } }

		public Dictionary<String, Texture2D> BulletTextures
		{ get { return bulletTextures; } }

		public Dictionary<String, Texture2D> WeaponTextures
		{ get { return weaponTextures; } }

		public Dictionary<String, Texture2D> RoomTextures
		{ get { return roomTextures; } }

		public Dictionary<String, Texture2D> MouseTextures
		{ get { return mouseTextures; } }

		/// <summary>
		/// Load in sprites
		/// </summary>
		internal void LoadContent(ContentManager Content)
		{
			//Load player textures
			playerTextures.Add("PlayerTexture", Content.Load<Texture2D>("spr_player"));

			//Load enemy textures
			enemyTextures.Add("EnemyTexture", Content.Load<Texture2D>("playerNew"));

			//Load bullet textures
			bulletTextures.Add("PlayerBullet", Content.Load<Texture2D>("playerBullet"));

			//Load weapon textures
			weaponTextures.Add("PistolTexture", Content.Load<Texture2D>("playerBullet"));

			//Load mouse textures
			mouseTextures.Add("MousePointer", Content.Load<Texture2D>("playerNew"));

			//Load room textures
			roomTextures.Add("WallTexture", Content.Load<Texture2D>("playerBullet"));
			roomTextures.Add("FloorTexture", Content.Load<Texture2D>("playerNew"));
		}
	}
}
