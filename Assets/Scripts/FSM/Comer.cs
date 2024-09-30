using UnityEngine;

public class Comer : IState
{
    private FSM _fsm;
    private Client _client;

    public Comer(FSM fsm, Client client)
    {
        _fsm = fsm;
        _client = client;
    }

    public void OnEnter()
    {
        if (_client.co != null)
        {
            _client.EndCoroutine(); // Stop any previous coroutine
        }

        Debug.Log("A COMERRR");
        _client.StartWaitEat();
    }

    public void OnUpdate()
    {

    }

    public void OnExit()
    {
        if(_client.comidaElegida != null)
        {
            _client.barraAsignada.AddMoneyToTable(_client.comidaElegida.price);
            FoodFactory.Instance.ReturnFood(_client.comidaElegida);
        }

        _client.EndCoroutine();
        
        _client.Exit();
    }
}
