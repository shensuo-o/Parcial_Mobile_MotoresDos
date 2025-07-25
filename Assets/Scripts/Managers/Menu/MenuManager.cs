using System;
using System.Globalization;
using Managers.Level;
using TMPro;
using UnityEngine;

namespace Managers.Menu
{
    public class MenuManager : MonoBehaviour
    {
        public static MenuManager instance;
        private static AudioSource _audioSrc;

        // Panels
        [Header("Panels")]
        public GameObject mainPanel;
        public GameObject instructionsPanel;
        public GameObject shopPanel;
        public GameObject settingsPanel;

        // UI Elements
        [Header("UI Elements")]
        public TextMeshProUGUI timerText;
        public TextMeshProUGUI livesText;
        public TextMeshProUGUI scoreText;
        public TextMeshProUGUI moneyText;

        // Gameplay Variables
        [Header("Gameplay Variables")]
        public float timer;
        public float liveGetTime = 3f;
        public int lives;
        public int maxLives = 5;
        public int savedScore;
        public int savedMoney;
        private int _newMoney;
        private bool _timerIsRunning;

        // DateTime
        private DateTime _currentDay;
        private DateTime _lastDay;
        private TimeSpan _timePassed;

        // Shop
        [Header("Shop")]
        public GameObject failedBuy;
        public GameObject failedBuyLock;
        public GameObject[] buyButtons;
        public GameObject[] equipButtons;

        private void Awake()
        {
            instance = this;
            _audioSrc = GetComponent<AudioSource>();

            instructionsPanel.SetActive(false);
            settingsPanel.SetActive(false);
            shopPanel.SetActive(false);

            InitializeTimer();
        }

        private void Start()
        {
            LoadPlayerData();
            UpdateUI("all");
            PlayerPrefs.SetInt("ClientAvailable0", 1);
            PlayerPrefs.SetInt("UsedButton0", 1);
        }

        private void Update()
        {
            if (_timerIsRunning)
            {
                timer += Time.deltaTime;
                if (timer >= liveGetTime * 60)
                {
                    timer = 0;
                    AddLife();
                }
            }

            if (lives >= maxLives)
            {
                _timerIsRunning = false;
                timer = 0;
            }
            else
            {
                _timerIsRunning = true;

            }

            UpdateUI("timer");
        }

        private void LoadPlayerData()
        {
            // Load saved values or set defaults
            lives = PlayerPrefs.GetInt("saveLives", maxLives);
            savedScore = PlayerPrefs.GetInt("saveScoreMenu");  // Load high score
            savedMoney = PlayerPrefs.GetInt("saveMoneyShop");  // Load total money from the shop
            _newMoney = PlayerPrefs.GetInt("saveScoreGame");  // Load the last game's earned money
        
            // Add the new money from the last game session to the saved total money
            savedMoney += _newMoney;

            // Update high score only if newMoney is higher than savedScore
            if (_newMoney > savedScore)
            {
                savedScore = _newMoney;
                PlayerPrefs.SetInt("saveScoreMenu", savedScore);  // Save updated high score
            }

            // After processing newMoney, reset it
            PlayerPrefs.SetInt("saveScoreGame", 0);  // Reset to avoid carrying over

            // Save the total money
            PlayerPrefs.SetInt("saveMoneyShop", savedMoney);  // Save updated total money
            PlayerPrefs.Save();  // Ensure changes are written to the disk

            Debug.Log($"Loaded Data: Lives={lives}, High Score={savedScore}, Money={savedMoney}, newMoney={_newMoney}");
        }

        private void SavePlayerData()
        {
            SaveTime();
            PlayerPrefs.SetInt("saveLives", lives);
            PlayerPrefs.SetInt("saveScoreMenu", savedScore);
            PlayerPrefs.SetInt("saveMoneyShop", savedMoney);
            PlayerPrefs.Save();
            Debug.Log($"Player data saved: Lives={lives}, High Score={savedScore}, Money={savedMoney}");
        }

        private void UpdateUI(string element)
        {
            switch (element.ToLower())
            {
                case "lives":
                    livesText.text = $"Lives: {lives}/{maxLives}";
                    break;

                case "timer":
                    timerText.text = TimeSpan.FromSeconds(timer).ToString("mm\\:ss");
                    break;

                case "score":
                    scoreText.text = savedScore.ToString();
                    break;

                case "money":
                    moneyText.text = savedMoney.ToString();
                    break;

                case "all":
                    UpdateUI("lives");
                    UpdateUI("timer");
                    UpdateUI("score");
                    UpdateUI("money");
                    break;

                default:
                    Debug.LogWarning($"Unknown UI element: {element}");
                    break;
            }
        }

        private void InitializeTimer()
        {
            _currentDay = DateTime.Now;

            string saveTime = PlayerPrefs.GetString("saveTime", string.Empty);

            if (!string.IsNullOrEmpty(saveTime) && DateTime.TryParse(saveTime, out DateTime result))
            {
                _lastDay = result;
            }
            else
            {
                // Asignar un valor por defecto, por ejemplo la fecha actual o manejar el error apropiadamente
                _lastDay = DateTime.Now;
                // O registra/loguea el problema aquí si lo deseas
            }
            _timePassed = _currentDay - _lastDay;

            double minutesPassed = _timePassed.TotalMinutes;
            while (minutesPassed >= liveGetTime && lives < maxLives)
            {
                AddLife();
                minutesPassed -= liveGetTime;
            }

            timer = (float)(minutesPassed * 60);
        }

        private void SaveTime()
        {
            // Guardar la fecha en formato ISO 8601
            PlayerPrefs.SetString("saveTime", DateTime.Now.ToString("o")); // "o" para round-trip (ISO 8601)
            PlayerPrefs.Save();
        }

        public void AddLife()
        {
            if (lives < maxLives)
            {
                lives++;
                UpdateUI("lives");
            }
        }

        public void ResetPlayerData()
        {
            PlayerPrefs.SetInt("saveLives", maxLives);
            PlayerPrefs.SetInt("saveScoreMenu", 0);
            PlayerPrefs.SetInt("saveMoneyShop", 0);
            PlayerPrefs.SetInt("saveScoreGame", 0);
            PlayerPrefs.SetString("saveTime", DateTime.Now.ToString(CultureInfo.InvariantCulture));

            PlayerPrefs.SetInt("UsedButton0", 1);
            PlayerPrefs.SetInt("UsedButton1", 0);
            PlayerPrefs.SetInt("UsedButton2", 0);
            PlayerPrefs.SetInt("UsedButton3", 0);
            PlayerPrefs.SetInt("UsedButton4", 0);
            PlayerPrefs.SetInt("UsedButton5", 0);
            PlayerPrefs.SetInt("UsedButton6", 0);

            PlayerPrefs.SetInt("ClientAbailable0", 1);
            PlayerPrefs.SetInt("ClientAbailable1", 0);
            PlayerPrefs.SetInt("ClientAbailable2", 0);
            PlayerPrefs.SetInt("ClientAbailable3", 0);
            PlayerPrefs.SetInt("ClientAbailable4", 0);
            PlayerPrefs.SetInt("ClientAbailable5", 0);
            PlayerPrefs.SetInt("ClientAbailable6", 0);

            lives = maxLives;
            savedScore = 0;
            savedMoney = 0;
            _newMoney = 0;
            timer = 0;

            SavePlayerData();

            UpdateUI("all");

            Debug.Log("Player data has been reset to defaults!");
        }

        public bool HasFullLife()
        {
            return lives == maxLives;
        }

        public void EquipColor(int colorCode)
        {
            PlayerPrefs.SetInt("BarColor", colorCode);
            PlayerPrefs.Save();
        }

        public void BuyLife(int amount)
        {
            int cost = (amount * 4) - (amount - 1);
            if (lives + amount <= maxLives && savedMoney >= cost)
            {
                lives += amount;
                savedMoney -= cost;
                UpdateUI("lives");
                UpdateUI("money");
                SavePlayerData();
            }
            else
            {
                Instantiate(failedBuy, failedBuyLock.transform);
            }
        }

        public void BuyClient(int clientTag)
        {
            if (savedMoney >= 15)
            {
                PlayerPrefs.SetInt("ClientAvailable" + clientTag, 1);
                savedMoney -= 15;
                UpdateUI("money");
            }
        }

        public void PanelHowToPlay()
        {
            SetActivePanel(instructionsPanel);
        }

        public void PanelShop()
        {
            SetActivePanel(shopPanel);
            for (int i = 0; i < buyButtons.Length; i++)
            {
                bool isBought = PlayerPrefs.GetInt($"Color{i}") == i;
                buyButtons[i].SetActive(!isBought);
                equipButtons[i].SetActive(isBought);
            }
        }

        public void BackToMenu()
        {
            SetActivePanel(mainPanel);
            UpdateUI("money");
        }

        public void Settings()
        {
            SetActivePanel(settingsPanel);
        }

        private void SetActivePanel(GameObject panelToActivate)
        {
            mainPanel.SetActive(false);
            instructionsPanel.SetActive(false);
            shopPanel.SetActive(false);
            settingsPanel.SetActive(false);

            panelToActivate.SetActive(true);
        }

        public void BuyColor(int code, int price, GameObject button)
        {
            if (savedMoney >= price)
            {
                savedMoney -= price;
                PlayerPrefs.SetInt($"Color{code}", code);
                button.SetActive(false);
                UpdateUI("money");
                SavePlayerData();
            }
        }

        private void OnApplicationQuit()
        {
            SavePlayerData();
            Debug.Log("Application quitting, data saved.");
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus)
            {
                SavePlayerData();
                Debug.Log("Application paused, data saved.");
            }
        }

        public void ExitApp()
        {
            SavePlayerData();
            Application.Quit();
        }

        public void LoadLevel(string levelName)
        {
            if (lives > 0)
            {
                PlaySound("SFX_UI_Confirm");
                lives--;
                SavePlayerData();
                LevelManager.instance.LoadScene(levelName);
            }
            else
            {
                PlaySound("SFX_UI_Cancel");
            }
        }

        public void PlaySound(string soundName)
        {
            AudioClip clickEffect = Resources.Load<AudioClip>(soundName);
            if (clickEffect)
            {
                _audioSrc.PlayOneShot(clickEffect);
            }
        }

    }
}