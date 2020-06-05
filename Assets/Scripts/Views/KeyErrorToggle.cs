using UnityEngine;

public class KeyErrorToggle : MonoBehaviour
{
    public GameObject keyErrorPanel;
    public GameObject gameItemsPanel;

    public void ToggleKeyErrorPanel()
    {
        if (keyErrorPanel != null) keyErrorPanel.SetActive(!keyErrorPanel.activeSelf);
        if (gameItemsPanel != null) gameItemsPanel.SetActive(!gameItemsPanel.activeSelf);
    }
}
