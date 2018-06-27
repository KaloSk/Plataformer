using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchControl : MonoBehaviour {

    public Transform plataform;
    public Vector3 targetPoint;
    public float transitionSpeed;

    int isActive = 0;
    Vector3 initialPoint;
    int keepMoving = 0;

	// Use this for initialization
	void Start () {
        initialPoint = plataform.position;
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
        StartCoroutine(CostantMovement());
        yield return null;
    }

    IEnumerator CostantMovement()
    {



        while (plataform.position != targetPoint && keepMoving == 0)
        {
            plataform.position = Vector3.MoveTowards(plataform.position, targetPoint, transitionSpeed);
            yield return null;
        }
        keepMoving = 1;
        while (plataform.position != initialPoint && keepMoving == 1)
        {
            plataform.position = Vector3.MoveTowards(plataform.position, initialPoint, transitionSpeed);
            yield return null;
        }
        keepMoving = 0;
        StartCoroutine(CostantMovement());
        yield return null;
    }
}
