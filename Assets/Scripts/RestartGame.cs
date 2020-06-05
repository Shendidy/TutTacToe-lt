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

        if (GameManager.boardWidth == 3 && keysTotal >= Constants._KeysPerBoard3x3)
        {
            if (GameManager.playersMoved3x3[0] || GameManager.playersMoved3x3[1] || GameManager.playersMoved3x3[2])
                GameDataManager.SaveGameData(new GameData(keysTotal -= Constants._KeysPerBoard3x3, DateTime.UtcNow));
            SceneManager.LoadScene("Scene3x3Game");
        }
        else if(GameManager.boardWidth == 4 && keysTotal >= Constants._KeysPerBoard4x4)
        {
            if (GameManager.playersMoved3x3[0] || GameManager.playersMoved3x3[1] || GameManager.playersMoved3x3[2] || GameManager.playersMoved3x3[3])
                GameDataManager.SaveGameData(new GameData(keysTotal -= Constants._KeysPerBoard4x4, DateTime.UtcNow));
            SceneManager.LoadScene("Scene4x4Game");
        }
        else if(GameManager.boardWidth == 5 && keysTotal >= Constants._KeysPerBoard5x5)
        {
            if (GameManager.playersMoved3x3[0] || GameManager.playersMoved3x3[1] || GameManager.playersMoved3x3[2] || GameManager.playersMoved3x3[3] || GameManager.playersMoved3x3[4])
                GameDataManager.SaveGameData(new GameData(keysTotal -= Constants._KeysPerBoard5x5, DateTime.UtcNow));
            SceneManager.LoadScene("Scene5x5Game");
        }
        else
        {
            GameDataManager.SaveGameData(new GameData(-1, DateTime.UtcNow));
            if (keyErrorPanel != null && gameItemsPanel != null)
            {
                AdMob.instance.RequestRewardedAd();
                //Debug.Log("Requested Rewarded ad from reset board class...");

                keyErrorPanel.SetActive(!keyErrorPanel.activeSelf);
                gameItemsPanel.SetActive(!gameItemsPanel.activeSelf);
            }
        }
    }
}
