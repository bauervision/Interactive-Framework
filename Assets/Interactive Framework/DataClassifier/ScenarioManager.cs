using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace InteractiveFramework.DataClassifier
{
    public class ScenarioManager : MonoBehaviour
    {
        public static ScenarioManager instance;
        [SerializeField]
        public ScriptableScenario[] currentScenarios;
        [SerializeField] TextMeshProUGUI ScenarioText;
        [SerializeField] TextMeshProUGUI CurrentTaskText;
        public GameObject[] scenarioObjects;

        public UnityEvent scenarioStarted = new UnityEvent();
        public UnityEvent scenarioCompleted = new UnityEvent();
        VRScenario currentScenario;
        ScenarioTask currentTask;

        int currentScenarioIndex = 0;

        private void Awake() { instance = this; }

        private void Start()
        {
            // if we dont have any scenario objects, there's nothing to do here
            if (scenarioObjects.Length == 0 || scenarioObjects == null)
                return;
            else
            {
                // hide all scenario object groups
                foreach (GameObject so in scenarioObjects)
                    so.SetActive(false);

                // show the first non completed scenario
                currentScenarioIndex = FindCurrentScenarioIndex();

                if (currentScenarioIndex != -1)
                {
                    currentScenario = ShowCurrentScenarioObject(currentScenarioIndex);
                    scenarioStarted.Invoke();
                }
                else
                {
                    // didnt find any scenarios that need to be completed
                    ScenarioText.text = "All Scenarios Completed!";
                    CurrentTaskText.text = "";
                }
            }

        }

        VRScenario ShowCurrentScenarioObject(int indexOfScenario)
        {
            // show the scenario scene objects
            scenarioObjects[indexOfScenario].SetActive(true);
            // update UI
            UpdateScenarioUI(currentScenarios[indexOfScenario].scenario, false);
            // return the current scenario
            return currentScenarios[indexOfScenario].scenario;
        }

        void UpdateScenarioUI(VRScenario currentScenario, bool scenarioChange)
        {
            ScenarioText.text = "Scenario: " + currentScenario.scenarioName;
            // if we dont have a task, or our scenario has changed, find current, otherwise, just use current
            currentTask = (currentTask == null || scenarioChange) ? FindCurrentTask(currentScenario) : currentTask;
            print("Current Task: " + currentTask.taskName);
            CurrentTaskText.text = currentTask != null ? "Task: " + currentTask.taskName : "";
        }

        public void SetTaskComplete(int taskArrayID)
        {
            currentTask.taskComplete = true;
            print("Current Task Completed: " + currentTask.taskName);

            //completed a whole scenario? advance to the next
            if (currentTask.isLastTask)
            {
                print("Current Task is Last Task...");
                // whole scenario is done
                currentScenario.isCompleted = true;

                // hide the current scenario scene objects
                scenarioObjects[currentScenarioIndex].SetActive(false);

                currentScenarioIndex++;

                // if we have more scenarios to show
                if (currentScenarioIndex <= scenarioObjects.Length - 1)
                {
                    currentScenario = ShowCurrentScenarioObject(currentScenarioIndex);
                    print("Next Scenario loading: " + currentScenario.scenarioName);
                    UpdateScenarioUI(currentScenario, true);
                }
                else// we're done with all Scenarios
                {
                    ScenarioText.text = "All Scenarios Complete!";
                    CurrentTaskText.text = "";

                }
            }
            else
            {
                //not the last task
                taskArrayID++;
                // if we have more to show
                if (taskArrayID <= currentScenario.tasks.Length - 1)
                {
                    currentTask = currentScenario.tasks[taskArrayID];
                    UpdateScenarioUI(currentScenario, false);
                }
            }
        }

        VRScenario FindCurrentScenario()
        {
            int indexOfFirstNonComplete = currentScenarios.ToList().FindIndex(0, currentScenarios.Count(), v => v.scenario.isCompleted != true);
            if (indexOfFirstNonComplete != -1)
                return currentScenarios[indexOfFirstNonComplete].scenario;
            else
            {
                // we didnt find a non completed scenario, so we're done!
                return null;
            }
        }

        int FindCurrentScenarioIndex()
        {
            return currentScenarios.ToList().FindIndex(0, currentScenarios.Count(), v => v.scenario.isCompleted != true);
        }

        ScenarioTask FindCurrentTask(VRScenario currentScenario)
        {
            int indexOfFirstNonComplete = currentScenario.tasks.ToList().FindIndex(0, currentScenario.tasks.Count(), v => v.taskComplete != true);
            if (indexOfFirstNonComplete != -1)
                return currentScenario.tasks[indexOfFirstNonComplete];
            else
            {
                // we didnt find a non completed task in the first scenario, so check the next
                currentScenarioIndex++;
                if (currentScenarioIndex <= scenarioObjects.Length - 1)
                {
                    // keep checking...
                    return FindCurrentTask(currentScenarios[currentScenarioIndex].scenario);
                }
                else// no more scenarios to check?
                    return null;

            }
        }

        int FindFirstNonCompletedTask(VRScenario currentScenario)
        {
            return currentScenario.tasks.ToList().FindIndex(0, currentScenario.tasks.Count(), v => v.taskComplete != true);

        }
    }
}
