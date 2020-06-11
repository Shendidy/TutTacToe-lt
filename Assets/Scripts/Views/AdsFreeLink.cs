using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdsFreeLink : MonoBehaviour
{
    public void OpenLink()
    {
        var storeLink = GameManager.mobileSystem == "ios" ? Constants._PlayStoreLink : Constants._AppStoreLink;
        Application.OpenURL(storeLink);
    }        
}
