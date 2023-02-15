using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InteractiveFramework.Interactions
{
    public class Initializer : MonoBehaviour
    {
        public GameObject[] activate;
        public GameObject[] deActivate;

        void Awake()
        {
            HandleActivation(activate, true);
            HandleActivation(deActivate, false);

        }

        void HandleActivation(GameObject[] objects, bool value)
        {
            if (objects.Length > 0)
                foreach (GameObject a in objects)
                    a.SetActive(value);
        }


    }
}