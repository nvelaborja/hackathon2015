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
    class OutsidePlayer
    {
        public int speed, bulletSpeed, bulletDelay, bulletBuffer, i;
        public Vector2 position, previousPosition, origin, stageOrigin, playerOrigin;
        public Texture2D playerTexture;
        public List<Bullet> bulletList;
        public List<ParticleSystem> bursts = new List<ParticleSystem>();
        public ParticleSystem burst;
        public string lastShift = "circle";
        public float tempPosX;
        double angleX = 0.0, angleY = 0.0;

        public Stage hexStage = new Stage(6);
        public Stage heptStage = new Stage(7);
        public Stage octStage = new Stage(8);
        public Stage circleStage = new Stage(50);
        public List<Vector2> circlePoints = new List<Vector2>();
        public List<Vector2> hexPoints = new List<Vector2>();
        public List<Vector2> heptPoints = new List<Vector2>();
        public List<Vector2> octPoints = new List<Vector2>();
        public List<Line> circleLines = new List<Line>();
        public List<Line> hexLines = new List<Line>();
        public List<Line> heptLines = new List<Line>();
        public List<Line> octLines = new List<Line>();



        SoundManager SM;

        public OutsidePlayer()
        {
            position = new Vector2(1728 / 2 + 465, (972 / 2) - 12);
            origin = new Vector2(1728 / 2, (972 / 2));
            stageOrigin = new Vector2(1728 / 2, 972 / 2);
            playerOrigin = new Vector2();
            previousPosition = position;
            speed = 6;
            bulletSpeed = 6;
            bulletDelay = 40;
            bulletList = new List<Bullet>();
            circleStage.isVisible = true;
            bulletBuffer = 40;
            SM = new SoundManager();
            i = 0;
            tempPosX = 0;
        }

        public void LoadContent(ContentManager Content)
        {
            playerTexture = Content.Load<Texture2D>("Player_1");
            SM.LoadContent(Content);
        }

        public void UnloadContent()
        {

        }

        public void Update(GameTime gameTime, ContentManager Content, GraphicsDevice graphicsDevice, SpriteBatch spriteBatch)
        {
            KeyboardState keyState = Keyboard.GetState();

            //player movement
            if (keyState.IsKeyDown(Keys.A))
            {
                position.X += incrementAngleX(i, 1);
                position.Y += incrementAngleY(i, 1);
            }
            if (keyState.IsKeyDown(Keys.D))
            {
                position.X += incrementAngleX(i, 0);
                position.Y += incrementAngleY(i, 0);
            }
            //end player movement
            if (tempPosX != position.X)
            {
                tempPosX = position.X;
                i++;
            }


            // Shape shifting

            #region ShapeShifting

            if (keyState.IsKeyDown(Keys.I))
            {
                circleStage.isVisible = true;
                hexStage.isVisible = false;
                heptStage.isVisible = false;
                octStage.isVisible = false;
            }

            if (keyState.IsKeyDown(Keys.J))
            {
                circleStage.isVisible = false;
                hexStage.isVisible = true;
                heptStage.isVisible = false;
                octStage.isVisible = false;
            }


            if (keyState.IsKeyDown(Keys.K))
            {
                circleStage.isVisible = false;
                hexStage.isVisible = false;
                heptStage.isVisible = true;
                octStage.isVisible = false;
            }


            if (keyState.IsKeyDown(Keys.L))
            {
                circleStage.isVisible = false;
                hexStage.isVisible = false;
                heptStage.isVisible = false;
                octStage.isVisible = true;
            }

            #endregion



            //shooting
            if (keyState.IsKeyDown(Keys.S))
                ShootBig(Content);
            else if (keyState.IsKeyDown(Keys.W))
                ShootSmall(Content);

            UpdateBullets(Content, gameTime, spriteBatch);
            UpdateBurts(gameTime);

            // Reflections




            playerOrigin.X = position.X + playerTexture.Width / 2;
            playerOrigin.Y = position.Y + playerTexture.Height / 2;
            previousPosition = position;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // spriteBatch.Draw(playerTexture, position, null, Color.White, rotationAngle, origin, 1.0f, SpriteEffects.None, 0f);
            #region Bullet Draw
            foreach (Bullet b in bulletList)
            {
                b.Draw(spriteBatch);
            }
            spriteBatch.Draw(playerTexture, position, Color.White);
            #endregion


            #region Stage Draw

            circleLines = new List<Line>();
            hexLines = new List<Line>();
            heptLines = new List<Line>();
            octLines = new List<Line>();

            if (circleStage.isVisible)
            {
                if (lastShift != "circle")
                    SM.playShapeShift();
                Primitives2D.DrawCircle(spriteBatch, stageOrigin, 450, 50, Color.White, 16);
                circlePoints = Primitives2D.DrawCircle(spriteBatch, stageOrigin, 450, 50, Color.Black, 8);

                for (int i = 0; i < 50; i++)
                {
                    //Primitives2D.DrawLine(spriteBatch, shiftToCenter(circlePoints[i]), shiftToCenter(circlePoints[i + 1]), Color.Black);
                    Line newLine = new Line(shiftToCenter(circlePoints[i]), shiftToCenter(circlePoints[i + 1]));
                    circleLines.Add(newLine);
                }
                foreach (Line line in circleLines)
                {
                    Primitives2D.DrawLine(spriteBatch, (line.point1), 900f, (float)Math.Atan(line.slope), Color.Black, 1f);
                }
                lastShift = "circle";
            }
            else if (hexStage.isVisible)
            {
                if (lastShift != "hex")
                    SM.playShapeShift();
                Primitives2D.DrawCircle(spriteBatch, stageOrigin, 450, 6, Color.White, 16);
                hexPoints = Primitives2D.DrawCircle(spriteBatch, stageOrigin, 450, 6, Color.Black, 8);
                Primitives2D.DrawLine(spriteBatch, origin, playerOrigin, Color.Black);
                for (int i = 0; i < 6; i++)
                {
                    Primitives2D.DrawLine(spriteBatch, shiftToCenter(hexPoints[i]), shiftToCenter(hexPoints[i + 1]), Color.Black);
                    Line newLine = new Line(shiftToCenter(hexPoints[i]), shiftToCenter(hexPoints[i + 1]));
                    hexLines.Add(newLine);
                }
                foreach (Line line in hexLines)
                {
                    Primitives2D.DrawLine(spriteBatch, (line.point1), 900f, (float)Math.Atan(line.slope), Color.Black, 1f);
                }
                lastShift = "hex";
            }
            else if (heptStage.isVisible)
            {
                if (lastShift != "hept")
                    SM.playShapeShift();
                Primitives2D.DrawCircle(spriteBatch, stageOrigin, 450, 7, Color.White, 16);
                heptPoints = Primitives2D.DrawCircle(spriteBatch, stageOrigin, 450, 7, Color.Black, 8);
                Primitives2D.DrawLine(spriteBatch, origin, playerOrigin, Color.Black);
                for (int i = 0; i < 7; i++)
                {
                    Primitives2D.DrawLine(spriteBatch, shiftToCenter(heptPoints[i]), shiftToCenter(heptPoints[i + 1]), Color.Black);
                    Line newLine = new Line(shiftToCenter(heptPoints[i]), shiftToCenter(heptPoints[i + 1]));
                    heptLines.Add(newLine);
                }
                foreach (Line line in heptLines)
                {
                    Primitives2D.DrawLine(spriteBatch, (line.point1), 900f, (float)Math.Atan(line.slope), Color.Black, 1f);
                }
                lastShift = "hept";
            }
            else if (octStage.isVisible)
            {
                if (lastShift != "oct")
                    SM.playShapeShift();
                Primitives2D.DrawCircle(spriteBatch, stageOrigin, 450, 8, Color.White, 16);
                octPoints = Primitives2D.DrawCircle(spriteBatch, stageOrigin, 450, 8, Color.Black, 8);
                Primitives2D.DrawLine(spriteBatch, origin, playerOrigin, Color.Black);
                for (int i = 0; i < 8; i++)
                {
                    Primitives2D.DrawLine(spriteBatch, shiftToCenter(octPoints[i]), shiftToCenter(octPoints[i + 1]), Color.Black);
                    Line newLine = new Line(shiftToCenter(octPoints[i]), shiftToCenter(octPoints[i + 1]));
                    octLines.Add(newLine);
                }
                foreach (Line line in octLines)
                {
                    Primitives2D.DrawLine(spriteBatch, (line.point1), 900f, (float)Math.Atan(line.slope), Color.Black, 1f);
                }
                lastShift = "oct";
            }
            #endregion

            #region Burst Draw

            foreach (ParticleSystem p in bursts)
            {
                if (!p.isTimeOut)
                {
                    p.Draw(spriteBatch);
                }
            }

            #endregion
        }

        #region Shots

        public void ShootBig(ContentManager Content)
        {
            if (bulletBuffer > 0)
                bulletBuffer--;
            else
            {
                Vector2 newPosition;
                Bullet newBullet;
                if (hexStage.isVisible)
                {
                    newPosition = new Vector2((float)((position.X + playerTexture.Width / 2 - 7) - 105 * Math.Cos(angleX)), (float)((position.Y + playerTexture.Height / 2 - 7) - 105 * Math.Sin(angleY)));
                    newBullet = new Bullet(newPosition, 1, Content, angleX, angleY, 10);  // 1 for big bullet, 0 for small bullet
                }
                else if (heptStage.isVisible)
                {
                    newPosition = new Vector2((float)((position.X + playerTexture.Width / 2 - 7) - 85 * Math.Cos(angleX)), (float)((position.Y + playerTexture.Height / 2 - 7) - 85 * Math.Sin(angleY)));
                    newBullet = new Bullet(newPosition, 1, Content, angleX, angleY, 6);
                }
                else if (octStage.isVisible)
                {
                    newPosition = new Vector2((float)((position.X + playerTexture.Width / 2 - 7) - 70 * Math.Cos(angleX)), (float)((position.Y + playerTexture.Height / 2 - 7) - 70 * Math.Sin(angleY)));
                    newBullet = new Bullet(newPosition, 1, Content, angleX, angleY, 1);
                }
                else // circle stage
                {
                    newPosition = new Vector2((float)((position.X + playerTexture.Width / 2 - 7) - 45 * Math.Cos(angleX)), (float)((position.Y + playerTexture.Height / 2 - 7) - 45 * Math.Sin(angleY)));
                    newBullet = new Bullet(newPosition, 1, Content, angleX, angleY, 5);
                }

                bulletList.Add(newBullet);
                bulletBuffer = 40;
                SM.playBigShot();
            }

        }

        public void ShootSmall(ContentManager Content)
        {
            if (bulletBuffer > 0)
                bulletBuffer--;
            else
            {
                Vector2 newPosition;
                Bullet newBullet;
                if (hexStage.isVisible)
                {
                    newPosition = new Vector2((float)((position.X + playerTexture.Width / 2 - 3) - 105 * Math.Cos(angleX)), (float)((position.Y + playerTexture.Height / 2 - 3) - 105 * Math.Sin(angleY)));
                    newBullet = new Bullet(newPosition, 0, Content, angleX, angleY, 1);
                }
                else if (heptStage.isVisible)
                {
                    newPosition = new Vector2((float)((position.X + playerTexture.Width / 2 - 3) - 85 * Math.Cos(angleX)), (float)((position.Y + playerTexture.Height / 2 - 3) - 85 * Math.Sin(angleY)));
                    newBullet = new Bullet(newPosition, 0, Content, angleX, angleY, 6);
                }
                else if (octStage.isVisible)
                {
                    newPosition = new Vector2((float)((position.X + playerTexture.Width / 2 - 3) - 70 * Math.Cos(angleX)), (float)((position.Y + playerTexture.Height / 2 - 3) - 70 * Math.Sin(angleY)));
                    newBullet = new Bullet(newPosition, 0, Content, angleX, angleY, 15);
                }
                else // circle stage
                {
                    newPosition = new Vector2((float)((position.X + playerTexture.Width / 2 - 3) - 45 * Math.Cos(angleX)), (float)((position.Y + playerTexture.Height / 2 - 3) - 45 * Math.Sin(angleY)));
                    newBullet = new Bullet(newPosition, 0, Content, angleX, angleY, 10);
                }


                bulletList.Add(newBullet);
                bulletBuffer = 40;
                SM.playSmallShot();
            }
        }
        #endregion

        public Vector2 shiftToCenter(Vector2 point)
        {
            point.X += 864;
            point.Y += 486;
            return point;
        }

        public void UpdateBurts(GameTime gameTime)
        {
            foreach (ParticleSystem p in bursts)
            {
                if (!p.isTimeOut)
                {
                    p.Update(gameTime);
                }
            }
        }


        public void UpdateBullets(ContentManager Content, GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (Bullet b in bulletList)
            {
                b.Update(gameTime);


                b.position.X += -(float)Math.Cos(b.angleX) * b.speed;
                b.position.Y += -(float)Math.Sin(b.angleY) * b.speed;
                b.hitBox.X = (int)b.position.X;
                b.hitBox.Y = (int)b.position.Y;

                b.origin.X = b.position.X + b.bulletTexture.Width / 2;
                b.origin.Y = b.position.Y + b.bulletTexture.Height / 2;


                //if (DistanceFromCenter(b.position) > 300 && !b.invulnerableBullet)
                //if(!b.invulnerableBullet)
                if (1 == 1)
                {
                    if (circleStage.isVisible)
                    {
                        foreach (Line line in circleLines)
                        {
                            if (line.isCollide(b.position) == true)
                            {
                                b.reflections++;
                                b.angleX += Math.PI / 2;
                                b.angleY += Math.PI / 2;
                            }

                        }
                    }

                    if (hexStage.isVisible)
                    {
                        foreach (Line line in hexLines)
                        {
                            if (line.isCollide(b.position) == true)
                            {
                                b.reflections++;
                                b.angleX += Math.PI / 2;
                                b.angleY += Math.PI / 2;
                            }
                        }
                    }
                    if (heptStage.isVisible)
                    {
                        foreach (Line line in heptLines)
                        {
                            if (line.isCollide(b.position) == true)
                            {
                                b.reflections++;
                                b.angleX += Math.PI / 2;
                                b.angleY += Math.PI / 2;
                            }
                        }
                    }
                    if (octStage.isVisible)
                    {
                        foreach (Line line in octLines)
                        {
                            if (line.isCollide(b.position) == true)
                            {
                                b.reflections++;
                                b.angleX += Math.PI / 2;
                                b.angleY += Math.PI / 2;
                            }
                        }
                    }
                }

            }

            for (int i = 0; i < bulletList.Count; i++)
            {
                if (!bulletList[i].visible)
                {
                    burst = new ParticleSystem(Content, bulletList[i].position);
                    bursts.Add(burst);
                    SM.playBulletBurst();

                    bulletList.RemoveAt(i);
                    i--;
                }
            }
        }


        public double DistanceFromCenter(Vector2 position)
        {
            double distance = 0.0;

            position.X += playerTexture.Width / 2;
            position.Y += playerTexture.Height / 2;

            distance = Math.Sqrt(Math.Pow(position.X - stageOrigin.X, 2) + Math.Pow(position.Y - stageOrigin.Y, 2));

            return distance;
        }

        #region Increments

        public float incrementAngleX(int i, int DA)
        {
            double xval = 0;
            if (DA == 0)
            {
                angleX = (angleX + (0.065));// % (MathHelper.Pi * 2);
                xval = (float)475 * Math.Cos(angleX) + 852;
            }
            else
            {
                angleX = (angleX - (0.065));// % (MathHelper.Pi * 2);
                xval = (float)475 * Math.Cos(angleX) + 852;
            }
            return ((float)xval - position.X);
        }
        public float incrementAngleY(int i, int DA)
        {
            double yval = 0;
            if (DA == 0)
            {
                angleY = (angleY + (0.065));// % (MathHelper.Pi * 2);
                yval = (float)475 * Math.Sin(angleY) + 474;
            }
            else
            {
                angleY = (angleY - (0.065));// % (MathHelper.Pi * 2);
                yval = (float)475 * Math.Sin(angleY) + 474;
            }
            return ((float)yval - position.Y);
        }

        #endregion
    }
}
