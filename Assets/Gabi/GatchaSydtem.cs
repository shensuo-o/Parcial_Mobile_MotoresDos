using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatchaSydtem : MonoBehaviour
{
    [SerializeField] RarityPool[] myPools;

    [SerializeField] int pullQuantityToPity = 20;
    [SerializeField] int pullQuantityToRare = 7;
    int GatchaOpened;

    float totalChance;

    private void Start()
    {
        totalChance = 0;
        for (int i = 0; i < myPools.Length; i++)
        {
            totalChance += myPools[i].appearChance;
            for (int j = 0; j < myPools[i].myItems.Length; j++)
            {
                myPools[i].myItems[j].rarity = myPools[i].rarity;
            }
        }
        GatchaOpened=PlayerPrefs.GetInt("GatchaOpened");
    }

    public void Pull(int quantity)
    {
        if (PlayerPrefs.GetInt("GatchasPending") <= 0)
            return;
        PlayerPrefs.SetInt("GatchasPending", PlayerPrefs.GetInt("GatchasPending") - 1);
        bool hasRare = false;
        bool forceRare=false;

        if (quantity >= pullQuantityToRare)
        {
            forceRare = true;
        }
        
        for (int i = 0; i < quantity; i++)
        {
            ItemDto item = null;
            if (forceRare && !hasRare && i == quantity - 1)
                item = GetRandomItem(true);
            else
                item = GetRandomItem();

            if (item.rarity >= ItemRarity.Rare)
            {
                hasRare = true;
            }

            Debug.Log("Me tocó " + item.itemName + " de rareza " + item.rarity);
        }
    }


    ItemDto GetRandomItem(bool forceRare = false)
    {
        
        RarityPool pool = null;

        float randomValue = Random.Range(0, totalChance);

        if (GatchaOpened >= pullQuantityToPity)
        {
            pool = myPools[myPools.Length - 1];
            PlayerPrefs.SetInt("GatchaOpened", 0);

        }
        else
        {
            var initIndex = 0;
            if (forceRare)
            {
                initIndex = 1;
                randomValue -= myPools[0].appearChance;
            }

            for (int i = initIndex; i < myPools.Length; i++)
            {
                randomValue -= myPools[i].appearChance;

                if (randomValue <= 0)
                {
                    pool = myPools[i];
                    break;
                }
            }
        }

        if (pool.rarity == ItemRarity.UltraMegaHiperRare)
        {
            GatchaOpened = 0;
        }

        int randomIndex = Random.Range(0, pool.myItems.Length);

        return pool.myItems[randomIndex];
    }
}
