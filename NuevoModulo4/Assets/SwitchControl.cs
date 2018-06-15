using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchControl : MonoBehaviour {

    public Transform plataform;
    public Vector3 targetPoint;
    public float transitionSpeed;

    int isActive = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ActivePlataform(){
        if(isActive == 0){
            StartCoroutine(MovePlataformToTargetPoint());
            isActive = 1;
        }
    }

    IEnumerator MovePlataformToTargetPoint(){
        while(plataform.position != targetPoint){
            plataform.position = Vector3.MoveTowards(plataform.position, targetPoint, transitionSpeed);
            yield return null;
        }
        isActive = 0;
        yield return null;
    }
}
