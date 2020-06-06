using System;
using UnityEngine;
using UnityEngine.UI;

public class RewardedKeysAction : MonoBehaviour
{
    public GameObject collectKeysPanel;
    public GameObject gameItemsPanel;
    public Text keysCount;

    public void RewardPlayer()
    {
        int keysTotal = GameDataManager.LoadGameData()._keys;
        if (keysTotal < 0) keysTotal = 0;
        keysTotal += GameManager.rewardedKeys;
        GameDataManager.SaveGameData(new GameData(keysTotal, DateTime.UtcNow));

        keysCount.text = keysTotal.ToString();

        collectKeysPanel.SetActive(false);
        gameItemsPanel.SetActive(true);
    }
}
