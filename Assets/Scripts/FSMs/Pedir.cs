using Clients;
using Managers;
using ObjectPool;
using UnityEngine;

namespace FSMs
{
    public class Pedir : IState
    {
        private FSM _fsm;
        private Client _client;

        public Pedir(FSM fsm, Client client)
        {
            _fsm = fsm;
            _client = client;
        }

        // ReSharper disable Unity.PerformanceAnalysis
        public void OnEnter()
        {
            _client.seated = true;
            _client.GetComponent<Collider2D>().enabled = false;

            _client.soundManager.PlaySfx(_client.soundManager.askFood);
            if (_client.Co != null)
            {
                _client.EndCoroutine(); // Stop any previous coroutine
            }
        
            _client.selectedFood = FoodFactory.instance.foodPrefabs[Random.Range(0, FoodFactory.instance.foodPrefabs.Length)];

            if (_client.selectedFood.GetComponent<ComidaDia>())
            {
                Debug.Log(_client.selectedFood.foodName);
            }

            _client.dialogo.SetActive(true);
            _client.dialogo.GetComponentInChildren<ComidaEstetica>().SetSprite(_client.selectedFood.defaultSprite);

            Debug.Log("Pido un: " + _client.selectedFood.foodName);
            _client.StartWaitFood();
            Debug.Log("Espero la comida loco");
            GameManager.instance.NewOrder(_client.selectedFood);
        }

        public void OnUpdate()
        {

        }

        public void OnExit()
        {
            _client.EndCoroutine();
        }
    }
}
