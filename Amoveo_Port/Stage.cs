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
    class Stage
    {
        // Stage variables
        public Vector2 origin;
        public float radius;
        public int sides;
        public float thickness;
        public bool isVisible;
        public List<Vector2> pointList = new List<Vector2>();

        public Stage(int Sides)
        {
            // Load amount of sides from paramater
            sides = Sides;
        }

        public void Initialize()
        {
            // Set stage origin
            origin.X = 1728 / 2;
            origin.Y = 972 / 2;

            // Set stage stats
            radius = 450;
            thickness = 5;
            isVisible = false;
        }

        public void LoadContent(ContentManager Content)
        {
            // Potentially load texture here
            // Currently using no texture (transparent)
        }

        public void UnloadContent()
        {
            // Potentially unload texture content
        }

        public void Update(GameTime gameTime)
        {
            // Potentially add in rotating stages
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // Draw stage based on position and number of sides using Primitives 2D library
            pointList = Primitives2D.DrawCircle(spriteBatch, origin, radius, sides, Color.White, thickness);
        }
    }
}
