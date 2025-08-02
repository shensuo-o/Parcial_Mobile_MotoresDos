using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    [SerializeField] ItemDto[] myItems = new ItemDto[0];
    [SerializeField] ItemUI itemUIPrefab;
    [SerializeField] Transform shopParent;

    [SerializeField] int gold=100000000;
    private void Start()
    {
        SetAllItems();
    }
    public void SetAllItems()
    {
        for (int i = 0; i < myItems.Length; i++)
        {
            var newItem = Instantiate(itemUIPrefab, shopParent);
            //newItem.OnItemClick += OnSellItem;
            newItem.SetItem(myItems[i]);
            if (!PlayerPrefs.HasKey(myItems[i].name))
            {
                PlayerPrefs.SetInt(myItems[i].name,0);
            }
        }
    }

    //void OnSellItem(ItemDto item)
    //{
    //    if (gold >= item.cost)
    //    {
    //        Debug.Log("Compré el item " + item.itemName);
    //        gold -= item.cost;
    //    }
    //    else
    //    {
    //        Debug.Log("No tengo suficiente oro");
    //    }
    //}
}
