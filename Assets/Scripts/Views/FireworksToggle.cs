using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class FireworksToggle : MonoBehaviour
{
    public static FireworksToggle instance;

    public GameObject aboutPanel;
    public GameObject gameItemsPanel;
    public GameObject getKeysPanel;
    public GameObject noKeysPanel;
    public GameObject fireworksPanel;

    public PostProcessVolume m_Volume;

    private void Start()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    public void ToggleFireworksPanel()
    {
        if (gameItemsPanel != null) gameItemsPanel.SetActive(false);
        if (aboutPanel != null) aboutPanel.SetActive(false);
        if (getKeysPanel != null) getKeysPanel.SetActive(false);
        if (noKeysPanel != null) noKeysPanel.SetActive(false);
        if (fireworksPanel != null) fireworksPanel.SetActive(true);

        m_Volume.isGlobal = true;
    }
}
