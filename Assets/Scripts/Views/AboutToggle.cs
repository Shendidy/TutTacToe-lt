using UnityEngine;

public class AboutToggle : MonoBehaviour
{
    public GameObject aboutPanel;

    public void ToggleAboutPanel()
    {
        if (aboutPanel != null) aboutPanel.SetActive(!aboutPanel.activeSelf);
    }
}