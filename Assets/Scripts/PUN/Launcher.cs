using Photon.Pun;
using Photon.Realtime;
using System;
using UnityEngine;

namespace OSGames
{
    public class Launcher : MonoBehaviourPunCallbacks
    {
        string gameVersion = "1";
        public byte maxPlayer ;

        private void Awake()
        {
            PhotonNetwork.AutomaticallySyncScene = true;
            Debug.Log("AutoScene Sync");
        }
        public override void OnConnectedToMaster()
        {
            Debug.Log("Launcher is connected to master");
            PhotonNetwork.JoinRandomRoom();

        }
        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            Debug.Log("Joining Random room failed, create new room");
            PhotonNetwork.CreateRoom(null, new RoomOptions{ MaxPlayers = maxPlayer}); 
        }
        public override void OnJoinedRoom()
        {
            Debug.Log("You are joined the room");
        }
        public override void OnDisconnected(DisconnectCause cause)
        {
            base.OnDisconnected(cause);
        }

        public void Connect()
        {
            if (PhotonNetwork.IsConnected)
            {
                Debug.Log("its connected");
                PhotonNetwork.JoinRandomRoom();
            }
            else
            {
                PhotonNetwork.ConnectUsingSettings();
                PhotonNetwork.GameVersion = gameVersion;
            }
        }
    }

}