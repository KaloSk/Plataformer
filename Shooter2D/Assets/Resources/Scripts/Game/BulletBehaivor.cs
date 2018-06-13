using System.Collections.Generic;
using UnityEngine;

public class BulletBehaivor : MonoBehaviour {
    
    public int BulletSpeed = 10;
    int BulletDamage = 1;
    public int BulletType = 1;

    SpriteRenderer bulletSprite;
    
    /*Sounds Behaivor*/
    public AudioClip bulletSound;
    public AudioClip bulletExplode;
    

    public GameObject bulletExplosion;
    public float bulletExplosionTime = 1;

    //Current AudioSource
    AudioSource audioSource;
    
    // Use this for initialization
    void Start()
    {
        bulletSprite = GetComponent<SpriteRenderer>();
        audioSource = transform.gameObject.GetComponent<AudioSource>();
        audioSource.PlayOneShot(bulletSound);
    }

    // Update is called once per frame
    void Update()
    {
        if(BulletType!=GameConstants.BULLET_TYPE_LASER) transform.Translate(Vector3.up * BulletSpeed * Time.deltaTime);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        SpriteRenderer otherRenderer = other.gameObject.GetComponent<SpriteRenderer>();
        if (transform.gameObject.CompareTag("PlayerLaser"))
        {            
            if (otherRenderer != null && other.gameObject.CompareTag("SimpleEnemy"))
            {
                GameObject enemy = other.gameObject;
                enemy.GetComponent<EnemyBehaivor>().DecreaseLife(BulletDamage);
                audioSource = transform.gameObject.GetComponent<AudioSource>();
                //audioSource.PlayOneShot(bulletExplode);
                //Destroy(transform.gameObject);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        SpriteRenderer otherRenderer = other.gameObject.GetComponent<SpriteRenderer>();
        if(transform.gameObject.CompareTag("PlayerBullet")){
            if (otherRenderer != null && other.gameObject.CompareTag("PlayerBulletWall"))
            {
                Destroy(transform.gameObject);
            }
            if (otherRenderer != null && other.gameObject.CompareTag("SimpleEnemy"))
            {
                GameObject enemy = other.gameObject;
                enemy.GetComponent<EnemyBehaivor>().DecreaseLife((int)PlayerStats.Shooter);
                audioSource = transform.gameObject.GetComponent<AudioSource>();
                audioSource.PlayOneShot(bulletExplode);
                Destroy(transform.gameObject);

                switch (BulletType)
                {
                    case GameConstants.BULLET_TYPE_BASIC:
                        Destroy(Instantiate(bulletExplosion, transform.position, transform.rotation), bulletExplosionTime);
                        break;
                    case GameConstants.BULLET_TYPE_MISILE:
                        Destroy(Instantiate(bulletExplosion, transform.position, transform.rotation), bulletExplosionTime);
                        break;                        
                }
            }
        }

        if(transform.gameObject.CompareTag("EnemyBullet")){
            if (otherRenderer != null && other.gameObject.CompareTag("Player"))
            {
                GameObject player = other.gameObject;
                //player.GetComponent<AudioSource>().PlayOneShot(destroyedEnemy);
                player.GetComponent<PlayerBehaivor>().DecreaseLife(BulletDamage);
                Destroy(gameObject);

                switch (BulletType)
                {
                    case GameConstants.EXPLOSION_SMALL:
                        Destroy(Instantiate(bulletExplosion, transform.position, transform.rotation), bulletExplosionTime);
                        break;
                }
            }
        }
    }
}
