using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pausePanel;
    public bool gamePaused;
    // Start is called before the first frame update
    void Start()
    {
        gamePaused = false;    
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
        GameManager.instance.SaveScore();
        SceneManager.LoadScene("Menu");
    }

    public void ExitApp()
    {
        Application.Quit();
    }
}
