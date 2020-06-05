using UnityEngine;

public class BackToGame : MonoBehaviour
{
    public GameObject gameItemsPanel;
    public GameObject aboutPanel;
    public GameObject getKeysPanel;
    public GameObject noKeysPanel;

    public void BackToGamePanel()
    {
        if (gameItemsPanel != null) gameItemsPanel.SetActive(true);
        if (aboutPanel != null) aboutPanel.SetActive(false);
        if (getKeysPanel != null) getKeysPanel.SetActive(false);
        if (noKeysPanel != null) noKeysPanel.SetActive(false);
    }
}