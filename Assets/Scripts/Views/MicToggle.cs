using UnityEngine;

public class MicToggle : MonoBehaviour
{
    public GameObject micButton;
    public GameObject muteButton;

    public void micToggle()
    {
        if (micButton != null && muteButton != null)
        {
            micButton.SetActive(!micButton.activeSelf);
            muteButton.SetActive(!muteButton.activeSelf);
            AudioListener.pause = !AudioListener.pause;
        }
    }
}
