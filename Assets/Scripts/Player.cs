using System.Collections;
using UnityEngine;
using Photon.Pun;


public class Player : MonoBehaviourPun
{

    
    private Vector2 lastMotionVector;
    private Vector3 startVector;
    private Vector3 controlVector;
    private Vector3 endVector;
    
    

    private float slashTime;
    private float slashSpeed = 10f;

    private bool canDash = true;
    private bool canLog = true;
    //private bool isMoving = false; //CAN BE USED
    private bool coroutinePlaying = false;


    [SerializeField] private SO_Ally playerSO;
    [SerializeField] private TrailRenderer trailRenderer;
    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform endPoint;
    [SerializeField] private GameObject topcuk;
    [SerializeField] private GameObject playerCenter;

    [Header("Player Classes")]
    public IPlayerMovement playerMovement;
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private PlayerAnimator playerAnimator;

    [Header("Network")]
    private Camera playerCam;

    private void Start()
    {       
        if (!base.photonView.IsMine) { return; }    

        playerCam = Camera.main;
        playerCam.transform.position = new Vector3(transform.position.x , transform.position.y, transform.position.z - 10f);
        topcuk.SetActive(false);
        playerMovement = GetComponent<IPlayerMovement>();
    }

    private void Update()
    {
        if (!base.photonView.IsMine) { return; }
        playerCam.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - 10f);
        LookAt();
        AdjustStartEndPoint(lastMotionVector);
        
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash && !coroutinePlaying) 
        {
            StartCoroutine(Dash());
        }
        if (Input.GetKeyDown(KeyCode.F) && canLog && !coroutinePlaying)
        {
            StartCoroutine(handMove());
        }
        if (!coroutinePlaying) { Movement(); }
        
    }


    private void Movement()
    {


        playerAnimator.SetFloatHoriontal(playerInput.Horizontal);
        playerAnimator.SetFloatVertical(playerInput.Vertical);
        if (playerInput.Horizontal != 0 && playerInput.Vertical != 0) //CROSS MOVING
        {
            if (playerInput.Horizontal != 0 || playerInput.Vertical != 0)
            {
                lastMotionVector = new Vector2(playerInput.Horizontal, playerInput.Vertical);
            }
        }

        if (playerInput.Horizontal != 0 || playerInput.Vertical != 0) // AT LEAST ONE INPUT IS = 0
        {
            lastMotionVector = new Vector2(playerInput.Horizontal, playerInput.Vertical);

            playerAnimator.SetIsStop(false);
            //isMoving = true;
            playerAnimator.SetLastHorizontal(lastMotionVector.x);
            playerAnimator.SetLastVertical(lastMotionVector.y);
        }
        if (playerInput.Horizontal == 0 && playerInput.Vertical == 0) // ZERO INPUT
        {
            playerAnimator.SetIsStop(true);
            //isMoving = false;
        }
        playerMovement.MakeMove(playerInput.Horizontal, playerInput.Vertical, playerSO.runSpeed);
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
        playerMovement.MakeMove(playerInput.Horizontal, playerInput.Vertical, playerSO.dashingPower);
        yield return new WaitForSeconds(playerSO.dashingTime);
        trailRenderer.emitting = false;
        coroutinePlaying = false;
        yield return new WaitForSeconds(playerSO.dashingCooldown);
        canDash = true;
    }

    private void AdjustStartEndPoint(Vector2 lastMotionVector)
    {
            if (lastMotionVector == new Vector2(1f, 0)) //RIGHT
            {
                startPoint.transform.position = transform.position + new Vector3(0f, 0.75f, 0f);
                endPoint.transform.position = transform.position + new Vector3(0f, -0.75f, 0f);
                startVector = startPoint.transform.position;
                endVector = endPoint.transform.position;
                controlVector = startVector + (endVector - startVector) / 2 + new Vector3(0.75f, 0, 0) * 1.5f;
            }
            if (lastMotionVector == new Vector2(-1f, 0)) //LEFT
            {
                startPoint.transform.position = transform.position + new Vector3(0f, -0.75f, 0f);
                endPoint.transform.position = transform.position + new Vector3(0f, 0.75f, 0f);
                startVector = startPoint.transform.position;
                endVector = endPoint.transform.position;
                controlVector = startVector + (endVector - startVector) / 2 + new Vector3(-0.75f, 0, 0) * 1.5f;
            }

            if (lastMotionVector == new Vector2(0, 1f)) //UP
            {
                startPoint.transform.position = transform.position + new Vector3(-0.75f, 0f, 0f);
                endPoint.transform.position = transform.position + new Vector3(0.75f, 0f, 0f);
                startVector = startPoint.transform.position;
                endVector = endPoint.transform.position;
                controlVector = startVector + (endVector - startVector) / 2 + new Vector3(0, 0.75f, 0) * 1.5f;
            }
            if (lastMotionVector == new Vector2(0, -1f)) //DOWN
            {
                startPoint.transform.position = transform.position + new Vector3(0.75f, 0f, 0f);
                endPoint.transform.position = transform.position + new Vector3(-0.75f, 0f, 0f);
                startVector = startPoint.transform.position;
                endVector = endPoint.transform.position;
                controlVector = startVector + (endVector - startVector) / 2 + new Vector3(0, -0.75f, 0) * 1.5f;
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
