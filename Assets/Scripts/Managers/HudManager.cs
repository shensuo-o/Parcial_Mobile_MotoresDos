using System;
using TMPro;
using UnityEngine;

namespace Managers
{
    public class HudManager : MonoBehaviour
    {
        public TextMeshProUGUI timerText;
        public float timer;
        public float timeLimit;
        public bool isTimeLimitedMode;
        public TextMeshProUGUI moneyText;
        public int currentMoney;
        public TextMeshProUGUI livesText;

        void Update()
        {
            currentMoney = GameManager.instance.money;
            timer += Time.deltaTime;

            moneyText.text = "Money \n" + currentMoney;
            
            if (isTimeLimitedMode)
            {
                float remainingTime = Mathf.Max(0, timeLimit - timer);
                timerText.text = "Tiempo restante: " + TimeSpan.FromSeconds(remainingTime).ToString("mm\\:ss");
            }
            else
            {
                timerText.text = "Tiempo: " + TimeSpan.FromSeconds(timer).ToString("mm\\:ss");
            }
            
            // Mostrar vidas en modo infinito
            if (livesText && !GameManager.instance.isTimeLimitedMode)
            {
                livesText.text = "Vidas: " + GameManager.instance.lives;
            }
            else if (livesText)
            {
                livesText.gameObject.SetActive(false);
            }
        }
    }
}