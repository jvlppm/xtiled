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
        
        Int32 mapIdx;
        List<Map> maps;
        Map currentMap { get { return maps[mapIdx]; } }

        Rectangle screen;
        Rectangle mapView;

        double cycleTimer = 0;

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
            maps = new List<Map>();

            maps.Add(Content.Load<Map>("desert"));
            maps.Add(Content.Load<Map>("desert_base64_uncompressed"));
            maps.Add(Content.Load<Map>("desert_base64_gzip"));
            maps.Add(Content.Load<Map>("desert_base64_zlib"));
            maps.Add(Content.Load<Map>("isometric_grass_and_water"));
            maps.Add(Content.Load<Map>("perspective_walls"));
            maps.Add(Content.Load<Map>("sewers"));

            mapIdx = 0;
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
                delta.Y += Convert.ToInt32(gameTime.ElapsedGameTime.TotalMilliseconds / 4);
            if (keys.IsKeyDown(Keys.Up) || pad.IsButtonDown(Buttons.DPadUp))
                delta.Y -= Convert.ToInt32(gameTime.ElapsedGameTime.TotalMilliseconds / 4);
            if (keys.IsKeyDown(Keys.Right) || pad.IsButtonDown(Buttons.DPadRight))
                delta.X += Convert.ToInt32(gameTime.ElapsedGameTime.TotalMilliseconds / 4);
            if (keys.IsKeyDown(Keys.Left) || pad.IsButtonDown(Buttons.DPadLeft))
                delta.X -= Convert.ToInt32(gameTime.ElapsedGameTime.TotalMilliseconds / 4);

            cycleTimer += gameTime.ElapsedGameTime.TotalMilliseconds;
            if ((keys.IsKeyDown(Keys.PageUp) || pad.IsButtonDown(Buttons.RightShoulder)) && cycleTimer >= 250) {
                mapIdx = mapIdx + 1 >= maps.Count ? 0 : mapIdx + 1;
                mapView.X = 0;
                mapView.Y = 0;
                cycleTimer = 0;
            }
            if ((keys.IsKeyDown(Keys.PageDown) || pad.IsButtonDown(Buttons.LeftShoulder)) && cycleTimer >= 250) {
                mapIdx = mapIdx - 1 < 0 ? maps.Count - 1 : mapIdx - 1;
                mapView.X = 0;
                mapView.Y = 0;
                cycleTimer = 0;
            }

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
