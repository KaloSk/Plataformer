using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBossMovement : MonoBehaviour
{

    private float enemyTime;
    private bool loop = false;

    public float BossSpeed = 10;

    /**LOOP**/
    float timeCounter = 0;
    public float speed, width, height, positionX, positionY;

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= enemyTime)
        {
            // Change the next update (current second+1)
            enemyTime = Mathf.FloorToInt(Time.time) + 1;
            // Call your function
            if (!loop) loop = true;
            else loop = false;
        }

        /*****************
         * BOSS MOVEMENT *
         *****************/

       

    }

}
