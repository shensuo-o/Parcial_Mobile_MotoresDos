using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pedir : IState
{
    private FSM _fsm;
    private Client _client;

    public Pedir(FSM fsm, Client client)
    {
        _fsm = fsm;
        _client = client;
    }


    public void OnEnter()
    {
        _client.comidaElegida = FoodFactory.Instance.foodPrefabs[Random.Range(0, FoodFactory.Instance.foodPrefabs.Length)];
        _client.comidaElegida.client = _client;
        Debug.Log("Pido un: " + _client.comidaElegida.name);
        Debug.Log("Active? " + _client.gameObject.activeInHierarchy);
        _client.StartWaitFood();
        Debug.Log("Espero la comida loco");
        GameManager.instance.NewOrder(_client.comidaElegida);
        
    }

    public void OnUpdate()
    {
        
    }

    public void OnExit()
    {
        _client.EndCoroutine();
    }
}
