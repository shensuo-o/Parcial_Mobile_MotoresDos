using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurchaseClient : MonoBehaviour
{
    public Client[] AbailableClients;
    public ClientFactory factory;
    public int factorySize = 0;
    public int factoryIndex = 0;

    void Start()
    {
        for (int i = 0; i < AbailableClients.Length; i++)
        {
            if (PlayerPrefs.GetInt("ClientAbailable" + AbailableClients[i].clientTag) == 1)
            {
                factorySize++;
            }
        }

        factory.clientPrefabs = new Client[factorySize];

        for (int i = 0; i < AbailableClients.Length; i++)
        {
            Debug.Log(PlayerPrefs.GetInt("ClientAbailable" + AbailableClients[i].clientTag));

            if (PlayerPrefs.GetInt("ClientAbailable" + AbailableClients[i].clientTag) == 1)
            {
                Debug.Log("Added " + AbailableClients[i]);

                factory.clientPrefabs[factoryIndex] = AbailableClients[i];
                factoryIndex++;
            }
        }
    }
}