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
        
        public int delayDifficulty;
        private readonly int _maxDelayDifficulty = 30;

        private int _attendedClients;
        
        public int lives = 3;

        private int _spawnRate;
        public bool canSpawn = true;

        private SoundManager _soundManager;

        private void Start()
        {
            instance = this;
            _soundManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<SoundManager>();

            if (RemoteConfigTest.instance.isConfigFetched)
            {
                InitializeVariables();
                StartCoroutine(Spawner());
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
        }

        void InitializeVariables()
        {
            _spawnRate = RemoteConfigTest.instance.spawnRate;
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
            Status();
        }

        private void Status()
        {
            if (lives == 0)
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
            if (_attendedClients % 5 != 0)
                return;
            
            if(delayDifficulty < _maxDelayDifficulty)
                delayDifficulty++;
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
