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
        public class SeedInfo
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

        const int progressBarPictureCount = 3;
        const int progressBarPictureWidth = 91;
        const int progressBarPictureHeight = 1007;

        private SpriteBatch spriteBatch;
        private SpriteFont font;
        private SpriteFont largeFont;
        private SeedInfo[] seeds = new SeedInfo[9];
        private Texture2D[] textures = new Texture2D[9];
        private Texture2D helix;
        private Texture2D pixel;
        private Texture2D frame;
        private Texture2D progressBar;
        private Texture2D evilProgressBar;
        private int _selected = -1;
        private bool picking;
        private System.Drawing.Point mousePosition;
        private int windowHeigth;
        private int windowWidth;
        private float[] tooltipCounter = new float[9];
        private float progressCount;
        private float alpha;

        private const int barLength = 9;

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
            largeFont = contentManager.Load<SpriteFont>("largefont.tkfnt");
            helix = contentManager.Load<Texture2D>("helix.png");
            pixel = contentManager.Load<Texture2D>("pixel.png");
            frame = contentManager.Load<Texture2D>("frame.png");
            progressBar = contentManager.Load<Texture2D>("Dummy.png");
            evilProgressBar = contentManager.Load<Texture2D>("parasiteprogress.png");

            for (int i = 0; i < barLength; i++)
            {
                seeds[i] = new SeedInfo();
                seeds[i]._position = new Vector2(i * (windowWidth) / 10 + 5, 5);   
            }

            seeds[0]._type = VoxelType.TEAK_WOOD;
            textures[0] = contentManager.Load<Texture2D>("teak.png");
            seeds[1]._type = VoxelType.PINE_WOOD;
            textures[1] = contentManager.Load<Texture2D>("Pine.png");
            seeds[2]._type = VoxelType.SPRUCE_WOOD;
            textures[2] = contentManager.Load<Texture2D>("spruce.png");
            seeds[3]._type = VoxelType.BEECH_WOOD;
            textures[3] = contentManager.Load<Texture2D>("Beech.png");
            seeds[4]._type = VoxelType.OAK_WOOD;
            textures[4] = contentManager.Load<Texture2D>("oak.png");
            seeds[5]._type = VoxelType.REDWOOD;
            textures[5] = contentManager.Load<Texture2D>("redwood.png");
            seeds[6]._type = VoxelType.TEAK_WOOD;
            textures[6] = contentManager.Load<Texture2D>("teak.png");
            seeds[7]._type = VoxelType.TEAK_WOOD;
            textures[7] = contentManager.Load<Texture2D>("teak.png");
            seeds[8]._type = VoxelType.TEAK_WOOD;
            textures[8] = contentManager.Load<Texture2D>("teak.png");
        }

        private bool MouseOver(Vector2 pos, double width, double heigth)
        {
            return (mousePosition.X >= pos.X && mousePosition.X <= pos.X + width && mousePosition.Y >= pos.Y && mousePosition.Y < pos.Y + heigth);
        }

        public void Update()
        {
            for (int i = 0; i < barLength; i++)
            {
                if (i == 0 || i == 1 || i == 2 || i == 3 || i == 4)
                {
                if (MouseOver(seeds[i]._position, 83, 32) && picking)
                {
                    _selected = i;
                }
                }
            }  
        }

        public void Draw(Level currentlevel, GameTime gameTime)
        {
            float progress = (float)currentlevel.CurrentBiomass / (float)currentlevel.TargetBiomass;
            float evilProgress = (float)currentlevel.CurrentParasiteMass / (float)currentlevel.FinalParasiteBiomass;


            progressCount = (float)gameTime.TotalGameTime.TotalSeconds / 2;

            alpha = progressCount - (float)Math.Floor(progressCount);

            spriteBatch.Begin(SpriteSortMode.Deferred, spriteBatch.GraphicsDevice.BlendStates.NonPremultiplied);
            
            //draw Progress good/evil
            spriteBatch.Draw(progressBar, new DrawingRectangle(windowWidth, windowHeigth, progressBarPictureWidth, (int)((windowHeigth - 68) * progress)),
                new DrawingRectangle(progressBarPictureWidth * ((int)progressCount % progressBarPictureCount), 0, progressBarPictureWidth, (int)(progressBarPictureHeight * progress)),
                new Color(1f,1f,1f, 1f - alpha), (float)Math.PI, new Vector2(0, 0), SpriteEffects.None, 0);
            spriteBatch.Draw(progressBar, new DrawingRectangle(windowWidth, windowHeigth, progressBarPictureWidth, (int)((windowHeigth - 68) * progress)),
                new DrawingRectangle(progressBarPictureWidth * (((int)progressCount + 1) % progressBarPictureCount), 0, progressBarPictureWidth, (int)(progressBarPictureHeight * progress)),
                new Color(1f, 1f, 1f, alpha), (float)Math.PI, new Vector2(0, 0), SpriteEffects.None, 0);


            spriteBatch.Draw(evilProgressBar, new DrawingRectangle(windowWidth - progressBarPictureWidth, windowHeigth, progressBarPictureWidth, (int)((windowHeigth - 68) * evilProgress)),
                new DrawingRectangle(progressBarPictureWidth * ((int)progressCount % progressBarPictureCount), 0, progressBarPictureWidth, (int)(progressBarPictureHeight * evilProgress)),
                new Color(1f,1f,1f, 1f - alpha), (float)Math.PI, new Vector2(0, 0), SpriteEffects.None, 0);
            spriteBatch.Draw(evilProgressBar, new DrawingRectangle(windowWidth - progressBarPictureWidth, windowHeigth, progressBarPictureWidth, (int)((windowHeigth - 68) * evilProgress)),
                new DrawingRectangle(progressBarPictureWidth * (((int)progressCount + 1) % progressBarPictureCount), 0, progressBarPictureWidth, (int)(progressBarPictureHeight * evilProgress)),
                new Color(1f, 1f, 1f, alpha), (float)Math.PI, new Vector2(0, 0), SpriteEffects.None, 0);

            spriteBatch.End();
            spriteBatch.Begin();

            for (int i = 0; i < barLength; i++)
            {
                if (i == 0 || i == 1 || i == 2 || i == 3 || i == 4)
                {
                //draw frame
                spriteBatch.Draw(pixel, new DrawingRectangle((int)seeds[i]._position.X, (int)seeds[i]._position.Y , 84, 32), Color.Black);
                spriteBatch.Draw(frame, new DrawingRectangle((int)seeds[i]._position.X - 5, (int)seeds[i]._position.Y - 5, 94, 42), Color.Gray);
                //draw price
                spriteBatch.Draw(helix, new DrawingRectangle((int)seeds[i]._position.X + 35, (int)seeds[i]._position.Y + 5, 10, 20), Color.White);
                spriteBatch.DrawString(font, TypeInformation.GetPrice(seeds[i]._type).ToString(), new Vector2(seeds[i]._position.X + 43, seeds[i]._position.Y+5), Color.White); 
                //draw Icons
                spriteBatch.Draw(textures[i], new DrawingRectangle((int)seeds[i]._position.X, (int)seeds[i]._position.Y, 32, 32), Color.White);  
                }
            }

            //draw Resources
            spriteBatch.Draw(pixel, new DrawingRectangle(9 * windowWidth / 10 + 7, 5, progressBarPictureWidth * 2-10, 54), Color.Black);
            spriteBatch.Draw(frame, new DrawingRectangle(9 * windowWidth / 10 + 2, 0, progressBarPictureWidth * 2, 64), Color.Gray);
            spriteBatch.Draw(helix, new DrawingRectangle(9 * windowWidth / 10 + 15, 9, 23, 46), Color.White);
            spriteBatch.DrawString(largeFont, currentlevel.Resources.ToString(), new Vector2(9 * windowWidth / 10 + 36, -6), Color.White);

            if (_selected > -1)
            {
                spriteBatch.Draw(frame, new DrawingRectangle((int)seeds[_selected]._position.X - 5, (int)seeds[_selected]._position.Y - 5, 94, 42), Color.Yellow);
            }

            //draw Tooltip
            for (int i = 0; i < barLength; i++)
            {
                if (i == 0 || i == 1 || i == 2 || i == 3 || i == 4)
                {
                if (MouseOver(seeds[i]._position, 83, 32))
                {
                    tooltipCounter[i] += gameTime.ElapsedGameTime.Milliseconds;

                    if (tooltipCounter[i] >= 300)
                    {
                        int corrector = 0;
                        if (mousePosition.X + 310 > windowWidth) corrector = mousePosition.X + 310 - windowWidth;

                        spriteBatch.Draw(pixel, new DrawingRectangle(mousePosition.X + 10 - corrector, mousePosition.Y + 10, 280, 130), new Color(0.5f, 0.5f, 0.5f, 0.5f));
                        spriteBatch.DrawString(font, TypeInformation.GetName(seeds[i]._type), new Vector2(mousePosition.X + 35 - corrector, mousePosition.Y + 15), Color.Black);
                        spriteBatch.DrawString(font, "+" + TypeInformation.GetStrength(seeds[i]._type)[0] + "\n+" + TypeInformation.GetStrength(seeds[i]._type)[1], new Vector2(mousePosition.X + 15 - corrector, mousePosition.Y + 40), Color.Green);
                        spriteBatch.DrawString(font, "-" + TypeInformation.GetWeakness(seeds[i]._type)[0] + "\n-" + TypeInformation.GetWeakness(seeds[i]._type)[1], new Vector2(mousePosition.X + 15 - corrector, mousePosition.Y + 90), Color.Crimson);
                    }
                }
                else tooltipCounter[i] = 0;
                }
            }
            spriteBatch.End();
        }

    }
}
