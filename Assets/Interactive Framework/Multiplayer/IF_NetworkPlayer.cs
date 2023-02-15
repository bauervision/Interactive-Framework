using System.Collections;
using System.Collections.Generic;
using Mirror;
using TMPro;
using UnityEngine;

namespace InteractiveFramework.Multiplayer
{

    public class IF_NetworkPlayer : NetworkBehaviour
    {
        [SerializeField] TextMeshProUGUI playerNameText;
        [SerializeField] MeshRenderer playerRenderer;


        [SyncVar(hook = nameof(HandleDataUpdated))]
        [SerializeField]
        public DataModel dataModel;

        [SyncVar(hook = nameof(HandleNameUpdated))]
        string myLocalName;

        #region Server

        [Server]
        public void SetDisplayData(DataModel newData) { dataModel = newData; }

        [Server]
        public void SetDisplayName(string newName)
        {
            //update the local string to trigger the hook
            myLocalName = newName;
            // update the data
            dataModel.playerName = newName;
            // and the UI
            playerNameText.text = newName;
        }

        // called from a client to request change to its data globally
        [Command]
        void CmdSetDisplayName(string newName)
        {
            /* handle client side validation on change*/

            if (newName.Length < 2)
            {
                RpcLogNewName("Name is too short...must be more than two characters");
            }
            else if (newName.Length > 20)
            {
                RpcLogNewName("Name is too long...must be 20 characters or less");
            }
            else
            {
                //each client gets a log of their update
                RpcLogNewName("Successful name change to: " + newName);
                // and then each client gets the updated data, which triggers the hook to push to all connected clients
                SetDisplayName(newName);
            }

        }

        #endregion

        #region Client

        public void HandleDataUpdated(DataModel oldValue, DataModel newValue)
        {
            playerRenderer.material.SetColor("_BaseColor", newValue.playerColor);
            playerNameText.text = newValue.playerName;
        }

        public void HandleNameUpdated(string oldValue, string newValue)
        {
            playerNameText.text = newValue;
        }

        [ContextMenu("Set My Name")]
        void SetMyName()
        {
            // client decides to update their data and tells server
            CmdSetDisplayName("M");//dataModel.playerName + "_BV");
        }

        [ClientRpc]
        void RpcLogNewName(string newName)
        {
            Debug.Log(newName);
        }

        #endregion

    }
}
