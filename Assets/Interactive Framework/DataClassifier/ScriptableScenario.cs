using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace InteractiveFramework.DataClassifier
{

    [CreateAssetMenu(fileName = "New Scenario", menuName = "New VR Scenario", order = 0)]
    public class ScriptableScenario : ScriptableObject
    {
        public VRScenario scenario;

    }
}
