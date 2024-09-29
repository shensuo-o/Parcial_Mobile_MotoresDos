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
        Debug.Log("Empiezo a esperar");
        _client.StartWaitSeat();
    }

    public void OnUpdate()
    {
        
    }

    public void OnExit()
    {
        _client.EndCoroutine();
    }
}
