using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    public float horizontalSpeed;
    Vector3 movement;

    public Rigidbody rigidbody3D;
    public float impulseValue;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        movement = transform.position;
        float horizontalDirection = Input.GetAxis("Horizontal");
        float verticalDirection = Input.GetAxis("Vertical");


        if(Input.GetKey(KeyCode.J)){
            transform.Rotate(Vector3.up * 100 * Time.deltaTime);
        }

        if(horizontalDirection!=0)
        {
            movement += Vector3.right * horizontalDirection * horizontalSpeed * Time.deltaTime;
           
        }
        if (verticalDirection != 0)
        {
            movement += Vector3.forward * verticalDirection * horizontalSpeed * Time.deltaTime;

        }

        if(Input.GetKeyDown(KeyCode.Space)){
            rigidbody3D.AddForce(Vector3.up * impulseValue, ForceMode.Impulse);
        }

        rigidbody3D.MovePosition(movement);


        IsPlayerIsDead();
	}

    void IsPlayerIsDead(){
        if (transform.position.y < -100)
            transform.position = Vector3.up;
    }
}
