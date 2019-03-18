using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alien : MonoBehaviour {

    public float speed = 10;
    private Rigidbody2D rigidBody;
    public Sprite startingImage;
    public Sprite altImage;
    private SpriteRenderer spriteRenderer;

    public float secBeforeSpriteChange = 0.5f;
    public GameObject alienBullet;
    public float minFireRateTime = 1.0f;
    public float macFireRateTime = 3.0f;
    public float baseFireWaitTime = 3.0f;

    public Sprite explodedShipImage;

	// Use this for initialization
	void Start () {

        rigidBody = GetComponent<Rigidbody2D>();

        rigidBody.velocity = new Vector2(1,0) * speed;

        spriteRenderer = GetComponent<SpriteRenderer>();

       

        StartCoroutine(ChangeAlienSprite());

        baseFireWaitTime = baseFireWaitTime + Random.Range(minFireRateTime, minFireRateTime);

    }

    // Turn in opposite direction
    void Turn(int direction)
    {
        Vector2 newVelocity = rigidBody.velocity;
        newVelocity.x = speed * direction;
        rigidBody.velocity = newVelocity;
    }
	
    // Move down after hitting wall
    void MoveDown()
    {
        Vector2 position = transform.position;
        position.y -= 1;
        transform.position = position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "LeftWall")
        {
            Turn(1);
            MoveDown(); 
        }

        if (collision.gameObject.name == "RightWall")
        {
            Turn(-1);
            MoveDown();
        }

        if (collision.gameObject.name == "Bullet")
        {
            SoundManager.Instance.PlayOneShot(SoundManager.Instance.alienDies);
            Destroy(gameObject);
        }
    }
    public IEnumerator ChangeAlienSprite()
    {
        while (true)
        {
            if(spriteRenderer.sprite == startingImage)
            {
                spriteRenderer.sprite = altImage;
                //.Instance.PlayOneShot(SoundManager.Instance.alienBuzz1);
            }
            else
            {
                spriteRenderer.sprite = startingImage;
                SoundManager.Instance.PlayOneShot(SoundManager.Instance.alienBuzz2);
            }
            yield return new WaitForSeconds(secBeforeSpriteChange);
        }
    }

    void FixedUpdate()
    {
        if (Time.time > baseFireWaitTime)
        {
            baseFireWaitTime = baseFireWaitTime + Random.Range(minFireRateTime, minFireRateTime);

            Instantiate(alienBullet, transform.position, Quaternion.identity);
        }    
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            SoundManager.Instance.PlayOneShot(SoundManager.Instance.shipExplosion);
            collision.GetComponent<SpriteRenderer>().sprite = explodedShipImage;
            Destroy(gameObject);
            DestroyObject(collision.gameObject, 0.5f);
        }
    }
}
