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
            if (itemToRepresent.IsClient)
            {
                PlayerPrefs.SetInt("ClientAbailable" + itemToRepresent.clientTag, 1);
            }
            else
            PlayerPrefs.SetInt(itemToRepresent.name, 1);
            PlayerPrefs.Save();

            ByeButton();

        }
        else if (PlayerPrefs.GetInt(itemToRepresent.name)>0 && !itemToRepresent.IsLimitLes)
        {
            ByeButton();
        }
        else
        {
            buyButton.SetActive(true);

        }
    }
    public void ByeButton()
    {
        if (itemToRepresent.IsClient && PlayerPrefs.GetInt("ClientAbailable" + itemToRepresent.clientTag)>0)
        {
            buyButton.SetActive(false);

        }
        else if (!itemToRepresent.IsLimitLes && PlayerPrefs.GetInt(itemToRepresent.name)>0)
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
        if (itemToRepresent.IsClient)
        {
           PlayerPrefs.SetInt("ClientAbailable" + itemToRepresent.clientTag, 1);

        }
        else
        {
            PlayerPrefs.SetInt(itemToRepresent.name, PlayerPrefs.GetInt(itemToRepresent.name) + 1);

        }
        PlayerPrefs.Save();
        ByeButton();

    }

}
