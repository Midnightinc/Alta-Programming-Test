using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    [CreateAssetMenu(fileName = "New Map Data", menuName = "Maps/Data")]
    public class MapData : ScriptableObject
    {
        public string mapName;

        public Tile[] SavedMap;
    }
}
