using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EPOOutline;

using UnityEngine.UI;
using InteractiveFramework.Interactions;
using InteractiveFramework.ProAssets;
using UnityEngine.EventSystems;

namespace InteractiveFramework.DataClassifier
{
    [RequireComponent(typeof(BoxCollider), typeof(Outlinable), typeof(ProMaterial))]
    public class ScenarioAsset : MonoBehaviour
    {
        public ScenarioActor _actor;
        public GameObject uiPrefab;
        bool isPlaceable;

        public void UpdateMyData(ScenarioActor updatedData) { _actor = updatedData; }

        bool isSelected = false;
        int mouseValue;

        public bool materialMode;
        Outlinable outlinable;

        void Awake()
        {
            outlinable = this.GetComponent<Outlinable>();
        }

        private void OnMouseOver()
        {

            if (!EventSystem.current.IsPointerOverGameObject())
            {
                HandleOutline(true);
                // make sure asset manager stays updated
                if (!this.gameObject.isStatic)
                    AssetManager.instance.SetCurrentAsset(this.gameObject);
            }

        }


        private void OnMouseExit()
        {
            if (!isSelected)
                HandleOutline(false);
        }



        // mouseDown only works if collider is enabled
        private void OnMouseDown()
        {

            if (!EventSystem.current.IsPointerOverGameObject())
            {
                isSelected = true;
                HandleOutline(true);
                GetComponent<BoxCollider>().enabled = false;

                if (!this.gameObject.isStatic)
                {
                    PlacementManager.instance.SetActivePlacer(this.gameObject);

                    // make sure asset manager stays updated
                    AssetManager.instance.SetCurrentAsset(this.gameObject);
                }
            }

        }

        /// <summary>
        /// If a user selects on this game object and hits the delete key, destroy this
        /// </summary>
        private void MonitorDeletion()
        {
            if (Input.GetKeyDown(KeyCode.Delete))
                Destroy(this.transform.gameObject);
        }

        public void HandleOutline(bool value) { this.transform.GetComponent<Outlinable>().enabled = value; }

        public void DeselectModel()
        {
            GetComponent<BoxCollider>().enabled = true;
            HandleOutline(false);
            isSelected = false;
            // generate a new position based on current data
            _actor.position = new ActorPosition();
            _actor.position.positionX = transform.localPosition.x;
            _actor.position.positionY = transform.localPosition.y;
            _actor.position.positionZ = transform.localPosition.z;
            _actor.position.rotationX = transform.localRotation.x;
            _actor.position.rotationY = transform.localRotation.y;
            _actor.position.rotationZ = transform.localRotation.z;
        }

        private void Update()
        {

            if (isSelected)
                MonitorDeletion();

        }
    }
}
