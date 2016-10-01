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
    class InsidePlayer
    {
        // Inside Player variables
        public int health, speed;
        public Vector2 position, previousPosition, origin;
        public Rectangle hitBox;
        public Texture2D playerTexture;
        public bool isVisible = true;

        public InsidePlayer()
        {
            // Screen Positions
            position = new Vector2(1728 / 2 - 12, 972 / 2 - 12);
            origin = new Vector2(1728 / 2, 972 / 2);

            // Hit Box 
            hitBox = new Rectangle();
            hitBox.Height = 25;
            hitBox.Width = 25;
            hitBox.X = (int)position.X;
            hitBox.Y = (int)position.Y;

            // Player Stats
            health = 100;
            speed = 10;
            previousPosition = position;
        }

        public void LoadContent(ContentManager Content)
        {
            // Texture Loading
            playerTexture = Content.Load<Texture2D>("Player_1");
        }

        public void UnloadContent()
        {
            // TODO: Unload upon death
        }

        public void Update(GameTime gameTime)
        {
            // Key State for this frame
            KeyboardState keyState = Keyboard.GetState();

            //player movement
            if (keyState.IsKeyDown(Keys.Up))
                position.Y -= speed;

            if (keyState.IsKeyDown(Keys.Left))
                position.X -= speed;

            if (keyState.IsKeyDown(Keys.Down))
                position.Y += speed;

            if (keyState.IsKeyDown(Keys.Right))
                position.X += speed;

            // Radius check, keeps player within stage
            if (DistanceFromCenter(position) >= 370)
            {
                position = previousPosition;
            }

            // Update hit box to new player position
            hitBox.X = (int)position.X;
            hitBox.Y = (int)position.Y;

            // Change previous position to our new position
            previousPosition = position;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // Draw player
            spriteBatch.Draw(playerTexture, position, Color.White);
        }

        public double DistanceFromCenter(Vector2 position)
        {
            // Calculates distance from position x to center screen, given position x.

            double distance = 0.0;

            position.X += playerTexture.Width / 2;
            position.Y += playerTexture.Height / 2;

            distance = Math.Sqrt(Math.Pow(position.X - origin.X, 2) + Math.Pow(position.Y - origin.Y, 2));

            return distance;
        }
    }
}
