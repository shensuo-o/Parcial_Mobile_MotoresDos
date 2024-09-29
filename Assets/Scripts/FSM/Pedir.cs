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
        _client.comidaElegida = GameManager.instance.menu[Random.Range(0, GameManager.instance.menu.Length)];
        _client.StartWaitFood();
        GameManager.instance.CrearPedido(_client.comidaElegida);
    }

    public void OnUpdate()
    {
        
    }

    public void OnExit()
    {
        
    }
}
