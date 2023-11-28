using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour, IPlayerMovement
{
    Rigidbody2D playerRb;

    private void Awake()
    {
        playerRb = GetComponent<Rigidbody2D>();
    }

    public void MakeMove(float horizontal, float vertical, float runSpeed)
    {
        Vector2 dir = new Vector2(horizontal, vertical);

        playerRb.velocity = dir.normalized * runSpeed;
    }
}
