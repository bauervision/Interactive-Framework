using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InteractiveFramework.DataClassifier
{

    [CreateAssetMenu(fileName = "New App", menuName = "New VR App", order = 0)]
    public class ScriptableApp : ScriptableObject
    {
        public VRApp app;

    }


}
