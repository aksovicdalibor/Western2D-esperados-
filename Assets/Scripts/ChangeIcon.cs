using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeIcon : MonoBehaviour
{
    public GameObject imgNotReady;
    public GameObject imgReady;
    

    public void UpdateReadyState(bool isReady)
    {
        if (isReady)
        {
            imgReady.SetActive(true);
            imgNotReady.SetActive(false);
        }
        else
        {
            imgReady.SetActive(false);
            imgNotReady.SetActive(true);
        }
    }
}
