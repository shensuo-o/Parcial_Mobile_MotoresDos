using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]

public class RarityPool
{

    public ItemDto[] myItems;
    public ItemRarity rarity;
    public float appearChance;
    public ItemDto CompensationItem;
    public int CompensationGold;
}
public enum ItemRarity
{
    Normal = 0,
    Rare = 1,
    SuperRare = 2,
    UltraMegaHiperRare = 3
}