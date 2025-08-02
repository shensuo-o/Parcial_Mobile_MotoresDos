using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatchaSydtem : MonoBehaviour
{
    [SerializeField] RarityPool[] myPools;

    [SerializeField] int pullQuantityToPity = 20;
    [SerializeField] int pullQuantityToRare = 7;
    int GatchaOpened;
    int GatchaOpenedSinceRare;
    float totalChance;

    private void Start()
    {
        totalChance = 0;
        for (int i = 0; i < myPools.Length; i++)
        {
            totalChance += myPools[i].appearChance;
            //for (int j = 0; j < myPools[i].myItems.Length; j++)
            //{
            //    myPools[i].myItems[j].rarity = myPools[i].rarity;
            //}
        }
        GatchaOpenedSinceRare = PlayerPrefs.GetInt("GatchaOpenedSinceRare");
        GatchaOpened = PlayerPrefs.GetInt("GatchaTotalOpened");
    }

    public void Pull(int quantity)
    {
        if (PlayerPrefs.GetInt("GatchasPending") < quantity)
            return;
        bool hasRare = false;
        bool forceRare=false;

        if (GatchaOpened + quantity >= pullQuantityToRare)
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
                //GatchaOpenedSinceRare = 0;
            }

            Debug.Log("Me tocó " + item.itemName + " de rareza " + item.rarity);
        }
        PlayerPrefs.SetInt("GatchasPending", PlayerPrefs.GetInt("GatchasPending") - quantity);
        PlayerPrefs.SetInt("GatchaOpenedSinceRare", GatchaOpenedSinceRare);
        PlayerPrefs.SetInt("GatchaTotalOpened", GatchaOpened);

    }


    ItemDto GetRandomItem(bool forceRare = false)
    {
        GatchaOpened++;
        RarityPool pool = null;

        float randomValue = Random.Range(0, totalChance);
               
            var initIndex = 0;
            if (forceRare)
            {
                initIndex = 1;
                GatchaOpened = 0;
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
        

        if (pool.rarity == ItemRarity.Normal)
        {
            GatchaOpenedSinceRare++;
            if (GatchaOpenedSinceRare >= pullQuantityToPity)
            {
                randomValue = Random.Range(0, totalChance);
                randomValue -= myPools[0].appearChance;
                for (int i = 1; i < myPools.Length; i++)
                {
                    randomValue -= myPools[i].appearChance;

                    if (randomValue <= 0)
                    {
                        pool = myPools[i];
                        break;
                    }
                }
                GatchaOpenedSinceRare = 0;
            }

        }

        int randomIndex = Random.Range(0, pool.myItems.Length);

        return pool.myItems[randomIndex];
    }
}
