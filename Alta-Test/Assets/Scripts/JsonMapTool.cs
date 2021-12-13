using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Assets.Scripts
{
    public static class JsonMapTool
    {
        private static readonly string Autosave_Suffix = "Autosave";

        private static string path => $"{Directory.GetCurrentDirectory()}/Assets/Resources/MapData";

        public static string[] GetAllMapNames()
        {
            List<string> filtered = new List<string>();
            var all = Directory.GetFiles(path);


            for (int i = 0; i < all.Length - 1; i++)
            {
                if (!all[i].Contains(".meta"))
                {
                    var filter = all[i].Remove(0, path.Length + 5);
                    filter = filter.Remove(filter.Length - 5, 5);
                    filtered.Add(filter);
                }
            }

            return filtered.ToArray();
        }

        public static void SaveMap(MapData mapData)
        {
            if (mapData.mapName == "")
            {
                mapData.mapName = Autosave_Suffix;
            }


            foreach (var tile in mapData.savedMap)
            {
                if (tile.data.gameObject != null)
                {
                    tile.data.gameObjectName = tile.data.gameObject.name;
                }
            }

            string map = JsonUtility.ToJson(mapData);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            File.WriteAllText(($"{path}/Map_{mapData.mapName}.json"), map);
        }

        public static bool FileExists(string mapName)
        {
            return File.Exists($"{path}/Map_{mapName}.json");
        }

        public static MapData LoadMap(string mapName)
        {
            string file = File.ReadAllText($"{path}/Map_{mapName}.json");
            MapData data = JsonUtility.FromJson<MapData>(file);

            return data;
        }
    }
}
