using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Amoveo_Port
{
    class Pixel
    {
        // Pixel Variables
        public Texture2D texture;
        public Vector2 position, direction;
        public Color color = Color.White;
        public int counter = 30;

        public Pixel()
        {
            // Empty constructor
        }

        public void Initialize(ContentManager Content, Vector2 Position, int offSet)
        {
            // Load position from paramater, init direction
            // Texture loading needing to be here for some reason
            texture = Content.Load<Texture2D>("Spark");
            //Random rng = new Random(offSet);
            //Position.Y += rng.Next(-offSet, offSet);
            //Position.X += rng.Next(-offSet, offSet);
            position = Position;
            direction = new Vector2();
        }

        public void LoadContent()
        {


        }

        public void UnloadContent()
        {

        }

        public void Update(GameTime gameTime, int i)
        {
            // Small random pixel movement
            if (counter <= 0)
            {
                Random rng = new Random(4 * i);
                //Vector2 direction = getRandomDirection();
                //position.X += direction.X;
                //position.Y += direction.Y;
                position.X += rng.Next(-4, 5);
                position.Y += rng.Next(-4, 5);
                counter = 30;
            }
            else counter--;
        }

        public Vector2 getRandomDirection(int i)
        {
            Random rng = new Random(i);
            direction.X = rng.Next(-1, 2);
            direction.Y = rng.Next(-1, 2);

            return direction;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, color);
        }
    }
}
