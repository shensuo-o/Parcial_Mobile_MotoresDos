using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Item", menuName = "Custom/New Item", order = 0)]

public class ItemDto : ScriptableObject
{
    public string itemName;
    public Sprite itemIcon;
    public int cost;
    public bool IsLimitLes;
    public int Qty;
    public ItemRarity rarity;

}
