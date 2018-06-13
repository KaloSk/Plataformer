using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpEvents : MonoBehaviour {

    /****
     * 0 = Health Package
     * 1 = Bomb
     ****/
    public int PowerUpType = 0;
    public AudioClip PickupItem;
	
	// Update is called once per frame
	void Update () {
        
    }
    
    bool facingRight = false;
    int moveSpeed = 10;

    void Flip()
    {
        // Switch the way the player is labelled as facing
        facingRight = !facingRight;

        // Multiply the player's x local scale by -1
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        SpriteRenderer otherRenderer = other.GetComponent<SpriteRenderer>();
        if (otherRenderer != null && other.gameObject.CompareTag("Player"))
        {
            GameObject player = other.gameObject;
            player.GetComponent<AudioSource>().PlayOneShot(PickupItem);
            player.GetComponent<PlayerBehaivor>().IncreasePowerUp(PowerUpType);
            Destroy(gameObject);
        }
    }

}
