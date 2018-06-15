using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {

    public Transform lookTarget;
    public Vector3 targetPoint;
    Vector3 localPoint;

	// Use this for initialization
	void Start () { 
	}
	
	// Update is called once per frame
	void LateUpdate () {

        //Vector3 tt = new Vector3(transform.position.x, transform.position.y, lookTarget.position.z*-1);
        localPoint=  lookTarget.position + (lookTarget.right*targetPoint.x) + (lookTarget.up*targetPoint.y) + (lookTarget.forward*targetPoint.z);
        transform.position = Vector3.MoveTowards(transform.position, localPoint, 5f * Time.deltaTime);

        transform.LookAt(lookTarget);

        /*transform.LookAt(lookTarget);
        float difference = Mathf.Abs(transform.position.y - lookTarget.position.y);
        if(difference != targetHeight){
            Vector3 temp = transform.position;
            //temp.y = Mathf.MoveTowards(temp.y,targetHeight, 5f * Time.deltaTime);
            temp.y = Mathf.MoveTowards(temp.y, lookTarget.position.y + targetHeight, 5f * Time.deltaTime);
            transform.position = temp;
        }
        if(transform.position.x != lookTarget.position.x){
            Vector3 temp = transform.position;
            temp.x = Mathf.MoveTowards(temp.x, lookTarget.position.x, 5f * Time.deltaTime);
            transform.position = temp;
        }
        if(transform.position.z != lookTarget.position.z){
            Vector3 temp = transform.position;
            temp.z = Mathf.MoveTowards(temp.z, lookTarget.position.z - zDistance, 3f * Time.deltaTime);
            transform.position = temp;
        }*/
	}
}
