using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace InteractiveFramework.DataClassifier
{
    [System.Serializable]
    public class VRApp
    {
        public string appName;
        public string version;

        public bool useGraphView;//changes editor display UI for minimal graph view

        /// <summary>
        /// Scenarios contains an array of scriptable objects that can be dragged and dropped right onto this field and will
        /// contain references to all scenarios written for this app
        /// </summary>
        public ScriptableObject[] scenarios;
        // public VRScenario[] scenarios;

        public VRApp(string appName)
        {
            this.appName = appName;
            this.version = "0.0.1";
            this.useGraphView = false;
            this.scenarios = new ScriptableObject[] { };
        }

    }

    [System.Serializable]
    public class ScenarioEvents
    {
        public UnityEvent onScenarioStart = new UnityEvent();
        public UnityEvent onScenarioCompleted = new UnityEvent();
        public UnityEvent onScenarioSuccess = new UnityEvent();
        public UnityEvent onScenarioFail = new UnityEvent();

    }

    [System.Serializable]
    public class ScenarioAudio
    {
        public AudioClip scenarioStartAudio;
        public AudioClip scenarioCompleteAudio;
    }

    [System.Serializable]
    public class VRScenario
    {
        public string scenarioName;
        public bool isCompleted;
        public ScenarioTask[] tasks;
        public ScenarioAudio scenarioAudio;
        public ScenarioEvents scenarioEvents;
    }

    [System.Serializable]
    public class ScenarioTaskEvents
    {
        public UnityEvent onTaskStart = new UnityEvent();
        public UnityEvent onTaskCompleted = new UnityEvent();
        public UnityEvent onTaskSuccess = new UnityEvent();
        public UnityEvent onTaskFail = new UnityEvent();

    }

    [System.Serializable]
    public class ScenarioTaskAudio
    {
        public AudioClip startTaskAudio;
        public AudioClip successTaskAudio;
        public AudioClip failTaskAudio;

    }

    [System.Serializable]
    public class ScenarioTask
    {
        public string taskName;
        public bool taskStarted;
        public bool taskComplete;
        public bool taskFailed;
        public bool isLastTask;

        public ScenarioTaskAudio scenarioTaskAudio;
        public ScenarioTaskEvents scenarioTaskEvents;


        public ScenarioTask() { this.taskName = "New Task..."; }
        public ScenarioTask(string newName) { this.taskName = newName; }


    }


}
