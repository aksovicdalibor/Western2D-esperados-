using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DDOL : MonoBehaviour
{
    // Thanks to Fattie from StackOverFlow (stackoverflow.com/users/294884/fattie) and post
    public void Awake()
    {
        DontDestroyOnLoad(gameObject);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
