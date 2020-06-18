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
    public GameObject difficultyPanel;
    public GameObject gameItemsPanel;

    public void Awake()
    {
        if (easyButton == null)
        {
            easyButton = GameObject.FindGameObjectsWithTag("keyEasy")[0];
            midButton = GameObject.FindGameObjectsWithTag("keyMid")[0];
            hardButton = GameObject.FindGameObjectsWithTag("keyHard")[0];
            difficultyPanel = GameObject.FindGameObjectsWithTag("difficultyPanelTag")[0];
            gameItemsPanel = GameObject.FindGameObjectsWithTag("gameItemsPanelTag")[0];
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
        if (GameManager.newGame)
        {
            GameManager.difficulty = 1;

            easyButton.GetComponent<Image>().color = new Color(0f, 1f, 0f, 1f);
            midButton.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.6f);
            hardButton.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.6f);
        }
        else
        {
            if (difficultyPanel != null) difficultyPanel.SetActive(!difficultyPanel.activeSelf);
            if (gameItemsPanel != null) gameItemsPanel.SetActive(!gameItemsPanel.activeSelf);
        }
    }

    public void SetDifficultyMid()
    {
        if (GameManager.newGame)
        {
            GameManager.difficulty = 2;

            easyButton.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.6f);
            midButton.GetComponent<Image>().color = new Color(0f, 1f, 0f, 1f);
            hardButton.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.6f);
        }
        else
        {
            if (difficultyPanel != null) difficultyPanel.SetActive(!difficultyPanel.activeSelf);
            if (gameItemsPanel != null) gameItemsPanel.SetActive(!gameItemsPanel.activeSelf);
        }
    }

    public void SetDifficultyHard()
    {
        if (GameManager.newGame)
        {
            GameManager.difficulty = 3;

            easyButton.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.6f);
            midButton.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.6f);
            hardButton.GetComponent<Image>().color = new Color(0f, 1f, 0f, 1f);
        }
        else
        {
            if (difficultyPanel != null) difficultyPanel.SetActive(!difficultyPanel.activeSelf);
            if (gameItemsPanel != null) gameItemsPanel.SetActive(!gameItemsPanel.activeSelf);
        }
    }
}
