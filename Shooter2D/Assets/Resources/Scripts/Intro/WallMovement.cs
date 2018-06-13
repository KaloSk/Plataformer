using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallMovement : MonoBehaviour {

    public bool IncludeY;
    public int PP;
    public float WallSpeed = 0.3f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (transform.position.x <= PP)
        {
            transform.position = new Vector3(Mathf.PingPong(Time.time * WallSpeed, PP), (!IncludeY?0:Mathf.PingPong(Time.time * WallSpeed, PP)), transform.position.z);
        }
        else
        {
            transform.position = new Vector3(-Mathf.PingPong(Time.time * WallSpeed, PP), (!IncludeY ? 0 : Mathf.PingPong(Time.time * WallSpeed, PP)), transform.position.z);
        }
	}
}
