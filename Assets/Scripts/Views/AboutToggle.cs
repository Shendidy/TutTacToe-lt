using UnityEngine;

public class AboutToggle : MonoBehaviour
{
    public GameObject aboutPanel;
    public GameObject gameItemsPanel;

    public void ToggleAboutPanel()
    {
        if (aboutPanel != null) aboutPanel.SetActive(!aboutPanel.activeSelf);
        if (gameItemsPanel != null) gameItemsPanel.SetActive(!gameItemsPanel.activeSelf);
    }
}