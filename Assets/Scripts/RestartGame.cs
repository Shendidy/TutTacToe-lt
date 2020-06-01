using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartGame : MonoBehaviour
{
    public GameObject keyErrorPanel;
    public GameObject gameItemsPanel;

    public void Restart()
    {
        int keysTotal = GameDataManager.LoadGameData()._keys;

        GameManager.interstitialAdCounter++;
        if (GameManager.interstitialAdCounter % 3 == 0) AdMob.instance.ShowInterstitialAd();

        if (GameManager.boardWidth == 3 && keysTotal >= 1)
        {
            if (GameManager.playersMoved3x3[0] || GameManager.playersMoved3x3[1] || GameManager.playersMoved3x3[2])
                GameDataManager.SaveGameData(new GameData(keysTotal -= 1, DateTime.UtcNow));
            SceneManager.LoadScene("Scene3x3Game");
        }
        else if(GameManager.boardWidth == 4 && keysTotal >= 3)
        {
            if (GameManager.playersMoved3x3[0] || GameManager.playersMoved3x3[1] || GameManager.playersMoved3x3[2])
                GameDataManager.SaveGameData(new GameData(keysTotal -= 3, DateTime.UtcNow));
            SceneManager.LoadScene("Scene4x4Game");
        }
        else if(GameManager.boardWidth == 5 && keysTotal >= 5)
        {
            if (GameManager.playersMoved3x3[0] || GameManager.playersMoved3x3[1] || GameManager.playersMoved3x3[2])
                GameDataManager.SaveGameData(new GameData(keysTotal -= 5, DateTime.UtcNow));
            SceneManager.LoadScene("Scene5x5Game");
        }
        else
        {
            GameDataManager.SaveGameData(new GameData(-1, DateTime.UtcNow));
            if (keyErrorPanel != null) keyErrorPanel.SetActive(!keyErrorPanel.activeSelf);
            if (gameItemsPanel != null) gameItemsPanel.SetActive(!gameItemsPanel.activeSelf);
        }
    }
}
