using UnityEngine;
using UnityEngine.Rendering.PostProcessing;


public class BackToGame : MonoBehaviour
{
    public GameObject gameItemsPanel;
    public GameObject aboutPanel;
    public GameObject getKeysPanel;
    public GameObject noKeysPanel;
    public GameObject fireworksPanel;

    public PostProcessVolume m_Volume;

    public void BackToGamePanel()
    {
        AdMob.instance.RequestBanner();

        if (gameItemsPanel != null) gameItemsPanel.SetActive(true);
        if (aboutPanel != null) aboutPanel.SetActive(false);
        if (getKeysPanel != null) getKeysPanel.SetActive(false);
        if (noKeysPanel != null) noKeysPanel.SetActive(false);
        if (fireworksPanel != null) fireworksPanel.SetActive(false);

        m_Volume.isGlobal = false;

        AdMob.instance.ShowBannerAd();
    }
}






