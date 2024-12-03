using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        endScore.text = "You got " + GameManager.instance.money + " coins!";
    }

    public void PauseGame()
    {
        GameManager.instance.isAlive = false;
        GameManager.instance.canSpawn = false;
        pausePanel.SetActive(!gamePaused);
        gamePaused = !gamePaused;
    }
    public void LoadlMenu()
    {
        Time.timeScale = 1;
        GameManager.instance.SaveScore();
        SceneManager.LoadScene("Menu");
    }

    public void ExitApp()
    {
        Application.Quit();
    }
}
