﻿using System;
using System.Windows.Input;

using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Diagnostics;
using SharpDX.Toolkit.Graphics;
using SharpDX.Toolkit.Content;

namespace VoxelSeeds
{
    class Seedbar
    {
        class SeedInfo
        {
            public VoxelType _type;
            public Vector2 _position;
            /*
            public SeedInfo(VoxelType type, Texture2D texture, Vector2 position)
            {
                _type = type;
                _texture = texture;
                _position = position;
            }
            */
        }
        private SpriteBatch spriteBatch;
        private SpriteFont font;
        private SeedInfo[] seeds = new SeedInfo[10];
        private Texture2D[] textures = new Texture2D[10];
        private Texture2D helix;
        private Texture2D pixel;
        private Texture2D frame;
        private int _selected = -1;
        private bool picking;
        private System.Drawing.Point mousePosition;
        private int windowHeigth;
        private int windowWidth;
        private float[] tooltipCounter = new float[10];

        private const int barLength = 10;

        public SeedInfo GetSeedInfo()
        {
            return seeds[_selected];
        }

        public SeedInfo GetSeedInfo(int selected)
        {
            return seeds[selected];
        }

        public int GetSelected()
        {
            return _selected;
        }

        public Seedbar(System.Windows.Forms.Control inputControlElement)
        {       

            // input handling...
            inputControlElement.MouseDown += (object sender, System.Windows.Forms.MouseEventArgs e) =>
                picking = e.Button == System.Windows.Forms.MouseButtons.Left;
            inputControlElement.MouseUp += (object sender, System.Windows.Forms.MouseEventArgs e) =>
            {
                if (picking && e.Button == System.Windows.Forms.MouseButtons.Left)
                    picking = false;
            };
            inputControlElement.MouseMove += (object sender, System.Windows.Forms.MouseEventArgs e) =>
                mousePosition = e.Location;
        }

        public void LoadContent(GraphicsDevice graphicsDevice, ContentManager contentManager)
        {
            spriteBatch = new SpriteBatch(graphicsDevice);
            windowHeigth = spriteBatch.GraphicsDevice.BackBuffer.Height;
            windowWidth = spriteBatch.GraphicsDevice.BackBuffer.Width;
            font = contentManager.Load<SpriteFont>("Arial16.tkfnt");
            helix = contentManager.Load<Texture2D>("balls.dds");
            pixel = contentManager.Load<Texture2D>("pixel.png");
            frame = contentManager.Load<Texture2D>("frame.png");

            for (int i = 0; i < barLength; i++)
            {
                seeds[i] = new SeedInfo();
                seeds[i]._position = new Vector2(i * (windowWidth - 70) / 10 + 5, 5);
                seeds[i]._type = VoxelType.TEAK_WOOD;
                textures[i] = contentManager.Load<Texture2D>("balls.dds");
            }
        }

        private bool MouseOver(Vector2 pos, double width, double heigth)
        {
            return (mousePosition.X >= pos.X && mousePosition.X <= pos.X + width && mousePosition.Y >= pos.Y && mousePosition.Y < pos.Y + heigth);
        }

        public void Update()
        {
            for (int i = 0; i < barLength; i++)
            {
                if (MouseOver(seeds[i]._position, 83, 32) && picking)
                {
                    _selected = i;
                }
            }  
        }

        public void Draw(Level currentlevel, GameTime gameTime)
        {
            int progress = windowHeigth * currentlevel.CurrentBiomass/currentlevel.TargetBiomass;
            int evilProgress = windowHeigth * currentlevel.ParasiteBiomass/currentlevel.FinalParasiteBiomass;

            spriteBatch.Begin();
            
            //draw Progress good/evil
            spriteBatch.Draw(pixel, new DrawingRectangle(windowWidth, windowHeigth, 30, progress), null, Color.Blue, (float)Math.PI, new Vector2(0, 0), SpriteEffects.None, 0);
            spriteBatch.Draw(pixel, new DrawingRectangle(windowWidth-30, windowHeigth, 30, evilProgress), null, Color.Red, (float)Math.PI, new Vector2(0, 0), SpriteEffects.None, 0);

            for (int i = 0; i < barLength; i++)
            {
                spriteBatch.Draw(pixel, new DrawingRectangle((int)seeds[i]._position.X, (int)seeds[i]._position.Y, 82, 32), Color.Black);
                //draw frame
                spriteBatch.Draw(frame, new DrawingRectangle((int)seeds[i]._position.X - 5, (int)seeds[i]._position.Y - 5, 92, 42), Color.White);
                //draw curency
                spriteBatch.Draw(helix, new DrawingRectangle((int)seeds[i]._position.X + 37, (int)seeds[i]._position.Y+5, 10, 20), Color.White);
                spriteBatch.DrawString(font, TypeInformation.GetPrice(seeds[i]._type).ToString(), new Vector2(seeds[i]._position.X + 45, seeds[i]._position.Y+5), Color.White); 
                //draw Icons
                spriteBatch.Draw(textures[(int)seeds[i]._type], seeds[i]._position, new DrawingRectangle(0, 0, 32, 32), Color.White);   
            }

            if (_selected > -1)
            {
                spriteBatch.Draw(frame, new DrawingRectangle((int)seeds[_selected]._position.X - 5, (int)seeds[_selected]._position.Y - 5, 92, 42), Color.Yellow);
            }

            //draw Tooltip
            for (int i = 0; i < barLength; i++)
            {
                if (MouseOver(seeds[i]._position, 83, 32))
                {
                    tooltipCounter[i] += gameTime.ElapsedGameTime.Milliseconds;

                    if (tooltipCounter[i] >= 300)
                    {
                        int corrector = 0;
                        if (mousePosition.X + 160 > windowWidth) corrector = mousePosition.X + 160 - windowWidth;

                        spriteBatch.Draw(pixel, new DrawingRectangle(mousePosition.X + 10 - corrector, mousePosition.Y + 10, 150, 183), new Color(0.7f, 0.7f, 0.7f, 0.5f));
                        spriteBatch.DrawString(font, TypeInformation.GetName(seeds[i]._type), new Vector2(mousePosition.X + 50 - corrector, mousePosition.Y + 15), Color.Black);
                        spriteBatch.DrawString(font, "Strength:", new Vector2(mousePosition.X + 35 - corrector, mousePosition.Y + 40), Color.DarkBlue);
                        spriteBatch.DrawString(font, TypeInformation.GetStrength(seeds[i]._type)[0] + "\n" + TypeInformation.GetStrength(seeds[i]._type)[1], new Vector2(mousePosition.X + 35 - corrector, mousePosition.Y + 65), Color.DarkBlue);
                        spriteBatch.DrawString(font, "Weakness:", new Vector2(mousePosition.X + 35 - corrector, mousePosition.Y + 115), Color.Crimson);
                        spriteBatch.DrawString(font, TypeInformation.GetWeakness(seeds[i]._type)[0] + "\n" + TypeInformation.GetWeakness(seeds[i]._type)[1], new Vector2(mousePosition.X + 35 - corrector, mousePosition.Y + 140), Color.Crimson);
                    }
                }
                else tooltipCounter[i] = 0;
            }
            spriteBatch.End();
        }

    }
}
