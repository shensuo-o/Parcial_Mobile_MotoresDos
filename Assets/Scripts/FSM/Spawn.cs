using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : IState
{
    private FSM _fsm;
    private Client _client;


    public Spawn(FSM fsm, Client client)
    {
        _fsm = fsm;
        _client = client;
    }


    public void OnEnter()
    {
        if (_client.Co != null)
        {
            _client.EndCoroutine(); // Stop any previous coroutine
        }
        Debug.Log("Active? " + _client.gameObject.activeInHierarchy);

        Debug.Log("Empiezo a esperar");
        _client.StartWaitSeat();
        Debug.Log(_client.Co);
    }

    public void OnUpdate()
    {
        
    }

    public void OnExit()
    {
        _client.EndCoroutine();
    }
}
