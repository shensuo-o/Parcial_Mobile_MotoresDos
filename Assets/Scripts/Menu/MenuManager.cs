using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System;
using System.Net;

public class MenuManager : MonoBehaviour
{
    public static AudioClip ClickEffect;
    static AudioSource audioSrc;
    public GameObject mainPannel;
    public GameObject instructionsPannel;
    //public GameObject lvlsPannel;
    //public GameObject creditPannel;

    public TextMeshProUGUI timerText;
    public float timer;
    public float liveGetTime;
    public TextMeshProUGUI livesText;
    public int lives;
    public int maxLives;
    public bool timerIsRunning;

    public DateTime currentDay;
    public DateTime lastDay;
    public TimeSpan timePassed;

    private void Awake()
    {
        audioSrc = GetComponent<AudioSource>();
        instructionsPannel.SetActive(false);
        //lvlsPannel.SetActive(false);
        //creditPannel.SetActive(false);

        currentDay = DateTime.Now;

        if (PlayerPrefs.HasKey("save"))
        {
            string timeAsString = PlayerPrefs.GetString("save");
            lastDay = DateTime.Parse(timeAsString);

            timePassed = currentDay - lastDay;

            double minutesPassed = timePassed.TotalSeconds / 60;

            for (int i = 0; i < minutesPassed / 60; i++)
            {
                if (i == 3)
                {
                    lives++;
                    i = 0;
                    minutesPassed -= 3;
                    Debug.Log("Added 1 live");
                }
            }
        }

        Debug.Log("Time passed = " + timePassed.TotalSeconds);
        Debug.Log("Time now = " + currentDay);
        Debug.Log("Time Saved = " + lastDay);
    }

    private void Start()
    {
        timerIsRunning = true;
    }

    private void Update()
    {
        livesText.text = "Vidas: " + lives.ToString();
        timerText.text = TimeSpan.FromSeconds(timer).ToString("mm\\:ss");

        if (timerIsRunning)
        {
            timer += Time.deltaTime;

            if (timer >= liveGetTime * 60)
            {
                timer = 0;
                lives++;
            }
        }

        if (lives == maxLives)
        {
            timerIsRunning = false;
        }
        else if ( lives < maxLives)
        {
            timerIsRunning = true;
        }
    }

    private void OnApplicationPause()
    {
        SaveTime();
    }

    private void SaveTime()
    {
        PlayerPrefs.SetString("save", DateTime.Now.ToString());    
    }

    public void PlaySound(string name)
    {
        ClickEffect = Resources.Load<AudioClip>(name);
        audioSrc.PlayOneShot(ClickEffect);
    }

    public void LoadlLevel(int lvl)
    {
        if (lives >= 1)
        {
            lives--;
            SceneManager.LoadScene(lvl);
        }
    }

    public void PanelHowToPlay()
    {
        mainPannel.SetActive(false);
        //creditPannel.SetActive(false);
        //lvlsPannel.SetActive(false);
        instructionsPannel.SetActive(true);
    }

    //public void PanelLvlSelect()
    //{
    //    instructionsPannel.SetActive(false);
    //    //creditPannel.SetActive(false);
    //    mainPannel.SetActive(false);
    //    //lvlsPannel.SetActive(true);
    //}

    //public void PanelCredits()
    //{
    //    instructionsPannel.SetActive(false);
    //    mainPannel.SetActive(false);
    //    //lvlsPannel.SetActive(false);
    //    //creditPannel.SetActive(true);
    //}

    public void BackToMenu()
    {
        instructionsPannel.SetActive(false);
        //lvlsPannel.SetActive(false);
        //creditPannel.SetActive(false);
        mainPannel.SetActive(true);
    }

    public void ExitApp()
    {
        Application.Quit();
    }
}
