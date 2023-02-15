using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using UnityEditor.SceneManagement;
using System.IO;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEditor.Events;
using InteractiveFramework.Interactions.Items;

namespace InteractiveFramework.DataClassifier.Editor
{
    public class ScenarioAutomation : EditorWindow
    {
        #region Variables
        string oldScenarioName = "old";
        string newScenarioName, newAppName = string.Empty;
        private bool showAudio, showTaskAudio, makeCurrentMainMenu, mainMenuIsCurrent;
        VRApp newApp;
        VRScenario newScenario;
        int scenarioTaskCount = 0;
        Vector2 scrollPos;
        ScenarioTask[] newTasks = new ScenarioTask[] { };
        List<string> newtaskNameList = new List<string>();
        string[] readTaskList = new string[] { };
        ScriptableScenario newScriptable;
        List<ScriptableObject> currentScriptableScenarioList = new List<ScriptableObject>();
        GameObject mainMenu;
        #endregion

        [MenuItem("Interactive Framework/Automate VR App", false, 20)]
        public static void ShowEditorWindow() { GetWindow(typeof(ScenarioAutomation), false, "Automate VR Editor"); }


        void OnGUI()
        {
            EditorGUILayout.BeginVertical();
            EditorGUILayout.Space(10);


            #region App Name & Reset
            EditorGUILayout.BeginHorizontal();
            newAppName = EditorGUILayout.TextField("New App Name", newAppName);

            if (GUILayout.Button("Reset All"))
            {
                newApp = null;
                newAppName = null;
                makeCurrentMainMenu = false;
                mainMenuIsCurrent = false;
                newScenarioName = null;
                scenarioTaskCount = 0;
                newtaskNameList = new List<string>();
                readTaskList = null;
                newTasks = new ScenarioTask[] { };
                currentScriptableScenarioList = new List<ScriptableObject>();
                // clear out the builds
                EditorBuildSettings.scenes = null;
            }

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space(10);

            #endregion

            #region New App
            if (!string.IsNullOrEmpty(newAppName))
            {
                if (newApp == null)
                {
                    if (GUILayout.Button($"Create {newAppName} Folder"))
                    {
                        string filePath = $"Assets/Demos/{newAppName}";
                        // now let's save our scene
                        if (!Directory.Exists(filePath))
                            Directory.CreateDirectory(filePath);
                        AssetDatabase.Refresh();
                        // create the rest
                        CreateAllScenarioFolders();

                        newApp = new VRApp(newAppName);
                    }

                }



                if (GUILayout.Button(mainMenuIsCurrent ? "Switch to Scenario Mode" : "Set Main Menu Mode"))
                {
                    mainMenuIsCurrent = !mainMenuIsCurrent;
                    if (mainMenuIsCurrent)
                    {
                        // make sure we don't need to create another main menu scene
                        if (EditorSceneManager.GetActiveScene().name != "MainMenu")
                            CreateMainMenuScene();

                        CreateMainMenu();
                    }
                }
            }
            else
                newApp = null;


            #endregion

            #region Main Mode or Scenario Mode
            EditorGUILayout.Space(10);
            var style = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter };
            EditorGUILayout.LabelField(mainMenuIsCurrent ? "MAIN MENU MODE" : "SCENARIO MODE", style, GUILayout.ExpandWidth(true));
            GUIStyle labelWrapStyle = new GUIStyle(EditorStyles.textArea);
            style.wordWrap = true;
            if (mainMenuIsCurrent)
                EditorGUILayout.LabelField("Main Menu Mode: Any scenarios you add will immediately become buttons on the main menu interface in your current scene.", labelWrapStyle);
            else
                EditorGUILayout.LabelField("Scenario Mode: New scenarios will become integrated into your current scene", labelWrapStyle);

            EditorGUILayout.Space(10);

            newScenarioName = EditorGUILayout.TextField("Scenario Name", newScenarioName);
            if (oldScenarioName != newScenarioName)
            {
                newScenario = new VRScenario();
                newScenario.scenarioName = newScenarioName;
                oldScenarioName = newScenarioName;
            }
            EditorGUILayout.Space(10);

            showAudio = EditorGUILayout.Foldout(showAudio, "Show Scenario Audio");
            if (showAudio)
            {
                EditorGUILayout.LabelField("Scenario Start Audio");
                EditorGUILayout.ObjectField(new Object(), typeof(AudioClip), true);
                EditorGUILayout.LabelField("Scenario Completed Audio");
                EditorGUILayout.ObjectField(new Object(), typeof(AudioClip), true);
            }
            EditorGUILayout.Space(10);

            #endregion

            #region Task Buttons
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("+ Task"))
            {

                scenarioTaskCount++;
                scenarioTaskCount = Mathf.Clamp(scenarioTaskCount, 0, 20);
                newtaskNameList.Add($"New Task {scenarioTaskCount}");
                readTaskList = newtaskNameList.ToArray();
                newTasks = new ScenarioTask[readTaskList.Length];
            }

            if (GUILayout.Button("- Task"))
            {

                scenarioTaskCount--;
                scenarioTaskCount = Mathf.Clamp(scenarioTaskCount, 0, 20);
                newtaskNameList.RemoveAt(newtaskNameList.Count - 1);
                readTaskList = newtaskNameList.ToArray();
                newTasks = new ScenarioTask[readTaskList.Length];
            }
            if (GUILayout.Button("Reset"))
            {
                scenarioTaskCount = 0;
                newtaskNameList = new List<string>();
                readTaskList = null;
                newTasks = new ScenarioTask[] { };
            }

            #endregion
            EditorGUILayout.EndHorizontal();

            #region Tasks
            EditorGUILayout.BeginVertical();

            EditorGUILayout.Space(10);
            EditorGUILayout.LabelField("Scenario Tasks");
            EditorGUILayout.Space(10);

            scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Width(300), GUILayout.Height(200));
            EditorGUI.indentLevel++;
            if (readTaskList != null)
                for (int i = 0; i <= readTaskList.Length - 1; i++)
                {
                    EditorGUILayout.BeginVertical();
                    // handle task name updating via UI
                    readTaskList[i] = EditorGUILayout.TextField($"Task {i + 1}", readTaskList[i]);

                    // store the updated name
                    newTasks[i] = new ScenarioTask(readTaskList[i]);
                    // if this is the last task...
                    if (i == readTaskList.Length - 1)
                        newTasks[i].isLastTask = true;

                    showTaskAudio = EditorGUILayout.Foldout(showTaskAudio, "Show Task Audio");
                    if (showTaskAudio)
                    {
                        EditorGUILayout.LabelField($"Task {i + 1} Start Audio");
                        EditorGUILayout.ObjectField(new Object(), typeof(AudioClip), true);
                        EditorGUILayout.LabelField($"Task {i + 1} Success Audio");
                        EditorGUILayout.ObjectField(new Object(), typeof(AudioClip), true);
                        EditorGUILayout.LabelField($"Task {i + 1} Fail Audio");
                        EditorGUILayout.ObjectField(new Object(), typeof(AudioClip), true);
                    }
                    GUILayout.Box(GUIContent.none, GUILayout.Width(290), GUILayout.Height(2));
                    EditorGUILayout.EndVertical();

                    EditorGUILayout.Space(5);
                }

            EditorGUI.indentLevel--;

            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();
            #endregion

            if (newScenario != null && !string.IsNullOrEmpty(newScenario.scenarioName))
                if (GUILayout.Button("Save Scenario"))
                    CreateNewScenario(newScenario);

            GUILayout.EndVertical();
        }


        void CreateNewScenario(VRScenario newScenario)
        {
            CreateScriptableData(newScenario);
            ConnectSceneToScenarioData(newScenario);
        }

        void CreateScriptableData(VRScenario newScenario)
        {
            // Create the scriptable object asset
            newScriptable = ScriptableObject.CreateInstance<ScriptableScenario>();
            // set all its data
            newScriptable.scenario = newScenario;
            newScriptable.scenario.tasks = newTasks;
            // path has to start at "Assets"
            string path = $"Assets/Demos/{newAppName}/Scenario Scripts/{newScenario.scenarioName}.asset";
            AssetDatabase.CreateAsset(newScriptable, path);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            currentScriptableScenarioList.Add(newScriptable);
            newApp.scenarios = currentScriptableScenarioList.ToArray();
        }

        void ConnectSceneToScenarioData(VRScenario newScenario)
        {

            // if we are handling the main menu than just add the scenario buttons to the main menu first
            if (mainMenuIsCurrent)
            {
                // now start adding the new scenes for the scenarios
                string pathToNewScene = CreateScenarioScene(newScenario.scenarioName);

                //update the app hud
                if (mainMenu)
                {
                    AppHUDManager ahm = mainMenu.GetComponent<AppHUDManager>();
                    ahm.currentApp = newApp;
                }

                //get the grid parent
                GameObject gridContent = GameObject.Find("Grid Content");
                // add the button
                if (gridContent != null)
                {
                    GameObject scenarioObject = Instantiate(Resources.Load("UI/Scenario Button"), gridContent.transform) as GameObject;
                    scenarioObject.name = newScenario.scenarioName + ": Button";
                    // now update its fields
                    scenarioObject.transform.GetChild(0).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = newScenario.scenarioName;
                    scenarioObject.transform.GetChild(0).transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = $"Tasks ({newScenario.tasks.Count()})";

                    // finally set the persistent listener
                    Button scenarioButton = scenarioObject.GetComponent<Button>();
                    UnityAction<string> action = new UnityAction<string>(mainMenu.GetComponent<AppHUDManager>().LoadSceneFromButton);
                    UnityEventTools.AddStringPersistentListener(scenarioButton.onClick, action, pathToNewScene);

                }
            }
            else// adding scenarios directly
            {

                AddScenarioGameElementsToScene();
            }

        }

        string CreateScenarioScene(string sceneName)
        {
            string path = $"Assets/Demos/{newAppName}/Scenes/{sceneName}.unity";
            // add a new scene
            var newScene = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Additive);
            // switch to new scene as active
            EditorSceneManager.SetActiveScene(newScene);
            // add the required objects to the scene
            AddScenarioGameElementsToScene();
            // save the scene to disk
            bool saveOK = EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene(), string.Join("/", path));
            Debug.Log($"Saved {sceneName} Scene " + (saveOK ? "Success!" : "Error!"));

            // add main menu to build
            AddSceneToBuildSettings(EditorSceneManager.GetActiveScene().path);
            // remove the scene from the editor
            EditorSceneManager.UnloadSceneAsync(EditorSceneManager.GetActiveScene());

            return path;
        }

        void AddScenarioGameElementsToScene()
        {
            GameObject startingEnv = GameObject.Find("Starting Environment");
            if (startingEnv == null)
            {
                startingEnv = Instantiate(Resources.Load("Starting Environment")) as GameObject;
                startingEnv.name = "Starting Environment";
            }

            //now add a new gameobject to Scenario Environment for this scenario
            GameObject scenarioEnv = GameObject.Find("Scenario Environment");
            if (scenarioEnv != null)
            {
                // add a new empty GO to the new scenarioEnv object
                GameObject newScenarioObject = new GameObject(newScenario.scenarioName);
                newScenarioObject.transform.parent = scenarioEnv.transform;

                // finally add a basic interaction setup to get them started
                GameObject basic = Instantiate(Resources.Load("Interactions/Basic Interaction")) as GameObject;
                basic.name = "Basic Interaction Starter";
                basic.transform.parent = newScenarioObject.transform;

                // Update Scenario Manager
                GameObject scenarioManagerObject = GameObject.Find("Scenario Manager");
                if (scenarioManagerObject != null)
                {
                    ScenarioManager scenarioManager = scenarioManagerObject.GetComponent<ScenarioManager>();
                    // handle updating scenario objects
                    List<GameObject> scenarioObjects = new List<GameObject>();
                    // if we currently have other scenario objects set...
                    if (scenarioManager.scenarioObjects.Length > 0)
                        scenarioObjects = scenarioManager.scenarioObjects.ToList();
                    // add the new one
                    scenarioObjects.Add(newScenarioObject);
                    // update the manager
                    scenarioManager.scenarioObjects = scenarioObjects.ToArray();

                    // now connect the scriptable object
                    //first get the one we just made
                    List<ScriptableScenario> scenarioScriptables = new List<ScriptableScenario>();

                    // if we currently have others set...
                    if (scenarioManager.currentScenarios.Length > 0)
                        scenarioScriptables = scenarioManager.currentScenarios.ToList();
                    // add the new one
                    scenarioScriptables.Add(newScriptable);
                    // update the manager
                    scenarioManager.currentScenarios = scenarioScriptables.ToArray();

                    // now let's connect the interactables to the scenario manager
                    UnityAction<int> action = new UnityAction<int>(scenarioManager.SetTaskComplete);
                    UnityEventTools.AddIntPersistentListener(basic.transform.GetChild(0).GetComponent<Item>().itemTriggerSuccess, action, 0);

                }
            }
        }

        void CreateMainMenu()
        {
            // do we need to add main menu UI?
            mainMenu = GameObject.Find("Main Menu");
            if (mainMenu == null)
            {
                mainMenu = Instantiate(Resources.Load("UI/App Main Menu")) as GameObject;
                mainMenu.name = "Main Menu";
            }

            // now set what we can on the UI, namely the app name
            mainMenu.transform.GetChild(1).transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = newAppName;

        }
        void CreateMainMenuScene()
        {
            string path = $"Assets/Demos/{newAppName}/Scenes/MainMenu.unity";
            bool saveOK = EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene(), string.Join("/", path));
            Debug.Log("Saved Main Menu Scene " + (saveOK ? "OK" : "Error!"));

            // add main menu to build
            AddSceneToBuildSettings(EditorSceneManager.GetActiveScene().path);

        }




        private void AddSceneToBuildSettings(string scenePath)
        {
            List<EditorBuildSettingsScene> scenes = new List<EditorBuildSettingsScene>(EditorBuildSettings.scenes);

            // only bother adding scenes we don't have already.
            foreach (EditorBuildSettingsScene scene in scenes)
            {
                if (scene.path == scenePath)
                    return;
            }

            EditorBuildSettingsScene newScene = new EditorBuildSettingsScene();
            newScene.path = scenePath;
            newScene.enabled = true;

            scenes.Add(newScene);
            EditorBuildSettings.scenes = scenes.ToArray();

        }





        void CreateAllScenarioFolders()
        {
            string scenarioPath = $"Assets/Demos/{newAppName}";

            if (!Directory.Exists($"{scenarioPath}/Audio"))
                Directory.CreateDirectory($"{scenarioPath}/Audio");

            if (!Directory.Exists($"{scenarioPath}/Materials"))
                Directory.CreateDirectory($"{scenarioPath}/Materials");

            if (!Directory.Exists($"{scenarioPath}/Prefabs"))
                Directory.CreateDirectory($"{scenarioPath}/Prefabs");

            if (!Directory.Exists($"{scenarioPath}/Scenario Scripts"))
                Directory.CreateDirectory($"{scenarioPath}/Scenario Scripts");

            if (!Directory.Exists($"{scenarioPath}/Scenes"))
                Directory.CreateDirectory($"{scenarioPath}/Scenes");

            AssetDatabase.Refresh();
        }
    }

}
