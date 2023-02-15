using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using EPOOutline;

namespace InteractiveFramework.ProAssets
{

    public class ProMaterial : MonoBehaviour
    {

        public Material[] myMaterials;
        public int matUpdateIndex;
        int myMaterialIndex = 0;

        MeshRenderer meshRenderer;

        public bool materialMode;
        Outlinable outlinable;
        int mouseValue;
        bool isSelected = false;

        void Awake()
        {
            outlinable = this.GetComponent<Outlinable>();
            // check to see what we can adjust
            if (this.GetComponent<MeshRenderer>())
                meshRenderer = this.GetComponent<MeshRenderer>();
            else if (this.transform.GetChild(0).GetComponent<MeshRenderer>())
                meshRenderer = this.transform.GetChild(0).GetComponent<MeshRenderer>();
        }

        private void OnMouseOver()
        {

            if (!EventSystem.current.IsPointerOverGameObject())
            {
                HandleOutline(true);
                if (Input.GetKeyDown(KeyCode.Mouse1))
                {
                    materialMode = !materialMode;
                    outlinable.OutlineParameters.Color = materialMode ? Color.green : Color.yellow;

                }

                if (materialMode)
                    HandleMouseWheel();

            }

        }

        void HandleMouseWheel()
        {

            if (Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                mouseValue = (mouseValue + 1);
            }
            if (Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                mouseValue = (mouseValue - 1);
            }

            UpdateMaterial(mouseValue);


        }

        private void OnMouseExit()
        {
            if (!isSelected)
                HandleOutline(false);
        }

        public void HandleOutline(bool value) { this.transform.GetComponent<Outlinable>().enabled = value; }


        int lastValue;
        public void UpdateMaterial(float value)
        {
            if (meshRenderer == null)
            {
                Debug.LogWarning("Can't update the material of this model. No Mesh Renderer Found");
                return;
            }

            if (value != lastValue)
            {
                if (value < lastValue)
                    myMaterialIndex--;
                else
                    myMaterialIndex++;

                // get the incoming value and clamp it to the range of materials this model has stored
                if (myMaterialIndex > myMaterials.Length - 1)
                    myMaterialIndex = 0;

                if (myMaterialIndex < 0)
                    myMaterialIndex = myMaterials.Length - 1;

                lastValue = (int)value;
                Material[] materialArray = meshRenderer.materials;
                materialArray[matUpdateIndex] = myMaterials[myMaterialIndex];
                meshRenderer.materials = materialArray;
            }


        }
    }

}