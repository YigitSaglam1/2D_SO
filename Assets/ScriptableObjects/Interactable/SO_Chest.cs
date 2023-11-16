using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Chest", menuName ="ScriptableObjects/Interactables/Chest")]
public class SO_Chest : ScriptableObject
{
    public Sprite closedChest;
    public Sprite inRangeChest;
    public Sprite openChest;
}
