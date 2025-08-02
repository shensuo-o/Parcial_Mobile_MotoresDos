using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

}
