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
            LevelManager.instance.LoadScene();
        }

        public void ExitApp()
        {
            Application.Quit();
        }
    }
}
