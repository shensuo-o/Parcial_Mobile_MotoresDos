using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Managers.Menu;
public class ItemUI : MonoBehaviour
{
    [SerializeField] ItemDto itemToRepresent;
    [SerializeField] Image iconImg;
    [SerializeField] TextMeshProUGUI priceText;
    [SerializeField] GameObject buyButton;
    [SerializeField] bool StartBought;
    // Start is called before the first frame update
    public void SetItem(ItemDto _item)
    {
        itemToRepresent = _item;
        iconImg.sprite = itemToRepresent.itemIcon;
        priceText.text = itemToRepresent.cost.ToString();
        if (itemToRepresent.StartBought)
        {
            PlayerPrefs.SetInt(itemToRepresent.name, 1);
            PlayerPrefs.Save();
        }
        if (PlayerPrefs.GetInt(itemToRepresent.name)>0 && !itemToRepresent.IsLimitLes)
        {
            ByeButton();
        }
    }
    public void ByeButton()
    {
        if(!itemToRepresent.IsLimitLes && PlayerPrefs.GetInt(itemToRepresent.name)>0)
        buyButton.SetActive(false);
    }
    public void OnClickItem()
    {
        var money = PlayerPrefs.GetInt("saveMoneyShop");
        //OnItemClick?.Invoke(itemToRepresent);
        if (money >= itemToRepresent.cost)
        {
            if(!itemToRepresent.IsLife|| !MenuManager.instance.HasFullLife())
            {
                PlayerPrefs.SetInt("saveMoneyShop", money - itemToRepresent.cost);
            }
            else
            {
                return;
            }
            if (itemToRepresent.IsLife)
                MenuManager.instance.AddLife();
            MenuManager.instance.UpdateUI("money");
            BuyItem();
        }
    }
    void BuyItem()
    {
        PlayerPrefs.SetInt(itemToRepresent.name, PlayerPrefs.GetInt(itemToRepresent.name)+ 1);
        PlayerPrefs.Save();

        ByeButton();
    }

}
