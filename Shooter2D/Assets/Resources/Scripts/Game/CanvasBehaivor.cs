using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasBehaivor : MonoBehaviour {

    public int score;
    public Text scoreText;
    public GameObject levelComplete;

    public GameObject Boss;

    public GameController gameController;
    public int levelShotBoss = 5;
    public int levelCompleteScore = 10;

	// Use this for initialization
	void Start () {
    }

    int BossCalled = 0;

	// Update is called once per frame
	void Update () {
        scoreText.text = "Points: " + score;

        if(score>=levelCompleteScore){
            levelComplete.SetActive(true);
            gameController.StopSpawn();
        } else if(score>= levelShotBoss)
        {
            if (BossCalled == 0)
            {
                BossCalled++;
                Instantiate(Boss, new Vector3(0,12,0), Quaternion.identity);
            }
        }
    }

}
