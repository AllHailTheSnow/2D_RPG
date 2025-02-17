using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupSpawner : MonoBehaviour
{
    [SerializeField] private GameObject coinPrefab, healthPickupPrefab, staminaPickupPrefab;

    public void DropItems()
    {
        //Set a random number between 0 and 5
        int randomNum = Random.Range(0, 5);

        //If the random number is 1, instantiate a health pickup
        if (randomNum == 1)
        {
            Instantiate(healthPickupPrefab, transform.position, Quaternion.identity);
        }

        //If the random number is 2, instantiate a stamina pickup
        if (randomNum == 2)
        {
            Instantiate(staminaPickupPrefab, transform.position, Quaternion.identity);
        }

        //If the random number is 3, instantiate a random amount of coins
        if (randomNum == 3)
        {
            //Set a random number between 1 and 4
            int randomGoldAmount = Random.Range(1, 4);

            //Instantiate the coins based on the random number generated
            for (int i = 0; i < randomGoldAmount; i++)
            {
                Instantiate(coinPrefab, transform.position, Quaternion.identity);
            }
        }

    }
}
