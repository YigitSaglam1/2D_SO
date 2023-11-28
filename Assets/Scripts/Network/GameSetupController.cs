using Photon.Pun;
using System.IO;
using UnityEngine;

public class GameSetupController : MonoBehaviour
{
    public float minX;
    public float maxX;
    public float minY;
    public float maxY;

    void Start()
    {
        CreatePlayer();
    }

  
    private void CreatePlayer()
    {
        Debug.Log("Creating Player");
        Vector2 randomPosition = new Vector2(Random.Range(maxX, maxX), Random.Range(minY, maxY));
        PhotonNetwork.Instantiate(Path.Combine("Prefabs","Player"),new Vector3 (randomPosition.x, randomPosition.y, 0), Quaternion.identity);
    }
}
