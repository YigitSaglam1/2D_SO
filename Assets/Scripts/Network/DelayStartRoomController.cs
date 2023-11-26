using UnityEngine.SceneManagement;
using UnityEngine;
using Photon.Pun;

public class DelayStartRoomController : MonoBehaviourPunCallbacks
{
    [SerializeField] private int waitingRoomSceneIndex;
    public override void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }
    public override void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    public override void OnJoinedRoom()
    {
        SceneManager.LoadScene(waitingRoomSceneIndex);
    }
}
