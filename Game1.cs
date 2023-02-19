using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using System;
using System.Collections.Generic;

namespace pnt {
    public class Game1 : Game {
        private static int TEXTURE_CIRCLE_SIZE = 512;

        private GraphicsDeviceManager m_graphics;
        private SpriteBatch m_spritebatch;
        private List<BrushStroke> m_strokes;
        private Texture2D m_circle_texture;
        private BrushSetting m_brush_setting;

        private static (Keys, Color)[] color_key_pairs = new[] 
        {
            (Keys.D1, Color.Red),
            (Keys.D2, Color.Green),
            (Keys.D3, Color.Blue),
            (Keys.D4, Color.Black),
            (Keys.D5, Color.Pink),
            (Keys.D6, Color.White),
        };

        public Game1() {
            m_graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        private void GenerateCircleTexture() {
            m_circle_texture = new Texture2D(GraphicsDevice, TEXTURE_CIRCLE_SIZE, TEXTURE_CIRCLE_SIZE);

            Color[] pixels = new Color[TEXTURE_CIRCLE_SIZE*TEXTURE_CIRCLE_SIZE];
            float circle_radius = TEXTURE_CIRCLE_SIZE/2;

            for (int y = 0; y < TEXTURE_CIRCLE_SIZE; ++y) {
                for (int x = 0; x < TEXTURE_CIRCLE_SIZE; ++x) {
                    float delta_x = x - TEXTURE_CIRCLE_SIZE/2;
                    float delta_y = y - TEXTURE_CIRCLE_SIZE/2;

                    var distance_from_center = Math.Sqrt(delta_x*delta_x + delta_y*delta_y);
                    var color_value = Math.Clamp((distance_from_center / circle_radius), 0.4, 1);
                    pixels[y * TEXTURE_CIRCLE_SIZE + x].R = 255;
                    pixels[y * TEXTURE_CIRCLE_SIZE + x].G = 255;
                    pixels[y * TEXTURE_CIRCLE_SIZE + x].B = 255;
                    pixels[y * TEXTURE_CIRCLE_SIZE + x].A = (byte)(Math.Clamp((255 - color_value*255)*2, 0, 255)); 
                }
            }
            m_circle_texture.SetData(pixels);
        }

        protected override void Initialize() {
            base.Initialize();
            GenerateCircleTexture();
            Window.AllowUserResizing = true;
            m_brush_setting = new BrushSetting();
            m_brush_setting.color = Color.Black;
            m_brush_setting.size = 128;
            m_strokes = new List<BrushStroke>();
        }

        protected override void LoadContent() {
            m_spritebatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void Update(GameTime gameTime) {
            if (Keyboard.GetState().IsKeyDown(Keys.W)) {
                m_brush_setting.size += 5;
            } else if (Keyboard.GetState().IsKeyDown(Keys.S)) {
                m_brush_setting.size -= 5;
            }

            m_brush_setting.size = Math.Max(m_brush_setting.size, 0);

            foreach (var pair in color_key_pairs) {
                if (Keyboard.GetState().IsKeyDown(pair.Item1)) {
                    m_brush_setting.color = pair.Item2;
                }
            }

            var mouse = Mouse.GetState();

            if (mouse.LeftButton == ButtonState.Pressed) {
                m_strokes.Add(
                    new BrushStroke(
                        m_brush_setting, 
                        new Vector2(mouse.X, mouse.Y)
                    )
                );
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.White);

            var mouse = Mouse.GetState();
            m_spritebatch.Begin(blendState: BlendState.NonPremultiplied);
            {
                foreach (var stroke in m_strokes) {
                    m_spritebatch.Draw(
                        m_circle_texture,
                        new Rectangle((int)stroke.position.X - stroke.size/2,
                                      (int)stroke.position.Y - stroke.size/2,
                                      stroke.size, stroke.size),
                        stroke.color
                    );
                }

                m_spritebatch.Draw(
                    m_circle_texture, 
                    new Rectangle(mouse.X - m_brush_setting.size/2, mouse.Y - m_brush_setting.size/2, m_brush_setting.size, m_brush_setting.size), 
                    new Color(m_brush_setting.color.R, m_brush_setting.color.G, m_brush_setting.color.B, (byte)64)
                );
            }
            m_spritebatch.End();
        }
    }
}
