﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;



namespace GDAPSIIGame
{
    public class GameObject
    {
        private Texture2D texture;
        private Vector2 position;
        private Rectangle boundingBox;
        private Vector2 scale;

        public GameObject(Texture2D texture, Vector2 position, Rectangle boundingBox) {
            this.texture = texture;
            this.position = position;
            this.boundingBox = boundingBox;
            Console.WriteLine(texture.Width);
            Console.WriteLine(boundingBox.Width);
            scale = new Vector2((float)boundingBox.Width/texture.Width,  (float)boundingBox.Height/texture.Height);
            Console.WriteLine(scale);
        }

        public float X {
            get { return position.X; }
            set { position.X = value; }
        }

        public float Y
        {
            get { return position.Y; }
            set { position.Y = value; }
        }

        public Vector2 Position
        {
            get { return position; }
        }

        public Texture2D Texture
        {
            set { texture = value; }
        }

		public Rectangle BoundingBox
		{
			get { return boundingBox; }
		}

        public virtual void Draw(SpriteBatch sb) {
            sb.Draw(texture,
                position,
                null,
                null,
                Vector2.Zero,
                0.0f,
                scale,
                null,
                0);
        }

        public virtual void Update(GameTime gameTime)
        {
            boundingBox.X = (int)position.X;
            boundingBox.Y = (int)position.Y;
        }

        public bool Collide(Vector2 pos)
        {
            if (pos.X < position.X) { return false; }
            if (pos.X > (position.X + boundingBox.Width)) { return false; }
            if (pos.Y < position.Y) { return false; }
            if (pos.Y > (position.Y + boundingBox.Height)) { return false; }
            return true;
        }

    }
}
