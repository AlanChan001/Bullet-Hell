using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpSpawner : MonoBehaviour
{
    [SerializeField] private GameObject goldCoin, healthGlobe, staminaGlobe;

    public void DropItems() 
    {
        int randomNum = Random.Range(1, 4);

        if (randomNum == 1)
        {
            if (!staminaGlobe) { return; }
            Instantiate(staminaGlobe, transform.position, Quaternion.identity);
        }
        if (randomNum == 2)
        {
            if (!healthGlobe) { return; }

            Instantiate(healthGlobe, transform.position, Quaternion.identity);
        }
        if (randomNum == 3)
        {
            if (!goldCoin) { return; }

            int randomAmountOfGold = Random.Range(1, 4);
            for (int i = 0; i < randomAmountOfGold; i++)
            { 
                Instantiate(goldCoin, transform.position, Quaternion.identity);
            }
        }

    }
}
