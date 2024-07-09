using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EconomyManager : Singleton<EconomyManager>
{
    private TMP_Text goldText;
    private int currentGold;

    public void UpdateCurrentGold()
    {
        currentGold++;
        if (goldText == null)
        {
            goldText = GameObject.Find("Gold Amount Text").GetComponent<TMP_Text>();
        }

        goldText.text = currentGold.ToString("D3");
        
    }
}
