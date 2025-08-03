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
		// Añadir esta variable al inicio de la clase
		public bool isTutorialMode;
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
        public bool gachasAdded;
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

		// Modificar el método Start para que no inicie el spawner en modo tutorial
		private void Start()
        {
            instance = this;
            _soundManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<SoundManager>();
            _hudManager = FindObjectOfType<HudManager>();

            if (RemoteConfigTest.instance.isConfigFetched)
            {
                InitializeVariables();
                
                // Solo iniciar el spawner automático si NO estamos en modo tutorial
                if (!isTutorialMode)
                {
                    StartCoroutine(Spawner());
                }

                if (isTimeLimitedMode)
                {
                    SetupTimeLimitedMode();
                }
            }
            else
            {
                RemoteConfigTest.instance.OnConfigFetched += ConfigFetch;
            }
            gachasAdded = false;
        }

        private void OnDestroy()
        {
            RemoteConfigTest.instance.OnConfigFetched -= ConfigFetch;
        }

		// Modificar el método ConfigFetch para respetar el modo tutorial
		void ConfigFetch()
        {
            InitializeVariables();
            
            // Solo iniciar el spawner automático si NO estamos en modo tutorial
            if (!isTutorialMode)
            {
                StartCoroutine(Spawner());
            }

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

		// Modificar los métodos que afectan la dificultad para que no se activen en modo tutorial
		public void ClientGotOut()
        {
            _soundManager.PlaySfx(_soundManager.angry);
            lives--;

            // Solo verificar vidas en modo infinito y si no estamos en tutorial
            if (!isTimeLimitedMode && !isTutorialMode)
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

		// Modificar los métodos que afectan la dificultad para que no se activen en modo tutorial
		public void TryIncreaseDifficulty()
        {
            _attendedClients++;

            // Solo aumentar dificultad en modo infinito y si no estamos en tutorial
            if (!isTimeLimitedMode && !isTutorialMode && _attendedClients % 5 == 0 && delayDifficulty < _maxDelayDifficulty)
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
        public void GainGachas()
        {
            if (!gachasAdded)
            {
                PlayerPrefs.SetInt("GatchasPending", PlayerPrefs.GetInt("GatchasPending") + money / 15);
                print("sumaste gatchas " + (money / 15));
                gachasAdded = true;
            }

        }
        
		public Client SpawnTutorialClient(int clientType = 0)
		{
			clientType = Mathf.Clamp(clientType, 0, ClientFactory.Instance.clientPrefabs.Length - 1);
			
			var client = ClientFactory.Instance.GetClient(ClientFactory.Instance.clientPrefabs[clientType]);
			if (client && entrada.PlaceAvailable())
			{
				entrada.SpawnClient(client);
				
				// Hacer que el cliente no se vaya (tiempos muy largos)
				client.timerEntrance = 999f;
				client.timerFoodWait = 999f;
				
				// Asegurarse de que el cliente tenga el componente DragDrop habilitado
				var dragDrop = client.GetComponent<DragAndDrop.DragDrop>();
				if (dragDrop)
				{
					dragDrop.enabled = true;
					dragDrop.canDrag = true;
				}
				
				return client;
			}
			return null;
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