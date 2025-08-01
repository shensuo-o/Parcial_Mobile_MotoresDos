using System.Collections;
using Audio;
using Clients;
using Gameplay;
using Menu;
using UnityEngine;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        public string playerName = ""; // Unity Cloud
        public int money;
        public int maxMoney; // Unity Cloud
        public int points;
        public int maxPoints; // Unity Cloud
        public static GameManager instance;
        public GameObject pause;
        public Cocina cocina;
        public Entrada entrada;
        public bool isAlive;
        
        // Modo de juego: false = modo infinito (vidas), true = modo tiempo limitado
        public bool isTimeLimitedMode;
        
        // Variables para el modo de tiempo limitado
        public float timeLimit = 180f; // 3 minutos por defecto
        public enum GameDifficulty { Easy, Hard }
        public GameDifficulty gameDifficulty = GameDifficulty.Easy;
        
        public int delayDifficulty;
        private readonly int _maxDelayDifficulty = 7;

        private int _attendedClients;
        
        public int lives = 3;

        private int _spawnRate;
        public bool canSpawn = true;

        private SoundManager _soundManager;
        private HudManager _hudManager;

        private void Start()
        {
            instance = this;
            _soundManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<SoundManager>();
            _hudManager = FindObjectOfType<HudManager>();

            if (RemoteConfigTest.instance.isConfigFetched)
            {
                InitializeVariables();
                StartCoroutine(Spawner());
                
                if (isTimeLimitedMode)
                {
                    SetupTimeLimitedMode();
                }
            }
            else
            {
                RemoteConfigTest.instance.OnConfigFetched += ConfigFetch;
            }
        }

        private void OnDestroy()
        {
            RemoteConfigTest.instance.OnConfigFetched -= ConfigFetch;
        }

        void ConfigFetch()
        {
            InitializeVariables();
            StartCoroutine(Spawner());
            
            if (isTimeLimitedMode)
            {
                SetupTimeLimitedMode();
            }
        }

        void InitializeVariables()
        {
            _spawnRate = RemoteConfigTest.instance.spawnRate;

            if (isTimeLimitedMode)
            {
                delayDifficulty = gameDifficulty == GameDifficulty.Easy ? 2 : // Valor de dificultad baja
                    _maxDelayDifficulty; // Valor de dificultad alta
            }
            else
            {
                delayDifficulty = 0; // Dificultad progresiva para modo infinito
            }
        }

        private void SetupTimeLimitedMode()
        {
            // Reiniciar el temporizador en HudManager
            if (_hudManager)
            {
                _hudManager.timer = 0f;
                _hudManager.timeLimit = timeLimit;
                _hudManager.isTimeLimitedMode = true;
            }
        }

        private void Update()
        {
            // Verificar el tiempo límite en modo tiempo limitado
            if (isTimeLimitedMode && _hudManager)
            {
                if (_hudManager.timer >= timeLimit)
                {
                    isAlive = false;
                    canSpawn = false;
                    SaveScore();
                    _soundManager.PlaySfx(_soundManager.lose);
                    pause.GetComponent<PauseMenu>().EndPanel();
                }
            }
        }

        private IEnumerator Spawner()
        {
            WaitForSeconds wait = new(_spawnRate);

            while (canSpawn)
            {
                yield return wait;
                if (entrada.PlaceAvailable())
                {
                    var client = ClientFactory.Instance.GetClient(ClientFactory.Instance.clientPrefabs[Random.Range(0, ClientFactory.Instance.clientPrefabs.Length)]);
                    if (!client) yield return null;

                    entrada.SpawnClient(client);
                }
                _spawnRate = RemoteConfigTest.instance.spawnRate;
            }
        }

        public void ClientGotOut()
        {
            _soundManager.PlaySfx(_soundManager.angry);
            lives--;
            
            // Solo verificar vidas en modo infinito
            if (!isTimeLimitedMode)
            {
                Status();
            }
        }

        private void Status()
        {
            if (lives <= 0)
            {
                isAlive = false;
                canSpawn = false;
                SaveScore();
                _soundManager.PlaySfx(_soundManager.lose);
                pause.GetComponent<PauseMenu>().EndPanel();
            }
        }

        public void TryIncreaseDifficulty()
        {
            _attendedClients++;
            
            // Solo aumentar dificultad en modo infinito
            if (!isTimeLimitedMode && _attendedClients % 5 == 0 && delayDifficulty < _maxDelayDifficulty)
            {
                delayDifficulty++;
            }
        }
        
        public void SaveScore()
        {
            PlayerPrefs.SetInt("saveScoreGame", money);
            PlayerPrefs.Save();
            Debug.Log($"Score saved: {money}");
        }

        public void NewOrder(Plato plato)
        {
            cocina.GetOrder(plato);
        }

        public void DeliverOrder(Client client, Plato plato, Barra bar)
        {
            plato.MoveTo(client.hands);
            client.GetFood(plato);
            plato.GetComponent<Collider2D>().enabled = false;
            bar.DeleteClient(client);
        }

        public void GetBarMoney(int d)
        {
            _soundManager.PlaySfx(_soundManager.collectPayment);
            money += d;
            SaveScore();
        }
    }
}