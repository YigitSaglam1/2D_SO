using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class QuickStartLobbyController : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject quickStartButton;
    [SerializeField] private GameObject quickCancelButton;
    [SerializeField] private int roomSize;

    private int attempt;

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        quickStartButton.SetActive(true);
    }

    public void QuickStartButton()
    {
        quickStartButton.SetActive(false);
        quickCancelButton.SetActive(true);
        PhotonNetwork.JoinRandomRoom();
        Debug.Log("Quick Start");
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Failed to connect");
        CreateRoom();
    }

    public void CreateRoom()
    {
        int randomRoomNumber = Random.Range(0, 10000); // Creating a random name for a room
        RoomOptions roomOps = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = (byte)roomSize };
        PhotonNetwork.CreateRoom("Room" + randomRoomNumber, roomOps);
        Debug.Log("Room" + randomRoomNumber + " has been created!");
        attempt++; // Room create parameter
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        if (attempt <= 10)
        {
            Debug.Log("Attempt:" + attempt + " Room creation failed... trying again");
            CreateRoom();
        }
    }
    public void QuickCancelButton()
    {
        quickCancelButton.SetActive(false);
        quickStartButton.SetActive(true);
        PhotonNetwork.LeaveRoom();
    }
    void Start()
    {
        
    }


    void Update()
    {
        
    }
}
