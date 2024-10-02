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

    // Update is called once per frame
    void Update()
    {
        
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
        SceneManager.LoadScene("Menu", LoadSceneMode.Additive);
    }

    public void ExitApp()
    {
        Application.Quit();
    }
}
