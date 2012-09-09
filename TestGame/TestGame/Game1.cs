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

        Rectangle screen;
        Rectangle mapView;
        Int32 mapIdx;
        List<Map> maps;

        double actionTimer = 0;

        public Game1() {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize() {
            base.Initialize();
            screen = graphics.GraphicsDevice.Viewport.Bounds;
            mapView = graphics.GraphicsDevice.Viewport.Bounds;
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
            maps.Add(Content.Load<Map>("isometric_10_5"));
            maps.Add(Content.Load<Map>("isometric_5_10"));
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

            if (maps[mapIdx].Bounds.Contains(delta))
                mapView = delta;

            actionTimer += gameTime.ElapsedGameTime.TotalMilliseconds;
            if ((keys.IsKeyDown(Keys.PageUp) || pad.IsButtonDown(Buttons.RightShoulder)) && actionTimer >= 250) {
                mapIdx = mapIdx + 1 >= maps.Count ? 0 : mapIdx + 1;
                mapView.X = 0;
                mapView.Y = 0;
                actionTimer = 0;
            }
            if ((keys.IsKeyDown(Keys.PageDown) || pad.IsButtonDown(Buttons.LeftShoulder)) && actionTimer >= 250) {
                mapIdx = mapIdx - 1 < 0 ? maps.Count - 1 : mapIdx - 1;
                mapView.X = 0;
                mapView.Y = 0;
                actionTimer = 0;
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

            maps[mapIdx].Draw(spriteBatch, ref mapView, ref screen);
            spriteBatch.End();

            base.Draw(gameTime);
        }

    }
}
