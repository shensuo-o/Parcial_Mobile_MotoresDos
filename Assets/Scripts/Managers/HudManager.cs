using System;
using TMPro;
using UnityEngine;

namespace Managers
{
    public class HudManager : MonoBehaviour
    {
        public TextMeshProUGUI timerText;
        public float timer;
        public TextMeshProUGUI moneyText;
        public int currentMoney;

        void Update()
        {
            currentMoney = GameManager.instance.money;
            timer += Time.deltaTime;

            moneyText.text = "Money \n" + currentMoney;
            timerText.text = "Time : " + TimeSpan.FromSeconds(timer).ToString("mm\\:ss");
        }
    }
}
