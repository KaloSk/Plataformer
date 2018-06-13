using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBehaivor : MonoBehaviour {

    AudioSource audioSource;
    /*Sounds Behaivor*/
    public AudioClip audioBeingHeal;
    public AudioClip beingDamage;
    public AudioClip immuneDamage;
    public AudioClip destroyedEnemy;

    /*PLAYER*/
    public int maxPlayerLifePoints = 5;
    public int playerLifePoints = 5;    
    public int playerPoints = 0;
    public float PlayerSpeed = 20;

    public float PlayerLeftMax = -3.5f;
    public float PlayerRightMax = 3.5f;
    public float PlayerUpMax = 7.0f;
    public float PlayerDownMax = -7.0f;

    public List<int> PowerUpList;
    public List<Text> TextList;
    
    // BulletList
    public List<GameObject> bulletList = new List<GameObject>();
    int bulletIndex = 0;

    // Private Pamameters
    List<Axis> axisList = new List<Axis>();

    // Ship Color
    public SpriteRenderer spriteRenderer;

    private Transform bs1 = null;
    private Transform bs2 = null;

    private Transform scenter = null;

    private Transform sd1 = null;
    private Transform sd2 = null;
    
    // Other Parameters
    public Transform sightObject;

    Vector3 mouseWorldPos;

    public Text lifeText;

    public GameObject exploded;

    //PlayerMovement
    public List<Sprite> PlayerSpriteList;

    MyGameEvents gameEvents;

    // Use this for initialization
    void Start () {
        gameEvents = new MyGameEvents();
        Cursor.visible = false;
        axisList.Add(new Axis(GameConstants.HORIZONTAL, KeyCode.A, KeyCode.D));
        axisList.Add(new Axis(GameConstants.VERTICAL, KeyCode.S, KeyCode.W));
    }
	
    void PlayerMovementAnimator(){
        if (Input.GetKey(KeyCode.A))
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = PlayerSpriteList[1];
            gameObject.GetComponent<SpriteRenderer>().flipX = true;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = PlayerSpriteList[1];
            gameObject.GetComponent<SpriteRenderer>().flipX = false;
        }
        else
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = PlayerSpriteList[0];
            gameObject.GetComponent<SpriteRenderer>().flipX = false;
        }

        transform.Translate(Vector3.right * GetAxis(GameConstants.HORIZONTAL) * PlayerSpeed * Time.deltaTime, Space.World);
        transform.Translate(Vector3.up * GetAxis(GameConstants.VERTICAL) * PlayerSpeed * Time.deltaTime, Space.World);

        if (transform.position.x < PlayerLeftMax)
            transform.position = new Vector2(PlayerLeftMax, transform.position.y);
        else if (transform.position.x > PlayerRightMax)
            transform.position = new Vector2(PlayerRightMax, transform.position.y);

        if (transform.position.y < PlayerDownMax)
            transform.position = new Vector2(transform.position.x, PlayerDownMax);
        else if (transform.position.y > PlayerUpMax)
            transform.position = new Vector2(transform.position.x, PlayerUpMax);
    }

    public GameObject pauseUI;
    bool pausedGame;
    public void PauseGame()
    {
        pausedGame = !pausedGame;
        if (pausedGame)
        {
            pauseUI.SetActive(true);
            Time.timeScale = 0;
        }
        else if (!pausedGame)
        {
            pauseUI.SetActive(false);
            Time.timeScale = 1;
        }
    }

    // Update is called once per frame
    void Update () {

        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            PauseGame();
        }

        if (!pausedGame)
        {
            PlayerMovementAnimator();

            mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPos.z = transform.position.z;

            if (PlayerStats.Helper != 0)
            {
                sd1 = transform.Find("SightDirection1").GetComponent<Transform>();
                sd2 = transform.Find("SightDirection2").GetComponent<Transform>();

                sd1.up = (mouseWorldPos - transform.position).normalized;
                sd2.up = (mouseWorldPos - transform.position).normalized;
            }
            else
            {
                transform.Find("SightDirection1").GetComponent<SpriteRenderer>().enabled = false;
                transform.Find("SightDirection2").GetComponent<SpriteRenderer>().enabled = false;
            }


            if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
            {
                Shoot();
            }

            if (Input.GetKeyDown(KeyCode.Q))
            {
                if (maxPlayerLifePoints > playerLifePoints && PowerUpList[PowerUpParameters.HEAL] > 0)
                {
                    DecreasePowerUp(PowerUpParameters.HEAL);
                    gameObject.GetComponent<AudioSource>().PlayOneShot(audioBeingHeal);
                    playerLifePoints += 1;
                }
            }
            else if (Input.GetKeyDown(KeyCode.X))
            {
                if (PowerUpList[PowerUpParameters.BOMB] > 0)
                {
                    DecreasePowerUp(PowerUpParameters.BOMB);
                    ShootMisile();
                }
            }
            //LifePoints
            lifeText.text = (playerLifePoints < 0 ? "00" : playerLifePoints.ToString("00"));
        }
    }

    void LateUpdate()
    {
        Transform sd1 = transform.Find("SightDirection1").GetComponent<Transform>();
        Transform sd2 = transform.Find("SightDirection2").GetComponent<Transform>();

        sightObject.position = (Vector3.Distance(mouseWorldPos, transform.position) >= 1) ? mouseWorldPos : transform.position + sd1.up;
        sightObject.position = (Vector3.Distance(mouseWorldPos, transform.position) >= 1) ? mouseWorldPos : transform.position + sd2.up;

    }

    void Shoot()
    {
        if (PlayerStats.Helper > 0)
        {
            bulletList[bulletIndex].GetComponent<SpriteRenderer>().color = gameEvents.WeaponColor((int)PlayerStats.Helper);
            sd1 = transform.Find("SightDirection1").GetComponent<Transform>();
            sd2 = transform.Find("SightDirection2").GetComponent<Transform>();
            Destroy(Instantiate(bulletList[bulletIndex], sd1.Find("Cannon").position, sd1.rotation), 1.5f);
            Destroy(Instantiate(bulletList[bulletIndex], sd2.Find("Cannon").position, sd2.rotation), 1.5f);
        }

        bulletList[bulletIndex].GetComponent<SpriteRenderer>().color = gameEvents.WeaponColor((int) PlayerStats.Shooter);
        bs1 = transform.Find("BasicShooter1").GetComponent<Transform>();
        Destroy(Instantiate(bulletList[bulletIndex], bs1.Find("Cannon").position, bs1.rotation), 1.5f);
        bs2 = transform.Find("BasicShooter2").GetComponent<Transform>();
        Destroy(Instantiate(bulletList[bulletIndex], bs2.Find("Cannon").position, bs2.rotation), 1.5f);
    }


    void ShootMisile()
    {
        scenter = transform.Find("SightCenter").GetComponent<Transform>();
        Destroy(Instantiate(bulletList[3], scenter.Find("Cannon").position, scenter.rotation), 1f);
        transform.GetComponent<Rigidbody2D>().AddForce(Vector2.down * 10);
    }

    public int getLifePoints()
    {
        return playerLifePoints;
    }

    public GameObject gameOver;
    public GameController gameController;

    public void DecreaseLife(int damage)
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        if (damage == 0)
        {
            audioSource.PlayOneShot(immuneDamage);
        }
        else
        {
            audioSource.PlayOneShot(beingDamage);
            playerLifePoints -= damage;

            if (playerLifePoints <= 0)
            {
                gameController.StopSpawn();
                gameOver.SetActive(true);
                //Destroy(Instantiate(bulletList[bulletIndex], sd1.Find("Cannon").position, sd1.rotation), 1.5f);
                Destroy(Instantiate(exploded, transform.position, transform.rotation), 1.00f);
                Destroy(Instantiate(exploded, transform.position + Vector3.left + Vector3.up/2, transform.rotation), 0.75f);
                Destroy(Instantiate(exploded, transform.position + Vector3.right - Vector3.up / 2, transform.rotation), 0.50f);
                Destroy(gameObject, 0.1f);
            }
        }
    }
    
    public void IncreasePowerUp(int powerUpType)
    {
        PowerUpList[powerUpType] += 1;
        TextList[powerUpType].text = PowerUpList[powerUpType].ToString();
    }

    public void DecreasePowerUp(int powerUpType)
    {
        PowerUpList[powerUpType] -= 1;
        TextList[powerUpType].text = PowerUpList[powerUpType].ToString();
    }
    
    public int GetAxis(string axisName)
    {
        if (axisList != null && axisList.Count != 0)
        {
            Axis axis = axisList.Find(target => target.name == axisName);
            int axisValue = 0;
            if (Input.GetKey(axis.negative))
            {
                axisValue += -1;
                
            }
            if (Input.GetKey(axis.positive))
            {
                axisValue += 1;
                
            }
            
            return axisValue;
        }
        return 0;
    }
}
