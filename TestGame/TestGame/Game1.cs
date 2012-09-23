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

        Rectangle mapView;
        Int32 mapIdx;
        List<Map> maps;

        Rectangle player;
        Color playerColor;

        double actionTimer = 0;

        public Game1() {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize() {
            base.Initialize();
            mapView = graphics.GraphicsDevice.Viewport.Bounds;
            mapView.X = 0;
            mapView.Y = 0;

            player = maps[mapIdx].SourceTiles[0].Source;
            player.X = mapView.Width / 2 - player.Width / 2;
            player.Y = mapView.Height / 2 - player.Height / 2;
            playerColor = Color.White;
        }

        protected override void LoadContent() {
            spriteBatch = new SpriteBatch(graphics.GraphicsDevice);
            Map.InitObjectDrawing(graphics.GraphicsDevice);

            maps = new List<Map>();

            maps.Add(Content.Load<Map>("desert"));
            maps.Add(Content.Load<Map>("desert_base64_uncompressed"));
            maps.Add(Content.Load<Map>("desert_base64_gzip"));
            maps.Add(Content.Load<Map>("desert_base64_zlib"));
            maps.Add(Content.Load<Map>("isometric_grass_and_water"));
            maps.Add(Content.Load<Map>("isometric_10_5"));
            maps.Add(Content.Load<Map>("isometric_5_10"));
            maps.Add(Content.Load<Map>("perspective_walls"));
            maps.Add(Content.Load<Map>("sewers"));
            maps.Add(Content.Load<Map>("rot_odd_test"));

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

            if (maps[mapIdx].Bounds.Contains(delta)) {
                player.X += delta.X - mapView.X;
                player.Y += delta.Y - mapView.Y;
                mapView = delta;
            }

            actionTimer += gameTime.ElapsedGameTime.TotalMilliseconds;
            if ((keys.IsKeyDown(Keys.PageUp) || pad.IsButtonDown(Buttons.RightShoulder)) && actionTimer >= 250) {
                mapIdx = mapIdx + 1 >= maps.Count ? 0 : mapIdx + 1;
                mapView.X = 0;
                mapView.Y = 0;
                actionTimer = 0;

                player = maps[mapIdx].SourceTiles[0].Source;
                player.X = mapView.Width / 2 - player.Width / 2;
                player.Y = mapView.Height / 2 - player.Height / 2;
            }
            if ((keys.IsKeyDown(Keys.PageDown) || pad.IsButtonDown(Buttons.LeftShoulder)) && actionTimer >= 250) {
                mapIdx = mapIdx - 1 < 0 ? maps.Count - 1 : mapIdx - 1;
                mapView.X = 0;
                mapView.Y = 0;
                actionTimer = 0;

                player = maps[mapIdx].SourceTiles[0].Source;
                player.X = mapView.Width / 2 - player.Width / 2;
                player.Y = mapView.Height / 2 - player.Height / 2;
            }

            playerColor = Color.White;
            for (int ol = 0; ol < maps[mapIdx].ObjectLayers.Count; ol++) {
                for (int o = 0; o < maps[mapIdx].ObjectLayers[ol].MapObjects.Length; o++) {
                    if (maps[mapIdx].ObjectLayers[ol].MapObjects[o].Polygon != null) {
                        if (maps[mapIdx].ObjectLayers[ol].MapObjects[o].Polygon.Contains(ref player)) {
                            playerColor = Color.Red;
                        }
                        else if (maps[mapIdx].ObjectLayers[ol].MapObjects[o].Polygon.Intersects(ref player)) {
                            playerColor = Color.Yellow;
                        }
                    }
                    if (maps[mapIdx].ObjectLayers[ol].MapObjects[o].Polyline != null) {
                        if (maps[mapIdx].ObjectLayers[ol].MapObjects[o].Polyline.Intersects(ref player)) {
                            playerColor = Color.Yellow;
                        }
                    }
                }

            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // sewers map needs blendstate to look correct with alphas
            if (mapIdx == 8)
                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied);
            else
                spriteBatch.Begin();

            maps[mapIdx].Draw(spriteBatch, mapView);

            // draw object layers test
            for (int ol = 0; ol < maps[mapIdx].ObjectLayers.Count; ol++)
                maps[mapIdx].DrawObjectLayer(spriteBatch, ol, mapView, 0);

            // draw player
            spriteBatch.Draw(maps[mapIdx].Tilesets[maps[mapIdx].SourceTiles[0].TilesetID].Texture,
                             Map.Translate(player, mapView),
                             maps[mapIdx].SourceTiles[0].Source,
                             playerColor);

            spriteBatch.End();

            base.Draw(gameTime);
        }

    }
}
