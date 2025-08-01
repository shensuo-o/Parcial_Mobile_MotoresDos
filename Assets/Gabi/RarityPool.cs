using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]

public class RarityPool : MonoBehaviour
{

    public ItemDto[] myItems;
    public ItemRarity rarity;
    public float appearChance;
}
public enum ItemRarity
{
    Normal = 0,
    Rare = 1,
    SuperRare = 2,
    UltraMegaHiperRare = 3
}