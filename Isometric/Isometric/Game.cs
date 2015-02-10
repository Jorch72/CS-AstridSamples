﻿using Astrid.Core;
using Astrid.Framework;
using Astrid.Framework.Assets;
using Astrid.Framework.Audio;
using Astrid.Framework.Graphics;
using Astrid.Framework.Input;
using System;
using System.Linq;
using System.Collections.Generic;
namespace Isometric
{
    public class Game : GameBase, ITouchInputListener
    {
        public struct AtlasSprite
        {
            public AtlasRegion Visual;
            public Vector2 Position;
            public Vector2 GridPosition;
            public AtlasSprite(AtlasRegion visual, Vector2 position, Vector2 grid)
            {
                Visual = visual;
                Position = new Vector2(position.X + visual.OffsetX, position.Y + visual.OffsetY);
                GridPosition = grid;
            }
        }
        private Vector2 _position;
        private TextureRegion _texture;
        private SpriteBatch _spriteBatch;
        private TextureAtlasGDX _atlas;
        private Dictionary<string, AtlasRegion> _textures;
        private AtlasSprite[,] _terrain;
        private List<AtlasSprite> _orderedTerrain;
        private string[] _terrainNames = { "grass", "forest", "sand", "mud", "stone", "asphalt" };
        private Random _random;
        public Game(ApplicationBase application)
            : base(application)
        {
        }

        public override void Create()
        {
            _random = new Random();
            InputDevice.Processors.Add(new TouchInputProcessor(this));

            _soundEffect = AssetManager.Load<SoundEffect>(@"song.mp3");

            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _atlas = AssetManager.Load<TextureAtlasGDX>("pack.atlas");
            _textures = new Dictionary<string, AtlasRegion>
            {
                {"grass", _atlas["palette50_Terrain_Huge_face0"]},
                {"forest", _atlas["palette51_Terrain_Huge_face0"]},
                {"sand", _atlas["palette52_Terrain_Huge_face0"]},
                {"jungle", _atlas["palette53_Terrain_Huge_face0"]},
                {"mud", _atlas["palette54_Terrain_Huge_face0"]},
                {"stone", _atlas["palette55_Terrain_Huge_face0"]},
                {"poison", _atlas["palette56_Terrain_Huge_face0"]},
                {"ice", _atlas["palette57_Terrain_Huge_face0"]},
                {"asphalt", _atlas["palette58_Terrain_Huge_face0"]},
                {"river", _atlas["palette59_Terrain_Huge_face0"]},
                {"ocean", _atlas["palette60_Terrain_Huge_face0"]},
                {"manSE", _atlas["scheme1/palette3_Generic_Male_Large_face0"]},
                {"manSW", _atlas["scheme1/palette3_Generic_Male_Large_face1"]},
                {"manNW", _atlas["scheme1/palette3_Generic_Male_Large_face2"]},
                {"manNE", _atlas["scheme1/palette3_Generic_Male_Large_face3"]},
                {"womanSE", _atlas["scheme1/palette2_Generic_Female_Large_face0"]},
                {"womanSW", _atlas["scheme1/palette2_Generic_Female_Large_face1"]},
                {"womanNW", _atlas["scheme1/palette2_Generic_Female_Large_face2"]},
                {"womanNE", _atlas["scheme1/palette2_Generic_Female_Large_face3"]},
                {"goblinSE", _atlas["scheme0/palette10_Goblin_Large_face0"]},
                {"goblinSW", _atlas["scheme0/palette10_Goblin_Large_face1"]},
                {"goblinNW", _atlas["scheme0/palette10_Goblin_Large_face2"]},
                {"goblinNE", _atlas["scheme0/palette10_Goblin_Large_face3"]},
                {"dragonSE", _atlas["scheme0/palette6_Drakeling_Large_face0", 0]},
                {"dragonSW", _atlas["scheme0/palette6_Drakeling_Large_face1", 0]},
                {"dragonNW", _atlas["scheme0/palette6_Drakeling_Large_face2", 0]},
                {"dragonNE", _atlas["scheme0/palette6_Drakeling_Large_face3", 0]},
            };
            _texture = _textures["dragonSW"];
            _terrain = new AtlasSprite[10, 10];
            _orderedTerrain = new List<AtlasSprite>(100);

            var x = GraphicsDevice.Width / 2;
            var y = GraphicsDevice.Height;
            _position = new Vector2(x, y / 2);
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    _terrain[i, j] = RandomTerrainTexture(i, j);
                    _orderedTerrain.Add(_terrain[i, j]);
                }
            }
            _orderedTerrain = _orderedTerrain.OrderByDescending(sprite => sprite.GridPosition.X * 1001 + sprite.GridPosition.Y * 1000).ToList();
        }
        private AtlasSprite RandomTerrainTexture(int x, int y)
        {
            return new AtlasSprite(_textures[_terrainNames[_random.Next(_terrainNames.Length)]],
                new Vector2(GraphicsDevice.Width / 2 + (x - y) * 48, GraphicsDevice.Height - (x + y) * 24), new Vector2(x, y));
        }
        public override void Destroy()
        {
        }

        public override void Pause()
        {
        }

        public override void Resume()
        {
        }

        public override void Resize(int width, int height)
        {
        }

        private float _rotation;
        private bool _isRotating;

        public override void Update(float deltaTime)
        {

        }

        public override void Render(float deltaTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();
            foreach (AtlasSprite sprite in _orderedTerrain)
            {
                _spriteBatch.Draw(sprite.Visual, sprite.Position);
            }
            _spriteBatch.Draw(_texture, _position);
            _spriteBatch.End();
        }

        public bool OnTouchDown(Vector2 position, int pointerIndex)
        {
            return true;
        }

        public bool OnTouchUp(Vector2 position, int pointerIndex)
        {
            return true;
        }

        public bool OnTouchDrag(Vector2 position, Vector2 delta, int pointerIndex)
        {
            _position += delta;
            return true;
        }
    }
}
