using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public string playerName = ""; //Unity Cloud
    public int money = 0;
    public int maxMoney; //Unity Cloud
    public int points; 
    public int maxPoints; //Unity Cloud
    public static GameManager instance;
    public GameObject Pause;
    public Cocina cocina;
    public List<Barra> barras;
    public Entrada entrada;
    public bool isAlive;

    public int lives = 3;

    private int spawnRate = 0;
    public bool canSpawn = true;

    private void Start()
    {
        instance = this;

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
        spawnRate = RemoteConfigTest.instance.spawnRate;
    }

    private IEnumerator Spawner()
    {
        WaitForSeconds wait = new(spawnRate);

        while (canSpawn)
        {
            yield return wait;
            if (entrada.PlaceAvailable())
            {
                var client = ClientFactory.Instance.GetClient(ClientFactory.Instance.clientPrefabs[Random.Range(0, ClientFactory.Instance.clientPrefabs.Length)]);
                if (!client) yield return null;

                entrada.SpawnClient(client);
            }
            spawnRate = RemoteConfigTest.instance.spawnRate;
        }
    }
    
    public void ClientGotOut()
    {
        lives--;
        Status();
    }

    public void Status()
    {
        if (lives == 0)
        {
            isAlive = false;
            canSpawn = false;
            SaveScore();
            Pause.GetComponent<PauseMenu>().LoadlMenu();
        }
    }

    public void SaveScore()
    {
        PlayerPrefs.SetInt("saveScoreGame", money);
    }

    public void NewOrder(Plato plato)
    {
        cocina.GetOrder(plato);
    }

    public void DeliverOrder(Plato plato)
    {
        plato.MoveTo(plato.client.manos);
        plato.client.GetFood(plato);
        plato.GetComponent<Collider2D>().enabled = false;
    }

    public void GetBarMoney(int d)
    {
        money += d;
        Debug.Log(money);
    }
}
