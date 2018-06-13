using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionBehaivor : MonoBehaviour {

    public int bulletSpeed = 1;
    public AudioClip bulletSound;
    public AudioClip bulletExplode;
    public int explosionDamage = 1;

    //Current AudioSource
    AudioSource audioSource;

	// Use this for initialization
	void Start () {
        audioSource = transform.gameObject.GetComponent<AudioSource>();
        audioSource.PlayOneShot(bulletSound);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        SpriteRenderer otherRenderer = other.gameObject.GetComponent<SpriteRenderer>();
        if (otherRenderer != null && other.gameObject.CompareTag("SimpleEnemy"))
        {
            GameObject enemy = other.gameObject;
            enemy.GetComponent<EnemyBehaivor>().DecreaseLife(explosionDamage);
            audioSource = transform.gameObject.GetComponent<AudioSource>();
            audioSource.PlayOneShot(bulletExplode);
        }
    }
}
