using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledSharp;
using SFML.System;
using SFML.Graphics;

namespace Tonight
{
    public class Map : Drawable
    {
        public readonly TmxMap tmxMap;
        public readonly float Width;
        public readonly float Height;
        public readonly int WidthInTiles;
        public readonly int HeightInTiles;
        public readonly int TileSize;
        public List<SecurityGuy> enemies;
        private readonly TmxList<TmxLayer> layers;
        private readonly Dictionary<int, Tuple<IntRect, Texture>> matchingGidTexture;
        public readonly Dictionary<string, List<Object>> mapObjects;
        private View view;


        public Map(string pathName, View view)
        {
            enemies = new List<SecurityGuy>();
            this.view = view;
            tmxMap = new TmxMap(pathName);
            WidthInTiles = tmxMap.Width;
            HeightInTiles = tmxMap.Height;
            TileSize = tmxMap.TileWidth;
            layers = tmxMap.Layers;
            matchingGidTexture = ConvertGidDict(tmxMap.Tilesets);
            mapObjects = ConvertObjects(tmxMap.ObjectGroups);
            Width = WidthInTiles * TileSize;
            Height = HeightInTiles * TileSize;
        }

        public void Draw(RenderTarget target, RenderStates states)
        {
            foreach (var layer in layers)
            {
                DrawLayer(layer, target);
            }
        }

        private void DrawLayer(TmxLayer layer, RenderTarget target)
        {
            var tilesetSprite = new Sprite();

            for (int i = 0; i < HeightInTiles; i++)
                for (int j = 0; j < WidthInTiles; j++)
                {
                    var numberOfTile = layer.Tiles[i * TileSize + j].Gid;
                    var tuple = matchingGidTexture[numberOfTile];

                    tilesetSprite.Texture = tuple.Item2;

                    tilesetSprite.TextureRect = tuple.Item1;
                    tilesetSprite.Position = new Vector2f(j * TileSize, i * TileSize);
                    target.Draw(tilesetSprite);
                }
        }

        private Dictionary<int, Tuple<IntRect, Texture>> ConvertGidDict(TmxList<TmxTileset> tilesets)
        {
            var gidDict = new Dictionary<int, Tuple<IntRect, Texture>>();

            foreach (var tileset in tilesets)
            {
                var tilesetTexture = new Texture(tileset.Image.Source);

                var widthStart = tileset.Margin;
                var widthInc = tileset.TileWidth + tileset.Spacing;
                var widthEnd = tileset.Image.Width;

                var heightStart = tileset.Margin;
                var heightInc = tileset.TileHeight + tileset.Spacing;
                var heightEnd = tileset.Image.Height;

                var id = tileset.FirstGid;
                for (var height = heightStart; height < heightEnd; height += heightInc)
                {
                    for (var width = widthStart; width < widthEnd; width += widthInc)
                    {
                        var rect = new IntRect(width, height, tileset.TileWidth, tileset.TileHeight);
                        gidDict.Add(id, Tuple.Create(rect, tilesetTexture));
                        id += 1;
                    }
                }
            }
            return gidDict;
        }

        private Dictionary<string, List<Object>> ConvertObjects(IEnumerable<TmxObjectGroup> objectGroups)
        {
            var dict = new Dictionary<string, List<Object>>();

            foreach (var objectGroup in objectGroups)
            {
                var objList = new List<Object>();

                foreach (var o in objectGroup.Objects)
                {
                    var obj = new Object(view, o);

                    objList.Add(obj);
                }

                dict[objectGroup.Name] = objList;
            }

            return dict;
        }
    }
}
