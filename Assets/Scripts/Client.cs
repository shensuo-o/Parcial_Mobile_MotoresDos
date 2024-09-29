using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public class Client : MonoBehaviour
{
    public Plato comidaElegida;
    public Barra barraAsignada;

    public float timerEntrada;
    public float timerPedido;
    public float timerComida;
    public float timerConsume;

    private FSM _fsm;

    void Start()
    {
        _fsm = new FSM();
        _fsm.CreateState(FSM.ClientStates.Spawn, new Spawn(_fsm, this));
        _fsm.CreateState(FSM.ClientStates.Pidiendo, new Pedir(_fsm, this));
        _fsm.CreateState(FSM.ClientStates.Comiendo, new Comer(_fsm, this));
        _fsm.ChangeState(FSM.ClientStates.Spawn);
    }

    protected void Update()
    {
        _fsm.ArtificialUpdate();
    }

    public void GetFood()
    {
        _fsm.ChangeState(FSM.ClientStates.Comiendo);
    }
    
    public void Seated()
    {
        _fsm.ChangeState(FSM.ClientStates.Pidiendo);
    }

    public void StartWaitSeat()
    {
        StartCoroutine(Wait(timerEntrada));
    }

    public void EndWaitSeat()
    {
        StopCoroutine(Wait(timerEntrada));
    }

    public void StartWaitFood()
    {
        StartCoroutine(Wait(timerComida));
    }

    public void EndWaitFood()
    {
        StopCoroutine(Wait(timerComida));
    }

    IEnumerator Wait(float time)
    {
        yield return new WaitForSeconds(time);
        Exit();
    }
    public void StartWaitEat()
    {
        StartCoroutine(WaitEat(timerConsume));
    }

    public void EndWaitEat()
    {
        StopCoroutine(WaitEat(timerConsume));
    }

    IEnumerator WaitEat(float time)
    {
        yield return new WaitForSeconds(time);
        _fsm.ChangeState(FSM.ClientStates.Spawn);
    }

    public void Exit()
    {
        GameManager.instance.lives--;
        Destroy(this);
    }
}
