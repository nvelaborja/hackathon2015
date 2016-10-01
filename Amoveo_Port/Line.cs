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
    class Line
    {

        public Vector2 point1, point2;
        public float slope;
        public float b;
        public Rectangle rect = new Rectangle();
        public int reflectionBuffer = 20;

        public Line(Vector2 Point1, Vector2 Point2)
        {
            point1 = Point1;
            point2 = Point2;
            slope = ((point2.Y - point1.Y) / (point2.X - point1.X));
            b = (point1.Y / (slope * point1.X));
        }

        public bool isCollide(Vector2 position)
        {
            bool isColliding = false;

            // TODO: add in a possible buffer range for b

            if ((position.Y / (slope * position.X)) < b + 0.5 && (position.Y / (slope * position.X)) > b - 0.5 && reflectionBuffer <= 0)
            {
                isColliding = true;
                reflectionBuffer = 0;
            }
            else reflectionBuffer--;

            return isColliding;
        }

    }
}
