using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="new_Tree", menuName ="ScriptableObjects/Interactables/Tree")]
public class SO_Tree : ScriptableObject
{
    public Sprite treeAlive;
    public Sprite treeCut;
    public float treeMaxHealth;
}
