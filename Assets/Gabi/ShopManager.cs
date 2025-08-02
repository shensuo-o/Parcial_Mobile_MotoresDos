using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    [SerializeField] ItemDto[] myItems = new ItemDto[0];
    [SerializeField] ItemUI itemUIPrefab;
    [SerializeField] Transform shopParent;
    public ItemUI[] itemUIs;
    [SerializeField] int gold=100000000;
    private void Start()
    {
        itemUIs = new ItemUI[myItems.Length];
        SetAllItems();
    }
    public void SetAllItems()
    {
        if (itemUIs[0]==null)
        {
            for (int i = 0; i < myItems.Length; i++)
            {
                    var newItem = Instantiate(itemUIPrefab, shopParent);
                    itemUIs[i]= newItem;
                    newItem.SetItem(myItems[i]);
                    if (PlayerPrefs.HasKey(myItems[i].name))
                    {
                        newItem.ResetItem(myItems[i]);
                    }
                
            }
        }
        else
        {
            for (int i = 0; i < itemUIs.Length; i++)
            {
                itemUIs[i].SetItem(myItems[i]);
                if (PlayerPrefs.HasKey(myItems[i].name))
                {
                    itemUIs[i].ResetItem(myItems[i]);
                }
            }
        }

            //newItem.OnItemClick += OnSellItem;
            
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

