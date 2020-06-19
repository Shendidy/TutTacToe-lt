using UnityEngine;

public class DifficultyPanelToggle : MonoBehaviour
{
    public static DifficultyPanelToggle instance;
    public GameObject difficultyPanel;
    public GameObject gameItemsPanel;

    public void Start()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    public void ToggleDifficultyPanel()
    {
        if (difficultyPanel != null) difficultyPanel.SetActive(!difficultyPanel.activeSelf);
        if (gameItemsPanel != null) gameItemsPanel.SetActive(!gameItemsPanel.activeSelf);
    }
}