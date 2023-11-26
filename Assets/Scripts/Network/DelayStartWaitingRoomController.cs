using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class DelayStartWaitingRoomController : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMP_Text playerCountDisplay;
    [SerializeField] private TMP_Text timerToStartDisplay;

    [SerializeField] private int multiplayerSceneIndex;
    [SerializeField] private int menuSceneIndex;
    [SerializeField] private int minPlayersToStart;
    [SerializeField] private float maxWaitTime;
    [SerializeField] private float maxFullGameWaitTime;

    private PhotonView m_View;

    private int playerCount;
    private int roomSize;

    private bool readyToCountdown;
    private bool readyToStart;
    private bool startingGame;

    private float timerToStartGame;
    private float notFullGameTimer;
    private float fullGameTimer;

    void Start()
    {
        m_View = GetComponent<PhotonView>();
        fullGameTimer = maxFullGameWaitTime;
        notFullGameTimer = maxWaitTime;
        timerToStartGame = maxWaitTime;

        PlayerCountUpdate();
    }

    private void PlayerCountUpdate()
    {
        playerCount = PhotonNetwork.PlayerList.Length;
        roomSize = PhotonNetwork.CurrentRoom.MaxPlayers;

        playerCountDisplay.text = playerCount + "/" + roomSize;

        if (playerCount == roomSize)
        {
            readyToStart = true;
        }
        else if (playerCount >= minPlayersToStart)
        {
            readyToCountdown = true;
        }
        else
        {
            readyToStart = false;
            readyToCountdown = false;
        }
    }
    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        PlayerCountUpdate();
        if (PhotonNetwork.IsMasterClient)
        {
            m_View.RPC("RPC_SendTimer", RpcTarget.Others, timerToStartGame);
        }
    }
    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        PlayerCountUpdate();
    }
    [PunRPC]
    private void RPC_SendTimer(float timeIn)
    {
        timerToStartGame = timeIn;
        notFullGameTimer = timeIn;
        if (timeIn < fullGameTimer)
        {
            fullGameTimer = timeIn;
        }
    }

    private void Update()
    {
        WaitingForMorePlayers();
    }
    private void WaitingForMorePlayers()
    {
        if (playerCount <= 1)
        {
            ResetTimer();
        }

        if (readyToStart)
        {
            fullGameTimer -= Time.deltaTime;
            timerToStartGame = fullGameTimer;
        }
        else if (readyToCountdown)
        {
            notFullGameTimer -= Time.deltaTime;
            timerToStartGame = notFullGameTimer;
        }

        string tempTimer = string.Format("{0:00}", timerToStartGame);
        timerToStartDisplay.text = tempTimer;
        if (timerToStartGame <= 0f)
        {
            if (startingGame)
            {
                return;
            }
            StartGame();
        }
    }
    private void ResetTimer()
    {
        timerToStartGame = maxWaitTime;
        notFullGameTimer = maxWaitTime;
        fullGameTimer = maxFullGameWaitTime;
    }
    private void StartGame()
    {
        startingGame = true;
        if (!PhotonNetwork.IsMasterClient) { return; }
        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.LoadLevel(multiplayerSceneIndex);
    }

    public void DelayCancelButton()
    {
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene(menuSceneIndex);
    }
}
