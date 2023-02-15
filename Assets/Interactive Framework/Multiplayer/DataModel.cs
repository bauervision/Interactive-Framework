using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InteractiveFramework.Multiplayer
{
    [System.Serializable]
    public class DataModel
    {
        public string playerName;
        public Color playerColor;

        public DataModel() { }
        public DataModel(Color newColor, string newName)
        {
            this.playerColor = newColor;
            this.playerName = newName;
        }
    }
}
