using UnityEngine;

public class KeyErrorToggle : MonoBehaviour
{
    public GameObject keyErrorPanel;

    public void ToggleKeyErrorPanel()
    {
        if (keyErrorPanel != null) keyErrorPanel.SetActive(!keyErrorPanel.activeSelf);
    }
}
