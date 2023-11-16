using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Targeter : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private SO_Chest chestSO;

    private bool isInteractable;

    private void Start()
    {
        spriteRenderer.sprite = chestSO.closedChest;
    }
    private void Update()
    {
        if (isInteractable && Input.GetKeyDown(KeyCode.E)) 
        {
            //Interacted With Chest
            spriteRenderer.sprite = chestSO.openChest;
            Debug.Log("Interacted With Chest");
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            spriteRenderer.sprite = chestSO.inRangeChest;
            SetInteractable();//Makes Chest Interactable (Open)
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            spriteRenderer.sprite = chestSO.closedChest;
            SetInteractable();//Makes Chest Interactable (Close)
        }
    }
    private void SetInteractable()
    {
        if (isInteractable)
        {
            isInteractable = false;
        }
        else
        {
            isInteractable = true;
        }
    }
}
