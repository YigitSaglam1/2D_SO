using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerMovement 
{
    void MakeMove(float horizontal, float vertical, float runSpeed);
}
