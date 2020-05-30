using UnityEngine;

public class KeyErrorToggle : MonoBehaviour
{
    public static GameObject keyErrorPanel;

    public static void ToggleKeyErrorPanel()
    {
        if (keyErrorPanel != null) keyErrorPanel.SetActive(!keyErrorPanel.activeSelf);
    }
}
