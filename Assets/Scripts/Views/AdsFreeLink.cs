using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdsFreeLink : MonoBehaviour
{
    public void OpenLink()
    {
        var storeLink = GameManager.mobileSystem == "ios" ? Constants._AppStoreLink : Constants._PlayStoreLink;
        Application.OpenURL(storeLink);
    }        
}
