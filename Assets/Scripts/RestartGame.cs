using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartGame : MonoBehaviour
{
    public GameObject keyErrorPanel;
    public GameObject gameItemsPanel;

    public void Restart()
    {
        SceneManager.LoadScene("Scene3x3Game");
    }
}
