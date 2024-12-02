using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System;
using System.Net;
using UnityEditor;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public static AudioClip ClickEffect;
    public static MenuManager instance;
    static AudioSource audioSrc;
    public GameObject mainPannel;
    public GameObject instructionsPannel;
    public GameObject shopPannel;
    public GameObject settingsPannel;

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

    public TextMeshProUGUI scoreText;
    public int savedScore = 0;

    public TextMeshProUGUI moneyText;
    public int savedMoney = 0;
    public int newMoney = 0;

    public GameObject failedBuy;
    public GameObject failedBuyLock;

    public GameObject[] BuyButtons;
    public GameObject[] EquipButtons;

    private void Awake()
    {
        audioSrc = GetComponent<AudioSource>();
        instructionsPannel.SetActive(false);
        settingsPannel.SetActive(false);
        shopPannel.SetActive(false);

        instance = this;

        currentDay = DateTime.Now;

        if (PlayerPrefs.HasKey("saveTime"))
        {
            string timeAsString = PlayerPrefs.GetString("saveTime");
            lastDay = DateTime.Parse(timeAsString);

            timePassed = currentDay - lastDay;

            double minutesPassed = timePassed.TotalSeconds / 60;

            for (int i = 0; i <= minutesPassed; i++)
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

        timer += (float)timePassed.TotalSeconds;
    }

    private void Start()
    {
        timerIsRunning = true;
        lives = PlayerPrefs.GetInt("saveLives");

        savedScore = PlayerPrefs.GetInt("saveScoreMenu");
        savedMoney = PlayerPrefs.GetInt("saveMoneyShop");
        newMoney = PlayerPrefs.GetInt("saveScoreGame");
        savedMoney += newMoney;
        moneyText.text = savedMoney.ToString();
    }

    private void Update()
    {
        livesText.text = "Vidas: " + lives.ToString() + "/" + maxLives;
        timerText.text = TimeSpan.FromSeconds(timer).ToString("mm\\:ss");

        if (lives > maxLives)
        {
            lives = maxLives;
        }

        if (timerIsRunning)
        {
            timer += Time.deltaTime;

            if (timer >= liveGetTime * 60)
            {
                timer = 0;
                lives++;
            }
        }

        if (lives >= maxLives)
        {
            timerIsRunning = false;
        }
        else if ( lives < maxLives)
        {
            timerIsRunning = true;
        }

        if (PlayerPrefs.GetInt("saveScoreGame") >= savedScore)
        {
            savedScore = PlayerPrefs.GetInt("saveScoreGame");
        }
        scoreText.text = savedScore.ToString();
    }

    private void OnApplicationPause()
    {
        SaveTime();
        SaveScore();
        SaveLives();
        PlayerPrefs.Save();
    }

    public void AddLife()
    {
        if(lives < maxLives)
        {
            lives++;
        }
        SaveLives();
    }

    private void SaveTime()
    {
        PlayerPrefs.SetString("saveTime", DateTime.Now.ToString());
    }

    private void SaveScore()
    {
        PlayerPrefs.SetInt("saveScoreMenu", savedScore);
        PlayerPrefs.SetInt("saveMoneyShop", savedMoney);
    }

    private void SaveLives()
    {
        PlayerPrefs.SetInt("saveLives", lives);
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
            PlaySound("SFX_UI_Confirm");
            lives--;
            SaveTime();
            SaveScore();
            SaveLives();
            PlayerPrefs.Save();
            SceneManager.LoadScene(lvl);
        }
        else
        {
            PlaySound("SFX_UI_Cancel");
        }
    }

    public void PanelHowToPlay()
    {
        mainPannel.SetActive(false);
        shopPannel.SetActive(false);
        settingsPannel.SetActive(false);
        instructionsPannel.SetActive(true);
    }
    
    public void PanelShop()
    {
        instructionsPannel.SetActive(false);
        mainPannel.SetActive(false);
        settingsPannel.SetActive(false);
        shopPannel.SetActive(true);

        for (int i = 0; i < BuyButtons.Length; i++)
        {
            if (PlayerPrefs.GetInt("Color" + i) == i)
            {
                BuyButtons[i].SetActive(false);
            }
        }

        for (int i = 0; i < EquipButtons.Length; i++)
        {
            if (PlayerPrefs.GetInt("Color" + i) == i)
            {
                EquipButtons[i].SetActive(true);
            }
        }
    }

    public void BackToMenu()
    {
        instructionsPannel.SetActive(false);
        shopPannel.SetActive(false);
        settingsPannel.SetActive(false);
        mainPannel.SetActive(true);
        moneyText.text = savedMoney.ToString();
    }

    public void Settings()
    {
        instructionsPannel.SetActive(false);
        shopPannel.SetActive(false);
        settingsPannel.SetActive(true);
    }


    public void ExitApp()
    {
        Application.Quit();
    }

    public bool HasFullLife()
    {
        return !(lives < maxLives);
    }

    public void BuyLife(int amount)
    {
        if (lives < 5 - amount)
        {
            if (savedMoney >= (amount * 4) - (amount - 1))
            {
                lives += amount;
                savedMoney -= (amount * 4) - (amount - 1);
                SaveScore();
                SaveLives();
                moneyText.text = savedMoney.ToString();
            }
        }
        else
        {
            Instantiate(failedBuy, failedBuyLock.transform);
        }
    }

    public void BuyColor(int code, int price, GameObject Button)
    {
        Button.SetActive(false);

        if (savedMoney >= price)
        {
            savedMoney -= price;
            SaveScore();
            moneyText.text = savedMoney.ToString();

            for (int i = 0; i < BuyButtons.Length; i++)
            {
                if (code == i)
                {
                    PlayerPrefs.SetInt("Color" + i, i);
                }
            }
        }
    }

    public void EquipColor(int colorCode)
    {
        PlayerPrefs.SetInt("BarColor", colorCode);
        PlayerPrefs.Save();
    }
}
