using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    /// <summary>
    /// Rfference to the game manager for comunicating when hit has been detected.
    /// </summary>
    public GameManager gameManager;

    /// <summary>
    /// Refference to the single audioManager script, for playing various sounds.
    /// </summary>
    public AudioManager audioManager;

    /// <summary>
    /// Transform variable for determining angle at whitch to fire bullet.
    /// </summary>
    public Transform firePointTransform;

    /// <summary>
    /// BGameObject prefab holding all animations for bullet.
    /// </summary>
    public GameObject  bulletAnimation;

    /// <summary>
    /// float falue saying how much force is applied to the bullet upon firing.
    /// </summary>
    public float bulletForce = 10;

    /// <summary>
    /// Local bool for destinguishment between the shooter.
    /// </summary>
    private bool shotByPlayerOne;

    /// <summary>
    /// Angle for whitch bullet should rotate upon hitting the side walls (mirrors).
    /// </summary>
    private float reflectAngle;
    

    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    /// <summary>
    ///Decided to forward GameManager's Script instead using GameObject.Find("GameManager").GetComponent<GameManager>() for every bullet.
    /// Same for audioManager and SpriteRenderer.
    /// Used this method as late Start, because of parameters.
    /// </summary>
    public void Continue(GameManager sentGameManager, Transform firePoint, bool isPlayerOne, float rotatingAngleInDegrees)
    {
        gameManager = sentGameManager;
        audioManager = GameObject.FindObjectOfType<AudioManager>();
        firePointTransform = firePoint;
        shotByPlayerOne = isPlayerOne;
        reflectAngle = rotatingAngleInDegrees;
        //Debug.Log("GM set");
        
        audioManager.Play("RevolverShot");

        GameObject animation = Instantiate(bulletAnimation, transform.position, transform.rotation);
        animation.GetComponent<BulletAnimationManager>().PlayAnimation("HasFired");

        Rigidbody2D rb = gameObject.GetComponent<Rigidbody2D>();
        rb.AddForce(firePointTransform.up * bulletForce, ForceMode2D.Impulse);
        Debug.Log("Player " + (shotByPlayerOne ? "One" : "Two") + " has fired!!");
    }
    
    /// <summary>
    /// Method for deflecting a bullet by rotating it.
    /// </summary>
    /// <param name="name"></param>
    private void Deflect(string name)
    {
        gameObject.GetComponent<Rigidbody2D>().rotation = - 90 + reflectAngle * (name.Equals("BottomMirror") ? 1 : -1);
    }

    /// <summary>
    /// Different cases for when the collision happens.
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("collision happened..");
        Debug.Log(collision.collider.tag);

        GameObject animation = Instantiate(bulletAnimation, transform.position, transform.rotation);

        switch (collision.collider.tag)
        {
            case "Player1":
                //animator.SetBool("HasHitPlayer", true);
                animation.GetComponent<BulletAnimationManager>().PlayAnimation("HasHitPlayer");
                gameManager.PlayerShot(true);
                DestroyBullet();
                break;

            case "Player2":
                //animator.SetBool("HasHitPlayer", true);
                animation.GetComponent<BulletAnimationManager>().PlayAnimation("HasHitPlayer");
                gameManager.PlayerShot(false);
                DestroyBullet();
                break;
                
            case "Mirror":
                //animator.SetBool("HasHitElse", true);
                animation.GetComponent<BulletAnimationManager>().PlayAnimation("HasHitElse");
                audioManager.Play("Glass");
                Deflect(collision.collider.name);
                break;

            case "Bullet":
                //animator.SetBool("HasHitElse", true);
                animation.GetComponent<BulletAnimationManager>().PlayAnimation("HasHitElse");
                audioManager.Play("Ricochet");
                DestroyBullet();
                break;

            default:
                Debug.Log("Destroyer or Unindentified object hit!!");
                DestroyBullet();
                break;
        }

    }

    public void DestroyBullet()
    {
        Destroy(gameObject);
    }

    public void FreezeBullet()
    {
        Rigidbody2D rb = gameObject.GetComponent<Rigidbody2D>();
        rb.AddForce(transform.up * bulletForce * -1, ForceMode2D.Impulse);
    }
}
