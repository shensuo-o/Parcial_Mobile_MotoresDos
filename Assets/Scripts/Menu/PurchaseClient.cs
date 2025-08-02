using Clients;
using UnityEngine;

namespace Menu
{
    public class PurchaseClient : MonoBehaviour
    {
        public Client[] availableClients;
        private int _factorySize;
        private int _factoryIndex;

        void Start()
        {
            foreach (var t in availableClients)
            {
                if (PlayerPrefs.GetInt("ClientAbailable" + t.clientTag) >0 )
                {
                    _factorySize++;
                }
            }

            ClientFactory.Instance.clientPrefabs = new Client[_factorySize];

            foreach (var t in availableClients)
            {
                Debug.Log(PlayerPrefs.GetInt("ClientAbailable" + t.clientTag));

                if (PlayerPrefs.GetInt("ClientAbailable" + t.clientTag) >0)
                {
                    Debug.Log("Added " + t);

                    ClientFactory.Instance.clientPrefabs[_factoryIndex] = t;
                    _factoryIndex++;
                }
            }
        }
    }
}
