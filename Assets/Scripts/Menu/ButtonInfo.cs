using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonInfo : MonoBehaviour
{
    public int ButtonTag;
    public int ButtonNumber;
    
    public void setPP()
    {
        PlayerPrefs.SetInt("UsedButton" + ButtonNumber, 1);
    }

    public void ByeButton()
    {
        if (MenuManager.instance.savedMoney >= 15)
        {
            Destroy(this.gameObject, 1);
        }
    }
}
