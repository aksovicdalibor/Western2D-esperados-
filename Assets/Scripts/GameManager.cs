using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Class that hold most of the necessary references and is responsible for changing states of the state machine.
/// </summary>
public class GameManager : MonoBehaviour
{
    /// <summary>
    /// References to the scripts used for checking movements of the players, firing etc.
    /// </summary>
    [Header("Players")]
    public PlayerBehaviourScript ScriptPlayerOne;
    public PlayerBehaviourScript ScriptPlayerTwo;

    /// <summary>
    /// References to the GUI objects used for showing users current score.
    /// </summary>
    [Header("Score")]
    public GameObject playerOneText;
    public GameObject playerTwoText;
    
    /// <summary>
    /// Refferences used for changing icons for Ready-state, attached to 
    /// </summary>
    [Header("Scripts for changing ready status Icons")]
    public ChangeIcon ScriptChangeIconOne;
    public ChangeIcon ScriptChangeIconTwo;

    /// <summary>
    /// Canvas objects to be activated and deactivated dynamically.
    /// </summary>
    [Header("Canvas objects to be activated and deactivated dynamically")]
    public GameObject CanvasScreens;
    public GameObject PauseScreen;
    public GameObject ShotRegisteredScreen;
    public GameObject GameOverScreen;

    /// <summary>
    /// Local variables playerOneScore and playerTwoScore just to be able to keep track of separate scores.
    /// </summary>
    private int playerOneScore;
    private int playerTwoScore;

    /// <summary>
    /// Local variables p1Ready and p2Readyused for checking if both players are ready to continue.
    /// </summary>
    private bool p1Ready = false;
    private bool p2Ready = false;

    /// <summary>
    /// Property _currentState for keeping track of the state of the system.
    /// </summary>
    private State _currentState;
    public State CurrentState { get => _currentState; private set => _currentState = value; }

    /// <summary>
    /// Single available AudioManager instance got from _app object.
    /// </summary>
    [HideInInspector]
    public AudioManager audioManager;

    /// <summary>
    /// Single available GameSettings instance got from _app object.
    /// </summary>
    [HideInInspector]
    public GameSettings gameSettings;

    [Header("Winne Textbox")]
    public GameObject winnerText;
    
    /// <summary>
    /// Build in Start method for instatiating instances of singleton scripts, like GameSettings and AudioManager.
    /// Also used to start background music theme clip.
    /// </summary>
    private void Start()
    {
        audioManager = GameObject.Find("_app").GetComponent<AudioManager>();
        gameSettings = GameObject.Find("_app").GetComponent<GameSettings>();

        audioManager.Play("Theme");

        CurrentState = State.Playing;

        playerOneScore = 0;
        playerTwoScore = 0;
    }

    /// <summary>
    /// Build in Update method for checking for key presses during a game.
    /// Used for checking for pause while playing as well as checking to continue while paused, can load main menu,
    /// reset scores if the game is finished, and of course, resume when both players confirm ready state.
    /// </summary>
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (CurrentState == State.Playing)
            {
                Pause(true);
            }
            else
            {
                audioManager.Stop("Theme");
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
            }
        }

        if (ScriptPlayerOne.ReadyToContinue && ScriptPlayerTwo.ReadyToContinue)
        {
            if (CurrentState == State.Finished)
            {
                ResetScores();
            }

            Resume();
        }
    }

    /// <summary>
    /// Method called when the collision happens between bullet and a player. Updates score values on GUI as well as in Manager 
    /// itself, Logs whitch player was hit, plays audio and changes to teporary pause screen or game-finished screen waiting 
    /// for players to check in ready state in order to resume current or start new game.
    /// </summary>
    /// <param name="isPlayerOne"> Bool parameter telling if Player 1 was shot.</param>
    public void PlayerShot(bool isPlayerOne)
    {
        audioManager.Play("PlayerHit");

        if (isPlayerOne)
        {
            Debug.Log("Player 1 was shot..");
            playerTwoScore++;
            playerTwoText.GetComponent<TextMeshProUGUI>().text = playerTwoScore.ToString();
        }
        else
        {
            Debug.Log("Player 2 was shot..");
            playerOneScore++;
            playerOneText.GetComponent<TextMeshProUGUI>().text = playerOneScore.ToString();
        }

        if (playerOneScore >= gameSettings.FirstTo || playerTwoScore >= gameSettings.FirstTo)
        {
            GameOver(isPlayerOne);
        }
        else
        {
            Pause(false);
        }
    }

    /// <summary>
    /// Method used for changing games state to paused and showing needed pause screen. Invokes a FreezeTime method with delay 
    /// in order to give blood splatt animation time to play out completely. 
    /// Logs the reason for
    /// </summary>
    /// <param name="pausedThroughEsc">Parametar telling wether game has been paused on demand or because player got hit.</param>
    private void Pause(bool pausedThroughEsc)
    { 
        CurrentState = State.Paused;

        FreezeAllBullets();

        if (pausedThroughEsc)
        {
            FreezeTime();
        }
        else
        {
            Invoke("FreezeTime", 0.5f);
        }

        ScriptChangeIconOne.UpdateReadyState(false);
        ScriptChangeIconTwo.UpdateReadyState(false);

        CanvasScreens.SetActive(true);
    
        if (pausedThroughEsc)
        {
            Debug.Log("Escape button pressed! Pausing the game..");
            PauseScreen.SetActive(true);
        }
        else
        {
            Debug.Log("Player has been shot! Pausing the game..");
            ShotRegisteredScreen.SetActive(true);
        }
}

    /// <summary>
    /// Sets timeScale to 0 using built in propery Time.timeScale.
    /// </summary>
    private void FreezeTime()
    {
        Time.timeScale = 0;
    }
    
    /// <summary>
    /// Sets state to finished, and stops time immediately. Logs end of game and activates needed GUI screens.
    /// </summary>
    private void GameOver(bool winnerP1)
    {
        CurrentState = State.Finished;

        Time.timeScale = 0;
        string finalText = "Winner: " + ((winnerP1) ? "Player 1" : "Player 2");
        winnerText.GetComponent<TextMeshProUGUI>().text = finalText;

        ScriptChangeIconOne.UpdateReadyState(false);
        ScriptChangeIconTwo.UpdateReadyState(false);

        Debug.Log("Game finished..");

        CanvasScreens.SetActive(true);

        GameOverScreen.SetActive(true);


}

    /// <summary>
    /// Changes state back to playing and sets time scale back to default. Hides all the unnecesary screens. Resets readyToContinue
    /// flags inside playersBehaviour scripts.
    /// </summary>
    private void Resume()
    {
        Debug.Log("Both players ready!");

        Reset();

        CurrentState = State.Playing;
        Time.timeScale = 1;
        
        PauseScreen.SetActive(false);
        ShotRegisteredScreen.SetActive(false);
        GameOverScreen.SetActive(false);
        CanvasScreens.SetActive(false);

        ScriptPlayerOne.ReadyToContinue = false;
        ScriptPlayerTwo.ReadyToContinue = false;
    }
    
    /// <summary>
    /// Method called to reset scores on GUI text fields as well as variables in this script.
    /// </summary>
    private void ResetScores()
    {
        playerOneScore = 0;
        playerTwoScore = 0;

        playerOneText.GetComponent<TextMeshProUGUI>().text = playerOneScore.ToString();
        playerTwoText.GetComponent<TextMeshProUGUI>().text = playerTwoScore.ToString();
    }
    
    /// <summary>
    /// Resets scene.
    /// </summary>
    private void Reset()
    {
        DestroyAllBullets();
        ScriptPlayerOne.Reset();
        ScriptPlayerTwo.Reset();
    }

    /// <summary>
    /// Method that gets all GameObjects with tag "Bullet" and calls their DestroyBullet() method.
    /// </summary>
    private void DestroyAllBullets()
    {
        GameObject[] bulletsOnScene = GameObject.FindGameObjectsWithTag("Bullet");

        foreach (var bullet in bulletsOnScene)
        {
            bullet.GetComponent<Bullet>().DestroyBullet();
        }
    }

    /// <summary>
    /// Method for freezing the bullets in air applying force impuls in dirrection opposite of theirs.
    /// </summary>
    private void FreezeAllBullets()
    {
        GameObject[] bulletsOnScene = GameObject.FindGameObjectsWithTag("Bullet");

        foreach (var bullet in bulletsOnScene)
        {
            bullet.GetComponent<Bullet>().FreezeBullet();
        }
    }

    /// <summary>
    /// Method used for updating ready states of players upon registering nedded keys in update method.
    /// </summary>
    /// <param name="isForPlayerOne"> Parameter for distinguishing whitch player changed ready state</param>
    /// <param name="isReady"> Parameter saying to whitch state did the player newly set it</param>
    public void ChangeReadyState(bool isForPlayerOne, bool isReady)
    {
        Debug.Log("Player " + ((isForPlayerOne) ? 1 : 2) + " is " + ((isReady) ? "" : "NOT") + " Ready");
        if (isForPlayerOne)
        {
            ScriptChangeIconOne.UpdateReadyState(isReady);
        }
        else
        {
            ScriptChangeIconTwo.UpdateReadyState(isReady);
        }
    }
}
