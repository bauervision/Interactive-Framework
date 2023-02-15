using UnityEngine;
using EPOOutline;

using InteractiveFramework.DataClassifier;

namespace InteractiveFramework.Interactions
{

    public class PlacementManager : MonoBehaviour
    {
        public static PlacementManager instance;
        public GameObject activePlacer;
        private float mouseWheelRotation;

        Vector3 MousePosition, TargetPosition;
        public int clickedCount = 0;

        private void Awake() { instance = this; }

        // Update is called once per frame
        void Update()
        {
            if (activePlacer != null)
            {
                // always monitor movement and rotation if we have an active placer
                MoveCurrentObjectToMouse();

                if (!activePlacer.GetComponent<ScenarioAsset>().materialMode)
                    RotateFromMouseWheel();

                if (Input.GetMouseButtonDown(0))
                {
                    clickedCount++;
                    if (clickedCount == 2)
                    {
                        ReleaseIfClicked();
                        clickedCount = 0;
                    }
                }
            }
        }

        public void SetActivePlacer(GameObject newPlacer)
        {
            activePlacer = newPlacer;
            activePlacer.GetComponent<Outlinable>().enabled = true;
        }

        private void MoveCurrentObjectToMouse()
        {

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo))
            {
                activePlacer.transform.position = hitInfo.point;
                //activePlacer.transform.rotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
            }

        }

        private void RotateFromMouseWheel()
        {
            mouseWheelRotation += Input.mouseScrollDelta.y;
            activePlacer.transform.Rotate(Vector3.up, mouseWheelRotation * 10f);
        }


        private void ReleaseIfClicked()
        {
            if (activePlacer)
            {
                activePlacer.GetComponent<ScenarioAsset>().DeselectModel();
                activePlacer = null;
            }

        }



    }
}