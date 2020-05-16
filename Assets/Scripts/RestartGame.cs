using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartGame : MonoBehaviour
{
    public void Restart()
    {
        switch (GameManager.boardWidth)
        {
            case 3:
                SceneManager.LoadScene("Scene3x3Game");
                break;
            case 4:
                SceneManager.LoadScene("Scene3x3Game");
                break;
            case 5:
                SceneManager.LoadScene("Scene3x3Game");
                break;
        }
    }
}
