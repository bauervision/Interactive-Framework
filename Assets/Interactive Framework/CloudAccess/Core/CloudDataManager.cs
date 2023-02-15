using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InteractiveFramework.CloudAccess
{
    public class CloudDataManager : MonoBehaviour
    {
        public static CloudDataManager instance;
        string _cloudData;

        private void Awake() { instance = this; }

        public void SetCloudData(string data) { _cloudData = data; }
        public string GetCloudData() { return _cloudData; }


    }
}
