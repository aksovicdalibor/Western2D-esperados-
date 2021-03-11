using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;

/// <summary>
/// Script for getting all the player inputs, moving a charakter, and instantiating bullets.
/// </summary>
public class PlayerBehaviourScript : MonoBehaviour
{
    /// <summary>
    /// Refference to the game manager for comunication between players and system.
    /// </summary>
    public GameManager gameManager;

    /// <summary>
    /// Audio manager refference initialised later in code.
    /// </summary>
    [HideInInspector]
    public AudioManager audioManager;

    /// <summary>
    /// Rigidbody2d component, used for movement.
    /// </summary>
    public Rigidbody2D rb;
    
    /// <summary>
    /// Transform component StartingPoint used for reseting players position.
    /// </summary>
    public Transform startingPoint;

    /// <summary>
    /// Transform component of the firePoint used when instantiating bullets.
    /// </summary>
    public Transform firePoint;

    /// <summary>
    /// Refference to a bulletPrefab.
    /// </summary>
    public GameObject bulletPrefab;

    /// <summary>
    /// Local properti used as a flag to show if player is ready to continue, or start new game.
    /// </summary>
    public bool ReadyToContinue { get; set; }

    /// <summary>
    /// Local bool variable to distinguish between two players.
    /// </summary>
    public bool isPlayerOne;

    /// <summary>
    /// Variable for controlling movement speed of a players.
    /// </summary>
    public float movementSpeed = 10;

    /// <summary>
    /// Variable showing how muh rotation is applied when moving player to side.
    /// </summary>
    int rotatingAngleInDegrees = 15;

    /// <summary>
    /// Variable storing current movement, used for moving a charakter.
    /// </summary>
    private Vector2 movement;

    private void Start()
    {
        audioManager = GameObject.FindObjectOfType<AudioManager>();
        ReadyToContinue = false;
    }

    void Update()
    {
        if (gameManager.CurrentState == State.Playing)
        {
            CheckForMovement();
            CheckForFire();
        }
        else if(gameManager.CurrentState == State.Paused || gameManager.CurrentState == State.Finished)
        {
            CheckForReadyStateChange();
        }
    }

    private void FixedUpdate()
    {
        MoveAndRotate();
    }

    /// <summary>
    /// Method used for reseting players position and rotation. 
    /// </summary>
    public void Reset()
    {
        transform.position = startingPoint.position;
        if (!isPlayerOne)
        {
            transform.Rotate(0, 0, 180, Space.World);
        }
    }

    /// <summary>
    /// Method used for checking the movement. Called in every frame when game is not paused.
    /// </summary>
    private void CheckForMovement()
    {
        if (isPlayerOne)
        {
            movement.y = Input.GetAxisRaw("Vertical");
        }
        else
        {
            movement.y = Input.GetAxisRaw("Vertical2");
        }
    }

    /// <summary>
    /// Method used for moving and rotating charakter.
    /// </summary>
    private void MoveAndRotate()
    {
        //move
        rb.MovePosition(rb.position + movement * movementSpeed * Time.fixedDeltaTime);

        //rotate
        int offsetForPlayerTwo = 180;

        if (isPlayerOne)
        {
            offsetForPlayerTwo = 0;
        }

        if (movement.y > 0)
        {
            rb.rotation = offsetForPlayerTwo + rotatingAngleInDegrees;
        }
        else if (movement.y < 0)
        {
            rb.rotation = offsetForPlayerTwo - rotatingAngleInDegrees;
        }
        else
        {
            rb.rotation = offsetForPlayerTwo;
        }
    }

    /// <summary>
    /// Method used for checking the firing action. Called in every frame when game is not paused.
    /// </summary>
    private void CheckForFire()
    {
        if (isPlayerOne && Input.GetButtonDown("FireP1") || !isPlayerOne && Input.GetButtonDown("FireP2"))
        {
            Fire();
        }
    }

    /// <summary>
    /// Method used for instantiating bullets, and forwarding them all necessary information.
    /// </summary>
    private void Fire()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        bullet.GetComponent<Bullet>().Continue(gameManager, firePoint, isPlayerOne, rotatingAngleInDegrees);
    }

    /// <summary>
    /// Method used for checking the ready. Called in every frame when game is paused.
    /// </summary>
    private void CheckForReadyStateChange()
    {
        if (isPlayerOne && Input.GetButtonDown("FireP1") || !isPlayerOne && Input.GetButtonDown("FireP2"))
        {
            ReadyToContinue = !ReadyToContinue;
            gameManager.ChangeReadyState(isPlayerOne, ReadyToContinue);
        }

    }
}
