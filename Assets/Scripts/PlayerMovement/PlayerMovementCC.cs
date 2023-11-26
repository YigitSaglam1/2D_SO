using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementCC : MonoBehaviour, IPlayerMovement
{
    public void MakeMove(float horizontal, float vertical, float runSpeed)
    {

        Debug.Log("Character controllerd moved!");
    }
}
