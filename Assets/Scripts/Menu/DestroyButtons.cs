using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DestroyButtons : MonoBehaviour
{
    public GameObject[] Buttons;

    void Start()
    {
        for (int i = 0; i < Buttons.Length; i++)
        {
            Buttons[i].GetComponent<ButtonInfo>().ButtonTag = PlayerPrefs.GetInt("UsedButton" + Buttons[i].GetComponent<ButtonInfo>().ButtonNumber);

            if (Buttons[i].GetComponent<ButtonInfo>().ButtonTag == 1)
            {
                Buttons[i].SetActive(false);
            }
            else if (Buttons[i].GetComponent<ButtonInfo>().ButtonTag == 0)
            {
                Buttons[i].SetActive(true);
            }
        }
    }
}
