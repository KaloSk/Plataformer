using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntroScript : MonoBehaviour {

    public Button btnIniciar;
    public int Scene;

	// Use this for initialization
	void Start () {
        btnIniciar.onClick.AddListener(ButtonIniciar);
    }
	
	void ButtonIniciar()
    {
        new ChangeScene().CustomChangeScene(Scene);
    }
}
