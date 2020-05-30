using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartGame : MonoBehaviour
{
    public void Restart()
    {
        int keysTotal = GameDataManager.LoadGameData()._keys;

        if (GameManager.boardWidth == 3 && keysTotal >= 1)
        {
            GameDataManager.SaveGameData(new GameData(keysTotal -= 1, DateTime.UtcNow));
            SceneManager.LoadScene("Scene3x3Game");
        }
        else if(GameManager.boardWidth == 4 && keysTotal >= 3)
        {
            GameDataManager.SaveGameData(new GameData(keysTotal -= 3, DateTime.UtcNow));
            SceneManager.LoadScene("Scene4x4Game");
        }
        else if(GameManager.boardWidth == 5 && keysTotal >= 5)
        {
            GameDataManager.SaveGameData(new GameData(keysTotal -= 5, DateTime.UtcNow));
            SceneManager.LoadScene("Scene5x5Game");
        }
        else
        {
            GameDataManager.SaveGameData(new GameData(-1, DateTime.UtcNow));
            KeyErrorToggle.ToggleKeyErrorPanel();
        }
    }
}
