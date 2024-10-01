using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System;
using System.Net;

public class HudManager : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public float timer;
    public TextMeshProUGUI moneyText;
    public int currentMoney = 0;

    void Update()
    {
        currentMoney = GameManager.instance.money;
        timer += Time.deltaTime;

        moneyText.text = "Money collected: " + currentMoney.ToString();
        timerText.text = "Time : " + TimeSpan.FromSeconds(timer).ToString("mm\\:ss");
    }

    
}
