using UnityEngine;
using TMPro;
using System;

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
