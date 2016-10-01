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
    class Player
    {
        // Player Variables
        public List<Bullet> bulletList;
        public Vector2 position, origin;
        public Rectangle hitBox;
        public Texture2D playerTexture;
        public Texture2D smallBulletTexture, bigBulletTexture;
        public float rotationAngle, rotationAdjust;
        public int health, smallBulletSize, bigBulletSize;

        public Player(Vector2 Position, Texture2D Texture)
        {
            // Set position and texture from parameters
            position = Position;
            playerTexture = Texture;
        }

        public void Initialize()
        {
            // Init origin and hitbox rectangle
            origin = new Vector2(1728 / 2, 972 / 2);
            hitBox = new Rectangle();

            // Init player stats
            health = 100;
            smallBulletSize = 5;
            bigBulletSize = 20;
            rotationAngle = 0;

            //Init bullet textures
            smallBulletTexture = null;
            bigBulletTexture = null;
        }

        public void LoadContent(ContentManager Content)
        {
            // Load player texture
            playerTexture = Content.Load<Texture2D>("Player_2_Practice");
        }

        public void UnloadContent()
        {
            // TODO: unload texture content
        }

        public void Update(GameTime gameTime)
        {
            KeyboardState keyState = Keyboard.GetState();

            // Rotation from input
            float movement = 0f;

            if (keyState.IsKeyDown(Keys.NumPad1))
                movement += 0.1f;

            if (keyState.IsKeyDown(Keys.NumPad2))
                movement -= 0.1f;

            rotationAngle += movement;
            float circle = MathHelper.Pi * 2;
            rotationAngle = rotationAngle % circle;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // Draw Player
            spriteBatch.Draw(playerTexture, position, null, Color.White, rotationAngle, origin, 1.0f, SpriteEffects.None, 0f);
        }

    }
}
