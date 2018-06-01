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
    public float rayDetectionDistance = 0.1f;

    public Animator playerAnimator;
    public SpriteRenderer playerRenderer;

    public bool isMoving { get { return Input.GetAxis(GameConstants.AXIS_HORIZONTAL) != 0 || Input.GetAxis(GameConstants.AXIS_VERTICAL) != 0; } }

    Vector3 leftNode { get { return transform.position - new Vector3(0.7f, playerRenderer.bounds.extents.y, 0); } }
    Vector3 rightNode { get { return transform.position - new Vector3(-playerRenderer.bounds.extents.x+0.5f, playerRenderer.bounds.extents.y, 0); } }

    public Camera currentCamera;

    bool isGrounded;
    // Use this for initialization
    void Start()
    {
        basicHorizontalSpeed = horizontalSpeed;
    }

    // Update is called once per frame
    void Update()
    {

        RaycastHit2D downLeft = Physics2D.Raycast(leftNode, Vector3.down, rayDetectionDistance);
        RaycastHit2D downRight = Physics2D.Raycast(rightNode, Vector3.down, rayDetectionDistance);
        RaycastHit2D sideLeft = Physics2D.Raycast(leftNode + new Vector3(0, 0.1f, 0), Vector3.left, 0.1f);
        RaycastHit2D sideRight = Physics2D.Raycast(rightNode + new Vector3(0, 0.1f, 0), Vector3.right, 0.1f);

        float horizontalMovement = Input.GetAxis(GameConstants.AXIS_HORIZONTAL);
        if (horizontalMovement < 0)
        {
            if (sideLeft)
            {
                horizontalMovement = 0;
            }
        }
        else if (horizontalMovement > 0)
        {
            if (sideRight)
            {
                horizontalMovement = 0;
            }
        }

        if (!isGrounded)
        {
            verticalSpeed -= gravity * Time.deltaTime;

            if (verticalSpeed < 0)
            {
                rayDetectionDistance = -verticalSpeed * Time.deltaTime;
                CheckVerticalClamp(new RaycastHit2D[] { downLeft, downRight });
            }
            else
            {
                if (rayDetectionDistance != 0.1f)
                {
                    rayDetectionDistance = 0.1f;
                }
            }

        }
        else
        {
            if (!downLeft && !downRight)
            {
                isGrounded = false;
            }
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                verticalSpeed = jumpForce;
                isGrounded = false;
            }
        }

        transform.Translate(horizontalMovement * horizontalSpeed * Time.deltaTime, verticalSpeed * Time.deltaTime, 0);

        characterAnimation(horizontalMovement);

        cameraMovement();


    }

    void cameraMovement(){

        currentCamera.transform.position = thisVector(transform.position, true);
        /*if(currentCamera.transform.position.x < 0){
            currentCamera.transform.position = new Vector3(0, currentCamera.transform.position.y, 0);
        } else {*/
        //currentCamera.transform.position = thisVector(transform.position, true);
        //}

        //Vector3 ii = transform.position - currentCamera.transform.position;

        //Debug.Log(ii);

       /* if(ii.x>-3){
            currentCamera.transform.position = thisVector(transform.position, true);
        } else {
            currentCamera.transform.position = thisVector2(transform.position, true);
        }*/
    }

	private Vector3 thisVector(Vector3 t, bool onlyX){
        return new Vector3(t.x, onlyX?0:t.y, -10);
    }

    private Vector3 thisVector2(Vector3 t, bool onlyX)
    {
        return new Vector3(t.x+3, onlyX ? 0 : t.y, -10);
    }

    void characterAnimation(float horizontalMovement){
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
        if (Input.GetKey(KeyCode.LeftShift))
        {
            horizontalSpeed = basicHorizontalSpeed * 1.5f;
        }
        else
        {
            horizontalSpeed = basicHorizontalSpeed;
        }

        if (playerAnimator.GetFloat(GameConstants.LAST_AXIS_HORIZONTAL) < 0) gameObject.GetComponent<SpriteRenderer>().flipX = true;
        else gameObject.GetComponent<SpriteRenderer>().flipX = false;
    }

    void CheckVerticalClamp(RaycastHit2D[] nodeRays)
    {
        foreach (RaycastHit2D ray in nodeRays)
        {
            if (ray && rayDetectionDistance > ray.distance)
            {
                if (ray.distance <= float.Epsilon)
                {
                    float difference = leftNode.y - ray.collider.bounds.ClosestPoint(leftNode).y;
                    transform.Translate(0, -difference, 0);
                }
                else
                {
                    transform.Translate(0, -ray.distance, 0);
                }
                isGrounded = true;
                verticalSpeed = 0;
                break;
            }
        }
    }

    void CheckReposition(RaycastHit2D[] nodeRays)
    {
        Debug.Log(verticalSpeed);
        foreach (RaycastHit2D ray in nodeRays)
        {
            if (ray && verticalSpeed <= 0)
            {
                float distance = ray.collider.transform.localScale.y * ray.collider.bounds.size.y / 2;
                float difference = (leftNode.y - ray.collider.transform.position.y) - distance;
                if (Mathf.Abs(difference) <= 0.5f)
                {
                    transform.Translate(0, -difference, 0);
                    isGrounded = true;
                    verticalSpeed = 0;
                }
                Debug.DrawLine(transform.position + Vector3.down, ray.collider.transform.position, Color.green);
                break;
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawSphere(leftNode, 0.2f);
        Gizmos.DrawSphere(rightNode, 0.2f);
        Gizmos.color = Color.white;
        Gizmos.DrawRay(leftNode, Vector3.down * rayDetectionDistance);
    }

    public void StopAttack()
    {
        playerAnimator.SetBool(GameConstants.ANIMATOR_ATTACK, false);
    }

}
