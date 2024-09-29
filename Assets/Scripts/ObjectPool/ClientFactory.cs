using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientFactory : MonoBehaviour
{
    public static ClientFactory Instance;

    public Client[] clientPrefabs;
    public int stonks = 15;
    public bool dynamic = true;

    public ObjectPool<Client> clientPool;

    void Start()
    {
        Instance = this;
        clientPool = new ObjectPool<Client>(ClientCreator, Client.TurnOnOff, stonks, dynamic);
    }

    public Client ClientCreator()
    {
        return Instantiate(clientPrefabs[Random.Range(0, clientPrefabs.Length)], transform);
    }

    public void ReturnClient(Client c)
    {
        clientPool.ReturnObject(c);
    }
}
