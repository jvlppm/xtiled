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

        public Game1() {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize() {
            base.Initialize();
        }

        protected override void LoadContent() {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            currentMap = Content.Load<Map>("desert");
            Map desert2 = Content.Load<Map>("desert_base64_uncompressed");
            Map desert3 = Content.Load<Map>("desert_base64_gzip");
            Map desert4 = Content.Load<Map>("desert_base64_zlib");
            Map isometric_grass_and_water = Content.Load<Map>("isometric_grass_and_water");
            Map perspective_walls = Content.Load<Map>("perspective_walls");
            Map sewers = Content.Load<Map>("sewers");

            mapTex = Content.Load<Texture2D>("tmw_desert_spacing");

        }

        protected override void UnloadContent() {
        }

        protected override void Update(GameTime gameTime) {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();
      
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            for (int i = 0; i < currentMap.Layers[1].Tiles.Length; i++) {
                if (currentMap.Layers[1].Tiles[i] != null) {
                    spriteBatch.Draw(mapTex, currentMap.Layers[1].Tiles[i].Target, currentMap.Tiles[currentMap.Layers[1].Tiles[i].SourceID].Source, Color.White,
                        currentMap.Layers[1].Tiles[i].Rotation, currentMap.Tiles[currentMap.Layers[1].Tiles[i].SourceID].Origin, currentMap.Layers[1].Tiles[i].Effects, 0);
                }
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
