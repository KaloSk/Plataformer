using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishLevel : MonoBehaviour {

    public static int count;

	// Use this for initialization
	void Start () {
        count++;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
        if(collision.CompareTag("Player")){
            count--;
        }
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        if(CheckCount() && currentScene < SceneManager.sceneCountInBuildSettings -1){
            ResetCount();
            SceneManager.LoadScene(currentScene+1);
        }
        Destroy(gameObject);
	}

    bool CheckCount(){
        return count == 0;
    }

    public static void ResetCount(){
        count = 0;
    }
}
