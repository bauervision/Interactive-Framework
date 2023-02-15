using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace InteractiveFramework.DataClassifier
{

    public class AppHUDManager : MonoBehaviour
    {
        public static AppHUDManager instance;
        public VRApp currentApp;

        public GameObject gridContent;

        // Start is called before the first frame update
        void Awake() { instance = this; }

        private void Start()
        {
            // when we start, run through and make sure our HUD is up to date with the scriptables "Is Completed"
            gridContent = GameObject.Find("Grid Content");
            UpdateHUDScenarios();
        }

        // Update is called once per frame
        void Update()
        {

        }


        void UpdateHUDScenarios()
        {
            // update their UI based on the scriptable
            for (int i = 0; i < gridContent.transform.childCount; i++)
            {
                GameObject childButtonObj = gridContent.transform.GetChild(i).gameObject;
                // match up with the current scenario data
                ScriptableScenario currentScenario = currentApp.scenarios[i] as ScriptableScenario;
                // set color of check mark
                childButtonObj.transform.GetChild(0).transform.GetChild(2).gameObject.GetComponent<Image>().color = currentScenario.scenario.isCompleted ? Color.green : Color.grey;
            }
        }

        public void SetCurrentApp(VRApp app)
        {
            currentApp = app;
        }

        public void LoadSceneFromButton(string pathToNewScene)
        {
            SceneManager.LoadScene(pathToNewScene);
        }

    }

}
