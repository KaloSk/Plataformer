using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameConstants
{
    public const string AXIS_HORIZONTAL = "Horizontal";
    public const string AXIS_VERTICAL = "Vertical";

    public const string LAST_AXIS_HORIZONTAL = "LastHorizontal";
    public const string LAST_AXIS_VERTICAL = "LastVertical";

    public const string ANIMATOR_MOVING = "Moving";
    public const string ANIMATOR_JUMP_UP = "JumpUP";
    public const string ANIMATOR_JUMP_DOWN = "JumpDOWN";

    public const string ANIMATOR_ATTACK = "Attack";
}

public class PlayerMovement : MonoBehaviour
{
    
    float verticalSpeed;

    public float gravity;    
    public float horizontalSpeed = 1;
    public float jumpForce = 1;

    public Animator playerAnimator;

    public bool isMoving { get { return Input.GetAxis(GameConstants.AXIS_HORIZONTAL) != 0 || Input.GetAxis(GameConstants.AXIS_VERTICAL) != 0; } }

    bool isGrounded;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!isGrounded)
        {
            verticalSpeed -= gravity * Time.deltaTime;
            //playerAnimator.SetBool(GameConstants.ANIMATOR_JUMP_UP, false);
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                verticalSpeed = jumpForce;
                isGrounded = false;
            }
        }

        /*Debug.Log(verticalSpeed);*/

       

        float horizontalMovement = Input.GetAxis(GameConstants.AXIS_HORIZONTAL);

        transform.Translate(horizontalMovement * horizontalSpeed * Time.deltaTime, verticalSpeed * Time.deltaTime, 0);

        playerAnimator.SetFloat(GameConstants.AXIS_HORIZONTAL, horizontalMovement);

        

        if (verticalSpeed == 0)
        {
            playerAnimator.SetBool(GameConstants.ANIMATOR_JUMP_UP, false);
            playerAnimator.SetBool(GameConstants.ANIMATOR_JUMP_DOWN, false);
        }
        else if (verticalSpeed < 0) /*DOWN*/
        {
            playerAnimator.SetBool(GameConstants.ANIMATOR_JUMP_UP, false);
            playerAnimator.SetBool(GameConstants.ANIMATOR_JUMP_DOWN, true);
        }
        else /*UP*/
        {
            playerAnimator.SetBool(GameConstants.ANIMATOR_JUMP_UP, true);
            playerAnimator.SetBool(GameConstants.ANIMATOR_JUMP_DOWN, false);
        }

        if (isMoving)
        {
            playerAnimator.SetFloat(GameConstants.LAST_AXIS_HORIZONTAL, horizontalMovement);
            playerAnimator.SetBool(GameConstants.ANIMATOR_MOVING, true);
        }
        else
        {
            playerAnimator.SetBool(GameConstants.ANIMATOR_MOVING, false);
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            playerAnimator.SetBool(GameConstants.ANIMATOR_ATTACK, true);
        }

        if (playerAnimator.GetFloat(GameConstants.LAST_AXIS_HORIZONTAL) < 0) gameObject.GetComponent<SpriteRenderer>().flipX = true;
        else gameObject.GetComponent<SpriteRenderer>().flipX = false;
        
    }

    public void StopAttack()
    {
        playerAnimator.SetBool(GameConstants.ANIMATOR_ATTACK, false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        SpriteRenderer otherRenderer = other.GetComponent<SpriteRenderer>();
        if (otherRenderer!=null && otherRenderer.CompareTag("Plataform"))
        {
            isGrounded = true;
            verticalSpeed = 0;
            RaycastHit2D hit2D = Physics2D.Raycast(transform.position, Vector3.down);
            float currentDistance = 0;            
            currentDistance = hit2D.distance;
            Debug.Log(currentDistance);
            //transform.Translate(0, 1 - currentDistance, 0);
        } 
        else if (otherRenderer != null && otherRenderer.CompareTag("Death"))
        {
            Destroy(gameObject);
        }
    }
}
