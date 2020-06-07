using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleDifficulty : MonoBehaviour
{
    public GameObject easyButton;
    public GameObject midButton;
    public GameObject hardButton;

    public void Awake()
    {
        if (easyButton == null)
        {
            easyButton = GameObject.FindGameObjectsWithTag("keyEasy")[0];
            midButton = GameObject.FindGameObjectsWithTag("keyMid")[0];
            hardButton = GameObject.FindGameObjectsWithTag("keyHard")[0];
        }

        switch (GameManager.difficulty)
        {
            case 1:
                SetDifficultyEasy();
                break;
            case 2:
                SetDifficultyMid();
                break;
            case 3:
                SetDifficultyHard();
                break;
        }
    }

    public void SetDifficultyEasy()
    {
        GameManager.difficulty = 1;
        
        easyButton.GetComponent<Image>().color = new Color(0f, 1f, 0f, 1f);
        midButton.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.6f);
        hardButton.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.6f);
    }

    public void SetDifficultyMid()
    {
        GameManager.difficulty = 2;

        easyButton.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.6f);
        midButton.GetComponent<Image>().color = new Color(0f, 1f, 0f, 1f);
        hardButton.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.6f);
    }

    public void SetDifficultyHard()
    {
        GameManager.difficulty = 3;

        easyButton.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.6f);
        midButton.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.6f);
        hardButton.GetComponent<Image>().color = new Color(0f, 1f, 0f, 1f);
    }
}
