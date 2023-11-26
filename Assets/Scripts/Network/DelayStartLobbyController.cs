using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class DelayStartLobbyController : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject delayStartButton;
    [SerializeField] private GameObject delayCancelButton;
    [SerializeField] private int roomSize;

    private int attempt;

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        delayStartButton.SetActive(true);
    }

    public void DelayStartButton()
    {
        delayStartButton.SetActive(false);
        delayCancelButton.SetActive(true);
        PhotonNetwork.JoinRandomRoom();
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
    public void DelayCancelButton()
    {
        delayCancelButton.SetActive(false);
        delayStartButton.SetActive(true);
        PhotonNetwork.LeaveRoom();
    }
}
