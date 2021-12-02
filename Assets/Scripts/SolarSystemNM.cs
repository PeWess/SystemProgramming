using System;
using SolarSystemKritskiy_Characters;
using UnityEngine;
using UnityEngine.Networking;

namespace SolarSystemKritskiy_Main
{
    public class SolarSystemNM : NetworkManager
    {

        public override void OnServerAddPlayer(NetworkConnection connection, short playerControllerID)
        {
            var spawnTransform = GetStartPosition();
            var player = Instantiate(playerPrefab, spawnTransform.position, spawnTransform.rotation);
            player.GetComponent<ShipController>().PlayerName = ServiceLocator.Instance.PlayerNickname;
            NetworkServer.AddPlayerForConnection(connection, player, playerControllerID);
        }
    }
}