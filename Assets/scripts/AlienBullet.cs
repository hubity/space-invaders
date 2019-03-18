using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienBullet : MonoBehaviour {

    private Rigidbody2D rigidBody;

    public float speed = 30;

    public Sprite explodedShipImage;

	// Use this for initialization
	void Start () {

        rigidBody = GetComponent<Rigidbody2D>();

        rigidBody.velocity = Vector2.down * speed;
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Wall")
        {
            Destroy(gameObject);
        }
        if(collision.tag == "Player")
        {
            SoundManager.Instance.PlayOneShot(SoundManager.Instance.shipExplosion);

            collision.GetComponent<SpriteRenderer>().sprite = explodedShipImage;

            DestroyObject(collision.gameObject, 0.5f);
        }

        if (collision.tag == "Shield")
        {
            Destroy(gameObject);
            DestroyObject(collision.gameObject);
        }
    }

     void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

}
