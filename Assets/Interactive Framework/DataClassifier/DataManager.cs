using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace InteractiveFramework.DataClassifier
{

    public class DataManager : MonoBehaviour
    {
        public static DataManager instance;

        public TextAsset demoData;
        #region Essential Data Elements
        public ClassifierData data;
        public List<Scenario> allScenarios;
        public Scenario currentScenario;

        /// <summary>
        /// Holds the refernce to the game object which hold the mission asset data
        /// </summary>
        public GameObject _libraryAsset;


        #endregion

        void Awake() { instance = this; }
        // Start is called before the first frame update
        void Start()
        {
            // load our data from json first if present
            if (demoData)
            {
                Debug.Log("JSON data found, using that...");
                data = JsonUtility.FromJson<ClassifierData>(demoData.text);
                allScenarios = data.scenarios.ToList();
            }
            else// no json data present use saved stuff
            {
                if (DataSaver.CheckFirstTimeData())
                {
                    Debug.Log("Found saved data file, using that...");
                    LoadSavedData();
                }
                else//no saved data on the disc so create intial data
                {
                    data = new ClassifierData();
                    Debug.Log("No saved found, start fresh");
                }
            }
        }

        #region Asset Related
        public ScenarioActor _newAsset;


        /// <summary>
        /// Called from the UI: when user clicks a library item, but hasnt yet confirmed it
        /// </summary>
        /// <param name="libraryAsset">What GameObject do we store?</param>
        public void SetLibraryAssetToAdd(GameObject libraryAsset)
        {
            //immediately store this reference
            _libraryAsset = Instantiate(libraryAsset);
            // generate the new actor based off what was selected
            _newAsset = new ScenarioActor(libraryAsset.GetComponent<ScenarioAsset>()._actor);

            // since the incoming game object represents the prefab we derive from
            // store it so we can load the resource later
            _newAsset.prefabName = libraryAsset.name;
        }

        /// <summary>
        /// Called from UI when user has confirmed all the data about the new Library asset they want to add to the current mission
        /// </summary>
        public void AddNewAsset()
        {
            // update the data on the gameobject reference first
            _libraryAsset.GetComponent<ScenarioAsset>()._actor = _newAsset;

            // add the asset to the data
            List<ScenarioActor> currentAssetList = currentScenario.assets.ToList();
            currentAssetList.Add(_newAsset);
            currentScenario.assets = currentAssetList.ToArray();

            // now update the data on disc
            List<Scenario> currentScenarioList = new List<Scenario>();
            if (data.scenarios.Length > 0)
            {
                currentScenarioList = data.scenarios.ToList();
                Scenario current = currentScenarioList.Find(m => m.id == currentScenario.id);
                if (current != null)
                    current.assets = currentScenario.assets;
                else
                    Debug.LogWarning("Didn't find " + currentScenario.name + " in the data!");
            }
            else// no missions saved yet
                currentScenarioList.Add(currentScenario);


            data.scenarios = currentScenarioList.ToArray();
            DataSaver.Save_Data("Saved New Mission Asset to: " + currentScenario.name);

            // // add this asset to the asset manager so we can add it to the scene later
            AssetManager.instance.AddActorToMissionLibrary(_libraryAsset);
            // remove the reference
            DestroyImmediate(_libraryAsset);
        }



        #endregion


        private void LoadSavedData()
        {
            //grab the complete data off the disc
            data = DataSaver.Load_Data();
            allScenarios = data.scenarios.ToList();
        }

        /// <summary>
        /// Called from UI Manager
        /// </summary>
        /// <param name="name"></param>
        public void SaveNewScenario(string name, int siteIndex)
        {
            currentScenario = new Scenario(name, siteIndex);
            // update and save the data
            allScenarios.Add(currentScenario);
            data.scenarios = allScenarios.ToArray();
            DataSaver.Save_Data("Saved New Scenario: " + name);

        }

        public Scenario GetNewScenario(string name, int siteIndex)
        {
            currentScenario = new Scenario(name, siteIndex);
            // update and save the data
            allScenarios.Add(currentScenario);
            data.scenarios = allScenarios.ToArray();
            DataSaver.Save_Data("Saved New Scenario: " + name);
            return currentScenario;

        }

        /// <summary>
        /// Called from UIManager when user has confirmed the removal of a selected mission
        /// </summary>
        /// <param name="removeMission"></param>
        public List<Scenario> RemoveScenario(Scenario removeScenario)
        {
            allScenarios.Remove(removeScenario);
            data.scenarios = allScenarios.ToArray();
            DataSaver.Save_Data("Successfully Removed Scenario: " + removeScenario.name);
            return allScenarios;
        }

        /// <summary>
        /// Called when user confirms the removal of a Scenario actor
        /// </summary>
        /// <param name="removeAsset"></param>
        /// <returns></returns>
        public ScenarioActor[] RemoveActor(ScenarioActor removeAsset)
        {
            DataSaver.Save_Data("Successfully Removed Scenario Actor: " + removeAsset.name);

            // get current list of assets
            List<ScenarioActor> curActors = currentScenario.assets.ToList();
            // find the one we need to remove
            int assetIndex = curActors.FindIndex(a => a.id == removeAsset.id);
            if (assetIndex != -1)
                curActors.RemoveAt(assetIndex);

            // update the data
            currentScenario.assets = curActors.ToArray();
            allScenarios.Find(m => m.id == currentScenario.id).assets = currentScenario.assets;
            data.scenarios = allScenarios.ToArray();
            return curActors.ToArray();
        }

        /// <summary>
        /// Called from Save button on main ui
        /// </summary>
        public void SaveScenario()
        {
            // TODO: perform all needed updates to the data
            currentScenario.lastUpdate = System.DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss");
            DataSaver.Save_Data("Saved Scenario: " + currentScenario.name);

        }



    }

}