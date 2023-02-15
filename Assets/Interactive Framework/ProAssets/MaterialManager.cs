using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.ProBuilder;

namespace InteractiveFramework.ProAssets
{
    public class MaterialManager : MonoBehaviour
    {
        public Material[] floorMaterials;
        public Material[] wallMaterials;

        public GameObject floor;
        public GameObject walls;

        public int floorIndex;
        public int wallIndex;

        public void SetFloorIndex(int value) { floorIndex = value; ChangeFloor(); }
        public void SetWallIndex(int value) { wallIndex = value; ChangeWall(); }

        public void ChangeFloor()
        {
            floor.GetComponent<MeshRenderer>().sharedMaterial = floorMaterials[floorIndex];
        }

        public void ChangeWall()
        {
            walls.GetComponent<MeshRenderer>().sharedMaterial = wallMaterials[wallIndex];
        }
    }
}
