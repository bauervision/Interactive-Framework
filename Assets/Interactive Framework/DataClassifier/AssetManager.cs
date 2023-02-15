using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using InteractiveFramework.Interactions;

namespace InteractiveFramework.DataClassifier
{
    /// <summary>
    /// AssetManager handles all assets that have been added to the scene
    /// </summary>
    public class AssetManager : MonoBehaviour
    {
        public static AssetManager instance;
        public List<GameObject> missionActors = new List<GameObject>();

        public GameObject[] demoPrefabs;
        int activeDemoPrefabIndex = 0;

        GameObject interactiveParent;

        GameObject _currentAsset;

        void Awake()
        {
            instance = this;
            interactiveParent = GameObject.Find("Interactives");
        }

        /// <summary>
        /// Called from the UI: when user click red, green or blue buttons
        /// </summary>
        /// <param name="value"></param>
        public void SetActiveIndex(int value)
        {
            activeDemoPrefabIndex = value;
            HandleNewObjectCreation(demoPrefabs[activeDemoPrefabIndex]);

        }

        public void HandleNewObjectCreation(GameObject prefabToSpawn)
        {
            if (PlacementManager.instance.activePlacer == null)
            {
                PlacementManager.instance.clickedCount = 1;
                _currentAsset = Instantiate(prefabToSpawn, interactiveParent.transform);
                missionActors.Add(_currentAsset);
                PlacementManager.instance.SetActivePlacer(_currentAsset);

            }
        }



        public void AddActorToMissionLibrary(GameObject newActor)
        {
            // make a copy because original will be removed
            missionActors.Add(Instantiate(newActor));

        }

        /// <summary>
        /// Called from the UI: when user edits the input field
        /// </summary>
        /// <param name="value"></param>
        public void UpdateAssetName(string value)
        {
            // update the instance name and the data name
            _currentAsset.name = value;
            _currentAsset.GetComponent<ScenarioAsset>()._actor.name = value;
        }

        public void SetCurrentAsset(GameObject selectedObject) { _currentAsset = selectedObject; }
    }

}
