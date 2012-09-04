using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using FuncWorks.XNA.XTiled;

namespace TestGame {
    public class Game1 : Microsoft.Xna.Framework.Game {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Map currentMap;
        Texture2D mapTex;

        Rectangle screen;
        Rectangle mapView;

        public Game1() {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize() {
            base.Initialize();
            screen = graphics.GraphicsDevice.Viewport.Bounds;
            screen.X = 50;
            screen.Y = 50;
            screen.Height -= 100;
            screen.Width -= 100;

            mapView = screen;
            mapView.X = 0;
            mapView.Y = 0;
        }

        protected override void LoadContent() {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Map desert = Content.Load<Map>("desert");
            Map desert2 = Content.Load<Map>("desert_base64_uncompressed");
            Map desert3 = Content.Load<Map>("desert_base64_gzip");
            Map desert4 = Content.Load<Map>("desert_base64_zlib");
            Map isometric_grass_and_water = Content.Load<Map>("isometric_grass_and_water");
            Map perspective_walls = Content.Load<Map>("perspective_walls");
            Map sewers = Content.Load<Map>("sewers");

            currentMap = desert;
        }

        protected override void UnloadContent() {
        }

        protected override void Update(GameTime gameTime) {
            KeyboardState keys = Keyboard.GetState();
            GamePadState pad = GamePad.GetState(PlayerIndex.One);

            if (keys.IsKeyDown(Keys.Escape) || pad.IsButtonDown(Buttons.Back))
                this.Exit();

            Rectangle delta = mapView;
            if (keys.IsKeyDown(Keys.Down) || pad.IsButtonDown(Buttons.DPadDown))
                delta.Y += Convert.ToInt32(gameTime.ElapsedGameTime.TotalMilliseconds / 2);
            if (keys.IsKeyDown(Keys.Up) || pad.IsButtonDown(Buttons.DPadUp))
                delta.Y -= Convert.ToInt32(gameTime.ElapsedGameTime.TotalMilliseconds / 2);
            if (keys.IsKeyDown(Keys.Right) || pad.IsButtonDown(Buttons.DPadRight))
                delta.X += Convert.ToInt32(gameTime.ElapsedGameTime.TotalMilliseconds / 2);
            if (keys.IsKeyDown(Keys.Left) || pad.IsButtonDown(Buttons.DPadLeft))
                delta.X -= Convert.ToInt32(gameTime.ElapsedGameTime.TotalMilliseconds / 2);

            if (currentMap.Bounds.Contains(delta))
                mapView = delta;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied);

            for (int l = 0; l < currentMap.Layers.Length; l++) {
                if (currentMap.Layers[l].Visible)
                    for (int i = 0; i < currentMap.Layers[l].Tiles.Length; i++) {
                        if (currentMap.Layers[l].Tiles[i] != null) {
                            Rectangle target = currentMap.Layers[l].Tiles[i].Target;
                            target.X = target.X - mapView.X + screen.X - Convert.ToInt32(currentMap.Tiles[currentMap.Layers[l].Tiles[i].SourceID].Origin.X);
                            target.Y = target.Y - mapView.Y + screen.Y - Convert.ToInt32(currentMap.Tiles[currentMap.Layers[l].Tiles[i].SourceID].Origin.Y);

                            if (screen.Contains(target)) {
                                target.X += Convert.ToInt32(currentMap.Tiles[currentMap.Layers[l].Tiles[i].SourceID].Origin.X);
                                target.Y += Convert.ToInt32(currentMap.Tiles[currentMap.Layers[l].Tiles[i].SourceID].Origin.Y);

                                spriteBatch.Draw(
                                    currentMap.Tilesets[currentMap.Tiles[currentMap.Layers[l].Tiles[i].SourceID].TilesetID].Texture,
                                    target,
                                    currentMap.Tiles[currentMap.Layers[l].Tiles[i].SourceID].Source,
                                    currentMap.Layers[l].OpacityColor,
                                    currentMap.Layers[l].Tiles[i].Rotation,
                                    currentMap.Tiles[currentMap.Layers[l].Tiles[i].SourceID].Origin,
                                    currentMap.Layers[l].Tiles[i].Effects,
                                    0);
                            }
                            else if (screen.Intersects(target)) {
                                Rectangle delta = Rectangle.Intersect(screen, target);
                                Rectangle source = currentMap.Tiles[currentMap.Layers[l].Tiles[i].SourceID].Source;

                                source.X -= target.X - delta.X;
                                source.Y -= target.Y - delta.Y;
                                source.Height = delta.Height;
                                source.Width = delta.Width;

                                target.X -= target.X - delta.X;
                                target.Y -= target.Y - delta.Y;
                                target.Height = delta.Height;
                                target.Width = delta.Width;

                                target.X += Convert.ToInt32(currentMap.Tiles[currentMap.Layers[l].Tiles[i].SourceID].Origin.X);
                                target.Y += Convert.ToInt32(currentMap.Tiles[currentMap.Layers[l].Tiles[i].SourceID].Origin.Y);

                                spriteBatch.Draw(
                                    currentMap.Tilesets[currentMap.Tiles[currentMap.Layers[l].Tiles[i].SourceID].TilesetID].Texture,
                                    target,
                                    source,
                                    currentMap.Layers[l].OpacityColor,
                                    currentMap.Layers[l].Tiles[i].Rotation,
                                    currentMap.Tiles[currentMap.Layers[l].Tiles[i].SourceID].Origin,
                                    currentMap.Layers[l].Tiles[i].Effects,
                                    0);
                            }
                        }
                    }
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
