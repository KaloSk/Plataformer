using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement3D : MonoBehaviour {

    public float angularSpeed;
    public float movementSpeed;
    public Rigidbody rigigbody;
    Vector3 movement;
    Quaternion rotation;

	// Use this for initialization
	void Start () {
        groundCollection = new List<Collider>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        movement = transform.position;
        rotation = rigigbody.rotation;

        float horizontalM = Input.GetAxis("Horizontal");
        float verticalM = Input.GetAxis("Vertical");


        if (Input.GetKey(KeyCode.Q))
        {
            //rotation += Quaternion.Euler(Vector3.up * -angularSpeed * Time.fixedDeltaTime);
        } 
        else if (Input.GetKey(KeyCode.E))
        {
            //rotation += Quaternion.Euler(Vector3.up * angularSpeed * Time.fixedDeltaTime);
        }

        if(horizontalM!=0){
            movement += Vector3.right * movementSpeed * horizontalM * Time.fixedDeltaTime;
        }
        if(verticalM!=0){
            movement += Vector3.forward * movementSpeed * verticalM * Time.fixedDeltaTime;
        }

        rigigbody.MovePosition(movement);
        rigigbody.MoveRotation(rotation);
	}

	private void Update()
	{
        if(Input.GetKeyDown(KeyCode.Space)){
            rigigbody.AddForce(Vector3.up * 5f, ForceMode.Impulse);
        }
	}

    bool grounded;
    int groundCount;
    List<Collider> groundCollection;

    /*
	private void OnCollisionEnter(Collision collision)
	{
        foreach(ContactPoint c in collision.contacts){
            Debug.DrawRay(c.point, c.normal * 5f, Color.magenta, 1f);
            if(Vector3.Dot(c.normal, Vector3.up)>0.75f){
                Debug.Log("SHOULD BE GROUNDED");
                grounded = true;
                groundCount++;
                groundCollection.Add(collision.collider);
            }
        }
	}*/

	private void OnCollisionStay(Collision collision)
	{
        if(!groundCollection.Contains(collision.collider)){
            foreach (ContactPoint c in collision.contacts)
            {
                Debug.DrawRay(c.point, c.normal * 5f, Color.magenta, 1f);
                if (Vector3.Dot(c.normal, Vector3.up) > 0.75f)
                {
                    Debug.Log("SHOULD BE GROUNDED");
                    grounded = true;
                    groundCount++;
                    groundCollection.Add(collision.collider);
                    break;

                }
            }
        }
	}

	private void OnCollisionExit(Collision collision)
	{
        if(groundCollection.Contains(collision.collider)){
            groundCollection.Remove(collision.collider);
        }
        if(groundCollection.Count<=0){
            grounded = false;
        }
	}

}
