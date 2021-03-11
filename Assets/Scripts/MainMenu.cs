using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

/// <summary>
/// Script made to controll button presses on Main Menu (Loading game scene and playing sound clips).
/// </summary>
public class MainMenu : MonoBehaviour
{
    /// <summary>
    /// AudioManager refference for playing theme sound as well as button click sound clips.
    /// </summary>
    public AudioManager audioManager;
    
    /// <summary>
    /// initialising audioManager refference and playing background theme song.
    /// </summary>
    private void Start()
    {
        audioManager = GameObject.Find("_app").GetComponent<AudioManager>();

        audioManager.Play("MainMenuTheme");
    }

    /// <summary>
    /// Method that Loads game scene.
    /// </summary>
    public void PlayGame()
    {
        audioManager.Play("Button");
        audioManager.Stop("MainMenuTheme");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    /// <summary>
    /// Method that quits the game.
    /// </summary>
    public void QuitGame()
    {
        audioManager.Play("Button");
        Debug.Log("Exiting game..");
        Application.Quit();
    }
}
