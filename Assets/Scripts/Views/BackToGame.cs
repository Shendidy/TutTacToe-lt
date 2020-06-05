using UnityEngine;
using UnityEngine.Rendering.PostProcessing; // useful??


public class BackToGame : MonoBehaviour
{
    public GameObject gameItemsPanel;
    public GameObject aboutPanel;
    public GameObject getKeysPanel;
    public GameObject noKeysPanel;
    public GameObject fireworksPanel;



    public PostProcessVolume m_Volume;
    Bloom m_Bloom;

    public void Start()
    {
        //m_Bloom = ScriptableObject.CreateInstance<Bloom>();
        //m_Bloom.enabled.Override(true);
        //m_Bloom.intensity.Override(0f);

        //m_Volume = PostProcessManager.instance.QuickVolume(gameObject.layer, 100f, m_Bloom);
    }

    public void BackToGamePanel()
    {
        if (gameItemsPanel != null) gameItemsPanel.SetActive(true);
        if (aboutPanel != null) aboutPanel.SetActive(false);
        if (getKeysPanel != null) getKeysPanel.SetActive(false);
        if (noKeysPanel != null) noKeysPanel.SetActive(false);
        if (fireworksPanel != null) fireworksPanel.SetActive(false);

        //m_Bloom = m_Volume.profile.settings[0];

        //if (m_Volume.TryGetComponent(out m_Bloom))
        //    m_Bloom.intensity.Override(0f);

        m_Volume.isGlobal = false;

    }
}






