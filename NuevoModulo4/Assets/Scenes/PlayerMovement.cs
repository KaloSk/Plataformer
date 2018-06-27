using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour {

    public float horizontalSpeed;
    Vector3 movement;

    public Rigidbody rigidbody3D;
    public float impulseValue;
    public GameObject bulletObject;

    public Image panelIndicator;

    SwitchControl currentSwitch;

	// Use this for initialization
	void Start () {
        panelIndicator.enabled = false;
        panelIndicator.GetComponentInChildren<Text>().enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
        movement = transform.position;
        float horizontalDirection = Input.GetAxis("Horizontal");
        float verticalDirection = Input.GetAxis("Vertical");


        if(Input.GetKey(KeyCode.Q)){
            transform.Rotate(Vector3.up * -200 * Time.deltaTime);
        } 
        else if (Input.GetKey(KeyCode.E))
        {
            transform.Rotate(Vector3.up * 200 * Time.deltaTime);
        }

        if(horizontalDirection!=0)
        {
            movement += (transform.forward*-1) * horizontalDirection * horizontalSpeed * Time.deltaTime;
           
        }
        if (verticalDirection != 0)
        {
            movement += (transform.right) * verticalDirection * horizontalSpeed * Time.deltaTime;
        }

        if(Input.GetKeyDown(KeyCode.Space)){
            rigidbody3D.AddForce(Vector3.up * impulseValue, ForceMode.Impulse);
        } 

        if(Input.GetKeyDown(KeyCode.X)){
            PlayerShoot(bulletObject);
        }

        if(currentSwitch != null && Input.GetKeyDown(KeyCode.Z)){
            currentSwitch.ActivePlataform();
        }

        rigidbody3D.MovePosition(movement);


        IsPlayerIsDead();
	}

    void IsPlayerIsDead(){
        if (transform.position.y < -50)
            transform.position = Vector3.up;
    }

    void PlayerShoot(GameObject go){
        Transform sd1 = transform.Find("Cylinder").Find("G1").GetComponent<Transform>();
        Destroy(Instantiate(go, sd1.Find("Cannon").position, sd1.rotation), 2.5f);
        Transform sd2 = transform.Find("Cylinder").Find("G2").GetComponent<Transform>();
        Destroy(Instantiate(go, sd2.Find("Cannon").position, sd1.rotation), 2.5f);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Switch"))
        {
            currentSwitch = other.GetComponent<SwitchControl>();
            panelIndicator.enabled = true;
            panelIndicator.GetComponentInChildren<Text>().enabled = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Switch"))
        {
            currentSwitch = null;
            panelIndicator.enabled = false;
            panelIndicator.GetComponentInChildren<Text>().enabled = false;
        }
    }
}
