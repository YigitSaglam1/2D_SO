using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR;

public class Player : MonoBehaviour
{

    Rigidbody2D playerRb;
    Animator animator;
    
    private Vector2 lastMotionVector;
    private Vector3 startVector;
    private Vector3 controlVector;
    private Vector3 endVector;

    private float horizontal;
    private float vertical;
    private float moveLimiter = 0.7f;
    private float slashTime;
    private float slashSpeed = 10f;

    private bool canDash = true;
    private bool canLog = true;
    private bool isMoving = false;
    private bool coroutinePlaying = false;

    private readonly int horizontalSpeedHash = Animator.StringToHash("HorizontalSpeed");
    private readonly int verticalSpeedHash = Animator.StringToHash("VerticalSpeed");

    [SerializeField] private SO_Ally playerSO;
    [SerializeField] private TrailRenderer trailRenderer;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform endPoint;
    [SerializeField] private GameObject topcuk;
    [SerializeField] private GameObject playerCenter;



    private void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        topcuk.SetActive(false);
    }

    private void Update()
    {
        LookAt();
        AdjustStartEndPoint(lastMotionVector);
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash && !coroutinePlaying) 
        {
            StartCoroutine(Dash());
        }
        if (Input.GetKeyDown(KeyCode.F) && canLog && !coroutinePlaying && !isMoving)
        {
            StartCoroutine(handMove());
        }
        if (!coroutinePlaying) { Movement(); }
        
    }


    private void Movement()
    {
        
        animator.SetFloat(horizontalSpeedHash, horizontal);
        animator.SetFloat(verticalSpeedHash, vertical);
        if (horizontal != 0 && vertical != 0) // MOVING
        {
            
            if (horizontal == -1){ spriteRenderer.flipX = true; }
            if (horizontal == 1) { spriteRenderer.flipX = false; }
            horizontal *= moveLimiter;
            vertical *= moveLimiter;
            if (horizontal != 0 || vertical != 0)
            {
                lastMotionVector = new Vector2(horizontal, vertical);
                lastMotionVector = lastMotionVector * 0.75f;

            }
            if (horizontal ==-1)
            {
                spriteRenderer.flipX = true;
            }
        }
        if (horizontal == -1) { spriteRenderer.flipX = true; }
        if (horizontal == 1) { spriteRenderer.flipX = false; }
        if (horizontal != 0 || vertical != 0) // AT LEAST ONE INPUT IS = 0
        {
            lastMotionVector = new Vector2(horizontal, vertical);
            lastMotionVector = lastMotionVector * 0.75f;
            animator.SetBool("isStopped", false);
            isMoving = true;
            animator.SetFloat("LastVertical",lastMotionVector.x);
            animator.SetFloat("LastHorizontal",lastMotionVector.y);
        }
        if (horizontal == 0 && vertical == 0) // ZERO INPUT
        {
            animator.SetBool("isStopped", true);
            isMoving = false;
        }

        playerRb.velocity = new Vector2(horizontal * playerSO.runSpeed, vertical * playerSO.runSpeed);
    }

    private void LookAt()
    {
        Vector3 look = transform.InverseTransformPoint(playerCenter.transform.position);
        float angle = Mathf.Atan2(look.y, look.x) * Mathf.Rad2Deg;
        topcuk.transform.Rotate(0, 0, angle);
    }

    private IEnumerator handMove()
    {
        coroutinePlaying = true;
        canLog = false;
        topcuk.transform.position = startVector;
        topcuk.SetActive(true);
        yield return null;
        while (slashTime < 0.5f)
        {
            slashTime += 1f * Time.deltaTime;

            Vector3 m1 = Vector3.Lerp(startVector, controlVector, slashTime * slashSpeed);
            Vector3 m2 = Vector3.Lerp(controlVector, endVector, slashTime * slashSpeed);
            topcuk.transform.position = Vector3.Lerp(m1, m2, slashTime * slashSpeed);

            yield return null;
        }

        yield return null;
        slashTime = 0f;
        canLog = true;
        coroutinePlaying = false;
        topcuk.SetActive(false);
        topcuk.transform.position = startVector;
    }
    private IEnumerator Dash()
    {
        coroutinePlaying = true;
        canDash = false;
        trailRenderer.emitting = true;
        playerRb.velocity = new Vector2(horizontal * playerSO.dashingPower, vertical * playerSO.dashingPower);
        yield return new WaitForSeconds(playerSO.dashingTime);
        trailRenderer.emitting = false;
        coroutinePlaying = false;
        yield return new WaitForSeconds(playerSO.dashingCooldown);
        canDash = true;
    }

    private void AdjustStartEndPoint(Vector2 lastMotionVector)
    {
        if (horizontal != 0 || vertical != 0)
        {
            if (lastMotionVector == new Vector2(0.75f, 0)) //RIGHT
            {
                startPoint.transform.position = transform.position + new Vector3(0f, 0.75f, 0f);
                endPoint.transform.position = transform.position + new Vector3(0f, -0.75f, 0f);
                startVector = startPoint.transform.position;
                endVector = endPoint.transform.position;
                controlVector = startVector + (endVector - startVector) / 2 + new Vector3(0.75f, 0, 0) * 1.5f;
            }
            if (lastMotionVector == new Vector2(-0.75f, 0)) //LEFT
            {
                startPoint.transform.position = transform.position + new Vector3(0f, -0.75f, 0f);
                endPoint.transform.position = transform.position + new Vector3(0f, 0.75f, 0f);
                startVector = startPoint.transform.position;
                endVector = endPoint.transform.position;
                controlVector = startVector + (endVector - startVector) / 2 + new Vector3(-0.75f, 0, 0) * 1.5f;
            }

            if (lastMotionVector == new Vector2(0, 0.75f)) //UP
            {
                startPoint.transform.position = transform.position + new Vector3(-0.75f, 0f, 0f);
                endPoint.transform.position = transform.position + new Vector3(0.75f, 0f, 0f);
                startVector = startPoint.transform.position;
                endVector = endPoint.transform.position;
                controlVector = startVector + (endVector - startVector) / 2 + new Vector3(0, 0.75f, 0) * 1.5f;
            }
            if (lastMotionVector == new Vector2(0, -0.75f)) //DOWN
            {
                startPoint.transform.position = transform.position + new Vector3(0.75f, 0f, 0f);
                endPoint.transform.position = transform.position + new Vector3(-0.75f, 0f, 0f);
                startVector = startPoint.transform.position;
                endVector = endPoint.transform.position;
                controlVector = startVector + (endVector - startVector) / 2 + new Vector3(0, -0.75f, 0) * 1.5f;
            }
        }
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if (collision.CompareTag("EnemyWeapon"))
        //{
        //    health -= healthGainAmount; // REVÝZE EDÝLECEK !
        //    Debug.Log(health);
        //}
    }
}
