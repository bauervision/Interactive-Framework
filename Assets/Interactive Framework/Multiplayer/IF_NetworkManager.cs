using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

namespace InteractiveFramework.Multiplayer
{

    public class IF_NetworkManager : NetworkManager
    {
        public override void OnServerAddPlayer(NetworkConnectionToClient conn)
        {
            base.OnServerAddPlayer(conn);
            IF_NetworkPlayer newPlayer = conn.identity.GetComponent<IF_NetworkPlayer>();

            // set the name
            string newPlayerName = $"Player {numPlayers}";
            newPlayer.name = newPlayerName;

            // generate the new data
            DataModel newPlayerData = new DataModel(Random.ColorHSV(), newPlayerName);
            //update the network data
            newPlayer.SetDisplayData(newPlayerData);
        }

    }
}
