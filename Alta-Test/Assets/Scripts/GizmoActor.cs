using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    [System.Serializable]
    [ExecuteInEditMode]
    public class GizmoActor : MonoBehaviour
    {
        public Vector2 worldPosition;

        Map map;
        private void Update()
        {
            if (map == null)
            {
                map = FindObjectOfType<Map>();
            }
            else
            {
                if (Input.GetMouseButton(0))
                {
                }
            }
        }

        public GizmoActor(Vector2 worldPosition)
        {
            this.worldPosition = worldPosition;
        }

        public void DrawGizmo()
        {
            Gizmos.DrawWireSphere(worldPosition, 10);
        }
    }
}
