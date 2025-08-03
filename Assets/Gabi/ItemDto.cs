using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Managers.Menu;

[CreateAssetMenu(fileName = "Item", menuName = "Custom/New Item", order = 0)]

public class ItemDto : ScriptableObject
{
    public string itemName;
    public Sprite itemIcon;
    public int cost;
    public bool IsLife;
    public bool IsClient;
    public int clientTag;
    public bool IsLimitLes;
    public bool StartBought;
    public int Qty;
    //public int Qty;
    public ItemRarity rarity;
    public bool CanBePurchased()
    {
        if (IsClient)
        {
            if (PlayerPrefs.GetInt("ClientAbailable" + clientTag) > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        else if (IsLife)
        {
            if (PlayerPrefs.GetInt("saveLives") >= 5)
            {
                return false;
            }
            else
            {
                return true;

            }

        }
        else if (PlayerPrefs.GetInt(name) > 0)
        {

            return false;
        }
        else return true;

    }
    public void AddItem()
    {
        if (IsClient)
        {
            PlayerPrefs.SetInt("ClientAbailable" + clientTag, 1);
            Qty++;
            PlayerPrefs.Save();
        }
        else if (IsLife)
        {
            MenuManager.instance.AddLife();
        }
        else
        {
            PlayerPrefs.SetInt(name, PlayerPrefs.GetInt(name) + 1);
            Qty++;
            PlayerPrefs.Save();

        }
    }
    public void ResetItem()
    {
        if (StartBought) return;
        if (IsClient)
        {
            PlayerPrefs.SetInt("ClientAbailable" + clientTag, 0);
            Qty=0;
            Debug.Log("ClientAbailable" + clientTag + " vaciado");

        }

        else
        {
            PlayerPrefs.SetInt(name, 0);
            Qty=0;

        }
    }
}
