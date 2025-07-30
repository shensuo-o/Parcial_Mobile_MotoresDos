using System.Collections.Generic;
using Clients;
using UnityEngine;

public class ClientFactory : MonoBehaviour
{
    public static ClientFactory Instance;

    public Client[] clientPrefabs;
    public int stonks = 15;
    public bool dynamic = true;

    public Dictionary<Client, ObjectPool<Client>> clientPools = new Dictionary<Client, ObjectPool<Client>>();

    void Awake()
    {
        Instance = this;
        foreach (Client c in clientPrefabs)
        {
            if (!clientPools.ContainsKey(c))
            {
                // Crear un pool si no existe una para esa comida
                clientPools[c] = new ObjectPool<Client>(() => ClientCreator(c), Client.TurnOnOff, stonks, dynamic);
            }
        }
    }

    public Client ClientCreator(Client c)
    {
        return Instantiate(c, transform);
    }

    public Client GetClient(Client c)
    {
        if (!clientPools.ContainsKey(c))
        {
            clientPools[c] = new ObjectPool<Client>(() => ClientCreator(c), Client.TurnOnOff, stonks, dynamic);
        }

        return clientPools[c].GetObject();
    }

    public void ReturnClient(Client c)
    {
        // Encontrar la pool y traer el cliente
        foreach (var poolEntry in clientPools)
        {
            if (poolEntry.Key.GetType() == c.GetType())
            {
                poolEntry.Value.ReturnObject(c);
                return;
            }
        }
    }
}
