using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heal : MonoBehaviour
{
    [SerializeField] private SO_Ally playerSO;
    [SerializeField] private GameObject healObject;

    private float healAmount = 5f;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerSO.health += healAmount;
            if (playerSO.health > 100)
            {
                playerSO.health = 100;
            }
            Destroy(healObject);
        }
        Debug.Log(playerSO.health);
    }
}
