using UnityEngine;

public class GetKeysToggle : MonoBehaviour
{
    public GameObject getKeysPanel;
    public GameObject gameItemsPanel;

    public void ToggleGetKeysPanel()
    {
        if (getKeysPanel != null) getKeysPanel.SetActive(!getKeysPanel.activeSelf);
        if (gameItemsPanel != null) gameItemsPanel.SetActive(!gameItemsPanel.activeSelf);
    }
}
