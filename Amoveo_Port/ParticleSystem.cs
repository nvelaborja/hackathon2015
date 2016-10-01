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
    class ParticleSystem
    {
        public List<Pixel> Pixels = new List<Pixel>();
        public int counter = 30;
        public bool isTimeOut = false;
        Pixel pixel = new Pixel();


        public ParticleSystem(ContentManager Content, Vector2 Position)
        {
            for (int i = 0; i < 100; i++)
            {
                pixel.Initialize(Content, Position, i);
                Pixels.Add(pixel);
            }
        }

        public void Initialize()
        {

        }

        public void LoadContent(ContentManager Content)
        {


        }

        public void UnloadContent()
        {

        }

        public void Update(GameTime gameTime)
        {
            if (counter <= 0)
            {
                isTimeOut = true;
                return;
            }
            else
            {
                int i = 0;
                foreach (Pixel p in Pixels)
                {

                    p.Update(gameTime, i);
                    i++;
                }
                counter--;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Pixel p in Pixels)
            {
                p.Draw(spriteBatch);
            }
        }
    }
}
