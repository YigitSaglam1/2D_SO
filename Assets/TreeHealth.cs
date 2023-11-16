using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TreeHealth : MonoBehaviour
{
    private float health;

    [SerializeField] private SO_Ally playerSO;
    [SerializeField] private SO_Tree treeSO;
    [SerializeField] private SpriteRenderer spriteRenderer;
    

    [Header("ShakeInfo")]
    private Vector3 _startPos;
    private float _timer;
    private Vector3 _randomPos;

    [Header("ShakeSettings")]
    [Range(0f, 2f)]
    public float _time = 0.2f;
    [Range(0f, 2f)]
    public float _distance = 0.1f;
    [Range(0f, 0.1f)]
    public float _delayBetweenShakes = 0f;
    

    private void Start()
    {
        health = treeSO.treeMaxHealth;
        spriteRenderer.sprite = treeSO.treeAlive;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Tool"))
        {
            GetHit();
        }
    }

    private void GetHit()
    {
        if (health > 0)
        {
            StopAllCoroutines();
            StartCoroutine(Shake());
            health -= playerSO.damage;
            if (health <= 0)
            {
                health = 0;
                spriteRenderer.sprite = treeSO.treeCut;
            }
        }
    }

    private IEnumerator Shake()
    {
        _startPos = transform.position;
        _timer = 0f;

        while (_timer < _time)
        {
            _timer += Time.deltaTime;

            _randomPos = _startPos + (Random.insideUnitSphere * _distance);

            transform.position = _randomPos;

            if (_delayBetweenShakes > 0f)
            {
                yield return new WaitForSeconds(_delayBetweenShakes);
            }
            else
            {
                yield return null;
            }
        }
        transform.position = _startPos;
    }
}
