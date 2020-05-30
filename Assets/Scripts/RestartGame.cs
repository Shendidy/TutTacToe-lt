using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartGame : MonoBehaviour
{
    public void Restart()
    {
        switch (GameManager.boardWidth)
        {
            case 3:
                GameManager.keysTotal -= 1;
                SceneManager.LoadScene("Scene3x3Game");
                break;
            case 4:
                GameManager.keysTotal -= 3;
                SceneManager.LoadScene("Scene3x3Game"); // set 4x4 game scene
                break;
            case 5:
                GameManager.keysTotal -= 5;
                SceneManager.LoadScene("Scene3x3Game"); // set 5x5 game scene
                break;
        }
    }
}
