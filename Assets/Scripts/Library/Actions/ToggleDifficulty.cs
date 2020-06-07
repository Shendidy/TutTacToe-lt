using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleDifficulty : MonoBehaviour
{
    public GameObject easyButton;
    public GameObject hardButton;

    public void DifficultyToggle()
    {
        if (easyButton != null && hardButton != null)
        {
            easyButton.SetActive(!easyButton.activeSelf);
            hardButton.SetActive(!hardButton.activeSelf);

            GameManager.difficulty =
                GameManager.difficulty == 1 ? 2 : 1;
        }
    }
}
