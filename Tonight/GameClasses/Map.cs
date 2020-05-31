using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;
using TiledSharp;
using SFML.System;
using SFML.Graphics;

namespace Tonight
{
    public class Map : Drawable, IUpdatable
    {
        public const int Wall = 86;
        public readonly TmxMap tmxMap;
        public readonly float Width;
        public readonly float Height;
        public readonly int WidthInTiles;
        public readonly int HeightInTiles;
        public readonly int TileSize;
        public Hero Player;
        public List<Bullet> Bullets;
        private readonly List<TmxLayer> layers;
        public readonly TmxLayer collisionTiles;
        private readonly Dictionary<int, Tuple<IntRect, Texture>> matchingGidTexture;
        public readonly Dictionary<string, List<Object>> mapObjects;
        private View view;

        public Map(string pathName, View view)
        {
            this.view = view;
            tmxMap = new TmxMap(pathName);
            WidthInTiles = tmxMap.Width;
            HeightInTiles = tmxMap.Height;
            TileSize = tmxMap.TileWidth;
            layers = ConvertDrawableLayers(tmxMap.Layers);
            collisionTiles = GetCollisionTilesLayer(tmxMap.Layers);
            matchingGidTexture = ConvertGidDict(tmxMap.Tilesets);
            mapObjects = ConvertObjects(tmxMap.ObjectGroups);
            Width = WidthInTiles * TileSize;
            Height = HeightInTiles * TileSize;
            Bullets = new List<Bullet>();
        }

        public Vector2f GetStartPlayerCoordinates() => mapObjects["player"][0].Position;

        public List<Object> GetEnemies() => mapObjects["enemies"];
        public bool IsSegmentIntersectsWithSolidObjects(Vector2f startSegment, Vector2f endSegment)
        {
            return true;
        }
        public int GetTileGidInLayer(Vector2i mapPoint, TmxLayer layer)
        {
            return layer.Tiles[mapPoint.X + mapPoint.Y * WidthInTiles].Gid;
        }
        public SinglyLinkedList<Vector2i> FindPathInTiles(Vector2i from, Vector2i to)
        {
            if (GetTileGidInLayer(to, collisionTiles) == Wall)
                return null;
            var bfsQueue = new Queue<SinglyLinkedList<Vector2i>>();
            var visited = new HashSet<Vector2i>();
            bfsQueue.Enqueue(new SinglyLinkedList<Vector2i>(from));
            visited.Add(from);

            while (bfsQueue.Count != 0)
            {
                var currentLinkedList = bfsQueue.Dequeue();
                var currentPoint = currentLinkedList.Value;
                if (currentPoint == to)
                    return currentLinkedList;
                
                if (collisionTiles.Tiles[currentPoint.X + currentPoint.Y * WidthInTiles].Gid == 86)
                    continue;

                visited.Add(currentPoint);
                for (var dy = -1; dy <= 1; dy++)
                {
                    for (var dx = -1; dx <= 1; dx++)
                    {
                        if (dx == 0 && dy == 0)
                            continue;
                        
                        var neighbour = new Vector2i(currentPoint.X + dx, currentPoint.Y + dy);
                        if (visited.Contains(neighbour))
                            continue;
                        visited.Add(neighbour);
                        bfsQueue.Enqueue(new SinglyLinkedList<Vector2i>(neighbour, currentLinkedList));
                    }
                }
            }

            return null;
        }

        private List<TmxLayer> ConvertDrawableLayers(TmxList<TmxLayer> layersSet) => layersSet.Skip(1).ToList();
        private TmxLayer GetCollisionTilesLayer(TmxList<TmxLayer> layersSet) => layersSet[0];
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
                    var numberOfTile = layer.Tiles[i * WidthInTiles + j].Gid;
                    if (numberOfTile == 0)
                        continue;
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

        public Vector2i ConvertToTileCoordinates(Vector2f position)
        {
            return new Vector2i((int) position.X / TileSize, (int) position.Y / TileSize);
        }

        public Vector2f ConvertToWindowCoordinates(Vector2i tilePosition)
        {
            return new Vector2f(tilePosition.X * TileSize + TileSize / 2, 
                tilePosition.Y * TileSize + TileSize / 2);
        }
        public void Update(GameTime gameTime)
        {
            Bullets = Bullets
                   .Where(b =>
                   {
                       b.Update(gameTime);
                       return b.IsAlive;
                   })
                   .ToList();
        }
    }
}
