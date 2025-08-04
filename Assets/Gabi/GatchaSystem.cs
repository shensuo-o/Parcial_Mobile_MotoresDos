using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Managers.Menu;

public class GatchaSystem : MonoBehaviour
{
    [SerializeField] RarityPool[] myPools;

    [SerializeField] int pullQuantityToPity = 20;
    [SerializeField] ShakeSystem shakeSystem;
    [SerializeField] int pullQuantityToRare = 7;
    int GatchaOpened;
    int GatchaOpenedSinceRare;
    float totalChance;
    [SerializeField] GachaView gachaView;
    public void SetUpGatcha()
    {
        gachaView.CloseChest();
        shakeSystem.ForceReset();

        if (MenuManager.instance.gachaPending <=0)
            return;
        totalChance = 0;
        //gachaView = GetComponent<GachaView>();
        for (int i = 0; i < myPools.Length; i++)
        {
            totalChance += myPools[i].appearChance;
            //for (int j = 0; j < myPools[i].myItems.Length; j++)
            //{
            //    myPools[i].myItems[j].rarity = myPools[i].rarity;
            //}
        }
        GatchaOpenedSinceRare = MenuManager.instance.gachaOpenedSinceRare;
        GatchaOpened = MenuManager.instance.gachaTotalOpened ;
        ShakeSystem.instance.OnSuccess += Pull;

    }

    public void Pull()
    {
        
        bool hasRare = false;
        bool forceRare=false;

        if (GatchaOpened + 1 >= pullQuantityToRare)
        {
            forceRare = true;

        }
        
        for (int i = 0; i < 1; i++)
        {
            ItemDto item = null;
            if (forceRare && !hasRare && i == 1 - 1)
                item = GetRandomItem(true);
            else
                item = GetRandomItem();

            if (item.rarity >= ItemRarity.Rare)
            {
                //GatchaOpenedSinceRare = 0;
            }

            Debug.Log("Me tocó " + item.itemName + " de rareza " + item.rarity);
            gachaView.gachaReward.sprite = item.itemIcon;

        }
        PlayerPrefs.SetInt("GatchasPending", MenuManager.instance.gachaPending - 1);
        MenuManager.instance.UpdateUI("gacha");
        PlayerPrefs.SetInt("GatchaOpenedSinceRare", GatchaOpenedSinceRare);
        PlayerPrefs.SetInt("GatchaTotalOpened", GatchaOpened);
        ShakeSystem.instance.OnSuccess -= Pull;
        MenuManager.instance.UpdateGacha();
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
        if (pool.myItems[randomIndex].CanBePurchased())
        {
            pool.myItems[randomIndex].AddItem();
            print(pool.myItems[randomIndex].name);
            ShopManager.instance.SetAllItems();
            return pool.myItems[randomIndex];
        }
        else
        {
            print("me hubiera tocado" + pool.myItems[randomIndex].name);
            PlayerPrefs.SetInt("saveMoneyShop", PlayerPrefs.GetInt("saveMoneyShop")+pool.CompensationGold);
            PlayerPrefs.Save();
            MenuManager.instance.UpdateUI("money");
            return pool.CompensationItem;

        }
    }
}
