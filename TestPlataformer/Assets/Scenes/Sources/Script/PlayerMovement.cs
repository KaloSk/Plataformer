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
    float basicHorizontalSpeed;

    public float gravity;    
    public float horizontalSpeed = 1;
    public float jumpForce = 1;

    public Animator playerAnimator;

    public bool isMoving { get { return Input.GetAxis(GameConstants.AXIS_HORIZONTAL) != 0 || Input.GetAxis(GameConstants.AXIS_VERTICAL) != 0; } }

    Vector3 leftMode { get { return transform.position - new Vector3(1.3f, 1.4f); }}
    Vector3 rightMode { get { return transform.position + new Vector3(0.35f, -1.4f); } }

    bool isGrounded;
    // Use this for initialization
    void Start()
    {
        basicHorizontalSpeed = horizontalSpeed;
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
        if(Input.GetKey(KeyCode.LeftShift))
        {
            horizontalSpeed = basicHorizontalSpeed * 1.5f;
        } else
        {
            horizontalSpeed = basicHorizontalSpeed;
        }

        if (playerAnimator.GetFloat(GameConstants.LAST_AXIS_HORIZONTAL) < 0) gameObject.GetComponent<SpriteRenderer>().flipX = true;
        else gameObject.GetComponent<SpriteRenderer>().flipX = false;

        RaycastHit2D left = Physics2D.Raycast(leftMode, Vector3.down, 0.20f);
        RaycastHit2D right = Physics2D.Raycast(rightMode, Vector3.down, 0.20f);

        if(left || right){
            isGrounded = true;
            verticalSpeed = 0;
            CheckReposition(new RaycastHit2D[]{left,right});
        } else{
            isGrounded = false;
        } 

    }

    void CheckReposition(RaycastHit2D[] nodeRays){
        foreach(RaycastHit2D ray in nodeRays){
            if(ray){
                float distance = ray.collider.transform.localScale.y/2;
                float difference = leftMode.y - ray.collider.transform.position.y - distance;
                if(difference!=0){
                    transform.Translate(0, -difference, 0);
                } 
            }
        }
    }

	public void OnDrawGizmos()
	{
        //CODIGO DE CLASE
        Gizmos.DrawSphere(leftMode,0.1f);
        Gizmos.DrawSphere(rightMode, 0.1f);
	}

	public void StopAttack()
    {
        playerAnimator.SetBool(GameConstants.ANIMATOR_ATTACK, false);
    }

}
