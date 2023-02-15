using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using UnityEngine.UI;



namespace InteractiveFramework.DataClassifier
{

    public class UIManager : MonoBehaviour
    {
        public static UIManager instance;

        public Selected_Asset selected_Asset;

        void Awake() { instance = this; }



        #region Asset Related
        GameObject lastSelected;
        ScenarioActor _currentActor;


        public void HandleSelectedAssetData(GameObject selectedObj)
        {
            //immediately store the selected game object
            lastSelected = selectedObj;
            // and create our new actor data
            _currentActor = selectedObj.GetComponent<ScenarioAsset>()._actor;

            // first since we are selecting on something, we need to show the UI panel
            if (!selected_Asset.selectedPanel.activeInHierarchy)
                selected_Asset.selectedPanel.SetActive(true);
            // update the input field
            selected_Asset.name.text = _currentActor.name;

        }


        #endregion



        #region Private Utility
        void ClearOutGrid(GameObject grid)
        {
            if (grid.transform.childCount > 0)
                for (int i = grid.transform.childCount - 1; i >= 0; i--)
                    DestroyImmediate(grid.transform.GetChild(i).gameObject);
        }


        #endregion

    }

}