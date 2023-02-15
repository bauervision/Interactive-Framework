using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;

namespace InteractiveFramework.EditorUtiliities
{

    public class VRFMenuItems : EditorWindow
    {
        #region Prefabs
        [MenuItem("VR Framework/Interactions/Basic Interaction", false, 50)]
        public static void AddBasicInteraction()
        {
            GameObject basic = Instantiate(Resources.Load("Interactions/Basic Interaction")) as GameObject;
            basic.name = "Basic Interaction Example";
        }

        [MenuItem("VR Framework/Interactions/Collectible", false, 50)]
        public static void AddCollectible()
        {
            GameObject basic = Instantiate(Resources.Load("Interactions/Interactive Item")) as GameObject;
            basic.name = "Collectible";
        }

        [MenuItem("VR Framework/Interactions/Trigger", false, 50)]
        public static void AddTrigger()
        {
            GameObject basic = Instantiate(Resources.Load("Interactions/Trigger Point")) as GameObject;
            basic.name = "Trigger";
        }

        #endregion

        [MenuItem("VR Framework/Add Simple Player", false, 50)]
        public static void AddSimplePlayer()
        {
            GameObject simpleController = Instantiate(Resources.Load("SimplePersonController")) as GameObject;
            simpleController.name = "Simple Player";
        }

        [MenuItem("VR Framework/Add Interaction Scene", false, 50)]
        public static void AddInteractionScene()
        {
            GameObject startingEnv = Instantiate(Resources.Load("Starting Environment")) as GameObject;
            startingEnv.name = "Starting Environment";
        }


    }

}
