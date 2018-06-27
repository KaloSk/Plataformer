using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamBehavior : MonoBehaviour {

    public float speed;
    public Vector3 targetDistance;
    public Transform target;
    Vector3 targetNode;

    public float currentUp;

	void LateUpdate()
	{
        targetNode = target.position + (target.right * targetDistance.x) + (target.up * targetDistance.y) + (target.forward * targetDistance.z);
        transform.position = Vector3.MoveTowards(transform.position, targetNode, 5f * Time.deltaTime);
        transform.LookAt(target.position + Vector3.up * currentUp);
	}

}
