using Managers;
using Managers.Level;
using TMPro;
using UnityEngine;

namespace Menu
{
    public class PauseMenu : MonoBehaviour
    {
        public GameObject pausePanel;
        public GameObject endPanel;
        public TextMeshProUGUI endScore;
        public TextMeshProUGUI endScore2;
        public bool gamePaused;

        void Start()
        {
            gamePaused = false;
            endPanel.SetActive(false);
            pausePanel.SetActive(false);
        }

        public void EndPanel()
        {
            Time.timeScale = 0;
            endPanel.SetActive(true);
            GameManager.instance.GainGachas();
            endScore.text = "You got " + GameManager.instance.money + " coins!";
            endScore2.text = "You got " + GameManager.instance.money/15 + " gachaPlates";
        }

        public void PauseGame()
        {
            GameManager.instance.isAlive = false;
            GameManager.instance.canSpawn = false;
            pausePanel.SetActive(!gamePaused);
            gamePaused = !gamePaused;
        }
        public void LoadMenu()
        {
            Time.timeScale = 1;
            GameManager.instance.SaveScore();
            GameManager.instance.GainGachas();
            print("me voy al menu");

            LevelManager.instance.LoadScene();
        }

        public void ExitApp()
        {
            //NotificationsController.SendNewRepeatedNotification("Hora de cocinar", "Un restaurante no se atiende solo, es hora de cocinar", 120, 120, 1);
            //if (PlayerPrefs.GetInt("GatchasPending") > 0)
            //    NotificationsController.SendNewRepeatedNotification2("Hay nuevas recetas", "Tenes " + PlayerPrefs.GetInt("GatchasPending").ToString() + " recetas para descubrir", 60, 180, 2);
            //var lives = PlayerPrefs.GetInt("saveLives");
            //if (lives < 5)
            //{
            //    NotificationsController.SendNewNotification("La cocina espera", "Ya has descansado demasiado, es hora de que el cafe abra sus puertas", (5 - lives) * 3*60, 5);

            //}
            Application.Quit();
        }
    }
}
