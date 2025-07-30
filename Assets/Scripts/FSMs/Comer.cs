using Clients;
using UnityEngine;

namespace FSMs
{
    public class Comer : IState
    {
        private FSM _fsm;
        private Client _client;

        public Comer(FSM fsm, Client client)
        {
            _fsm = fsm;
            _client = client;
        }

        // ReSharper disable Unity.PerformanceAnalysis
        public void OnEnter()
        {
            if (_client.Co != null)
            {
                _client.EndCoroutine(); // Stop any previous coroutine
            }

            _client.dialogo.SetActive(false);

            var food = _client.selectedFood;
            float extraTime = 0;

            if (food is { isFrozen: true })
            {
                extraTime = _client.timerConsume * 0.5f;
            }

            _client.StartWaitEat(extraTime);

            Debug.Log("A COMER");
        }

        public void OnUpdate()
        {
        
        }

        public void OnExit()
        {
            if (_client.onHandFood)
            {
                var price = _client.onHandFood.isFrozen ? Mathf.FloorToInt(_client.selectedFood.price * 0.5f) : _client.selectedFood.price;

                _client.assignedBar.AddMoneyToTable(price);
            }
            _client.EndCoroutine();
            _client.Exit();
        }
    }
}