using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

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
        _client.StartWaitEat();
    }

    public void OnUpdate()
    {

    }

    public void OnExit()
    {
        _client.barraAsignada.AddToTable(_client.comidaElegida.price);
        _client.Exit();
    }

    
}
