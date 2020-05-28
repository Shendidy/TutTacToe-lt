using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AboutToggle : MonoBehaviour
{
    public GameObject aboutPanel;

    public void OpenAboutPanel()
    {
        if (aboutPanel != null) aboutPanel.SetActive(!aboutPanel.activeSelf);
    }
}
