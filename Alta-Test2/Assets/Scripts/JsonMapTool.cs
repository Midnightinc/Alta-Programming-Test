using System.IO;
using UnityEngine;

namespace Assets.Scripts
{
    public static class JsonMapTool
    {
        private static string path => $"{Directory.GetCurrentDirectory()}/Assets/Resources/MapData";
        public static void SaveMap(MapData mapData)
        {
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
