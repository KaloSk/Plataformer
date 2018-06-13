using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaivor : MonoBehaviour {

    /*Enemy Stats*/
    public int EnemyCrushDamage = 1;
    public int EnemyLifePoints = 5;
    public List<GameObject> EnemyPrize;    
    public GameObject EnemyExplosion;
    public GameObject EnemyBullet;
    public GameObject EnemyBullet2;
    public float EnemySpeed;
    public bool EnemyIsBoss;
    /*Audio Source*/
    AudioSource audioSource;
    /*Sounds Behaivor*/
    public AudioClip beingDamage;
    public AudioClip immuneDamage;
    public AudioClip destroyedEnemy;
    public AudioClip EnemyEntrance;
    
    /*Private*/
    private float EnemyDPS = 1;
    int playEnemyEntrance = 0;
    float timeCounter = 0;
    public float speed, width, height, positionX, positionY;
    

    // Use this for initialization
    void Start () {
        timer = new float[] { 0, 0, 0 };
        timerMax = new float[] { 0, 0, 0 };
        timerContinue = new bool[] { false, true, true };
    }

	// Update is called once per frame
	void Update () {
        if (!EnemyIsBoss)
        {
            if (Time.time >= EnemyDPS)
            {
                // Change the next update (current second+1)
                EnemyDPS = Mathf.FloorToInt(Time.time) + 1;
                // Call your fonction
                EnemyShoot(EnemyBullet, "SightDirection");
            }
        }
        else /*IS BOSS*/
        {
            if (!WaitedAndExecute(GameConstants.BOSS_ENEMY_PRESENTATION, 2, 0)) {
                if (playEnemyEntrance == 0)
                {
                    playEnemyEntrance += 1;
                    audioSource = transform.gameObject.GetComponent<AudioSource>();
                    audioSource.PlayOneShot(EnemyEntrance);
                }
                //transform.Translate(Time.deltaTime, 0, 0, Camera.main.transform);
                //Debug.Log("ENEMY IS PRESENTING");
                transform.position = Vector2.MoveTowards(transform.position, new Vector2(0, 5), Time.deltaTime*speed);
                return;
            }

            timeCounter += Time.deltaTime * speed;
            float x = (Mathf.Cos(timeCounter) * width) + positionX;
            float y = (Mathf.Sin(timeCounter) * height) + positionY;
            transform.position = new Vector2(x, y);

            if (WaitedAndExecute(GameConstants.BOSS_ENEMY_BASIC_ATTACK, 2, 0))
            {
                StartCoroutine(EnemyShootTime(EnemyBullet, 10, "SightDirection", 0.9f, false));
            }

            if (EnemyBullet2 != null)
            {
                if (!WaitedAndExecute(GameConstants.BOSS_ENEMY_SPECIAL_ATTACK, 8, 0)) return;
                StartCoroutine(EnemyShootTime(EnemyBullet2, 6, "MisilDirection", 0.2f, true));
            }
        }
    }
    
    void EnemyShoot(GameObject bullet, string target)
    {
        Transform sd = transform.Find(target).GetComponent<Transform>();
        Destroy(Instantiate(bullet, sd.Find("Cannon").position, sd.rotation), 1.2f);
    }

    IEnumerator EnemyShootTime(GameObject enemyBullet, int enemyBulletTotal, string enemyPartShooter, float enemyShootSpeed, bool enemyMultiplePartShooter)
    {
        for(var i = 0; i < enemyBulletTotal; i++)
        {
            EnemyShoot(enemyBullet, enemyPartShooter + (!enemyMultiplePartShooter?string.Empty:(i + 1).ToString()));
            yield return new WaitForSeconds(enemyShootSpeed);
        }
    }

    private float[] timer;
    private float[] timerMax;
    private bool[] timerContinue;
    private bool WaitedAndExecute(int position, float seconds, int extra)
    {
        timerMax[position] = seconds;
        timer[position] += (Time.deltaTime+extra);
        if (timer[position] >= timerMax[position])
        {
            if(timerContinue[position]) timer[position] = 0;
            return true; //max reached - waited x - seconds
        }
        return false;
    }

    public void DecreaseLife(int damage)
    {
        audioSource = transform.gameObject.GetComponent<AudioSource>();
        if (damage == 0)
        {
            audioSource.PlayOneShot(immuneDamage);
        }
        else
        {
            audioSource.PlayOneShot(beingDamage);
            EnemyLifePoints -= damage;
            if (EnemyLifePoints <= 0)
            {
                audioSource.PlayOneShot(destroyedEnemy);
                int c = Random.Range(-1, 2);
                if (c != -1) {
                    if (Random.Range(0, 100) < 35) {
                        Instantiate(EnemyPrize[c], transform.position, transform.rotation);
                    }
                }
                ExplodeEnemy();
                Camera.main.GetComponent<CanvasBehaivor>().score+=1;
            }
        }
    }

    void LateUpdate()
    {
        if (EnemyLifePoints <= 0) {
            Destroy(gameObject);
        }
    }

    public int getLifePoints()
    {
        return EnemyLifePoints;
    }

    public void ExplodeEnemy()
    {
        Destroy(Instantiate(EnemyExplosion, transform.position, transform.rotation), 0.1f);
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        SpriteRenderer otherRenderer = other.gameObject.GetComponent<SpriteRenderer>();
        if (otherRenderer != null && other.gameObject.CompareTag("Player"))
        {
            GameObject player = other.gameObject;
            player.GetComponent<AudioSource>().PlayOneShot(destroyedEnemy);
            player.GetComponent<PlayerBehaivor>().DecreaseLife(EnemyCrushDamage);
            DecreaseLife(EnemyLifePoints);
        }
        else if(otherRenderer != null && other.gameObject.CompareTag("Finish"))
        {
            Destroy(gameObject);
        }
    }

}
