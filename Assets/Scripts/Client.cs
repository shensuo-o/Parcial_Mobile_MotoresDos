using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.Search;
using UnityEngine;

public class Client : MonoBehaviour
{
    private float _timerEntrada;
    private float _timerComida;
    private float _timerConsume;

    public Plato comidaElegida = null;
    public Barra barraAsignada = null;
    public Seat asiento = null;
    public Seat manos = null;

    public bool seated = false;

    private FSM _fsm;

    private IEnumerator co;

    private void Awake()
    {
        manos = GetComponentInChildren<Seat>();
        _fsm = new FSM();
    }

    void Start()
    {
        
        this.GetComponent<Renderer>().sortingLayerName = "Default";
        this.GetComponent<Renderer>().sortingOrder = 3;
        _timerEntrada = Random.Range(10, 15);
        _timerComida = Random.Range(10, 15);
        _timerConsume = Random.Range(10, 15);
        _fsm.CreateState(FSM.ClientStates.Spawn, new Spawn(_fsm, this));
        _fsm.CreateState(FSM.ClientStates.Pidiendo, new Pedir(_fsm, this));
        _fsm.CreateState(FSM.ClientStates.Comiendo, new Comer(_fsm, this));
        _fsm.ChangeState(FSM.ClientStates.Spawn);
    }

    protected void Update()
    {

        _fsm.ArtificialUpdate();
        
    }

    public void GetFood(Plato plato)
    {
        comidaElegida = plato;
        _fsm.ChangeState(FSM.ClientStates.Comiendo);
    }

    public void Seated()
    {
        seated = true;
        _fsm.ChangeState(FSM.ClientStates.Pidiendo);
    }

    public void GoToSeat(Seat seat, Barra barra = null)
    {
        barraAsignada = barra;
        asiento = seat;
        transform.position = seat.transform.position;
        seat.ChangeStatus();
        if (barra != null)
        {
            Seated();
        }
    }

    public void StartWaitSeat()
    {
        co = Wait(_timerEntrada);
        StartCoroutine(co);
    }
    
    public void StartWaitFood()
    {
        co = Wait(_timerComida);
        StartCoroutine(co);
    }

    public void EndCoroutine()
    {
        StopCoroutine(co);
    }

    IEnumerator Wait(float time)
    {
        yield return new WaitForSeconds(time);
        GameManager.instance.ClientGotOut();
        Exit();
    }

    public void StartWaitEat()
    {
        StartCoroutine(WaitEat(_timerConsume));
    }

    IEnumerator WaitEat(float time)
    {
        yield return new WaitForSeconds(time);
        Debug.Log("Ya comí");
        _fsm.ExitState();
    }

    public void Exit()
    {
        asiento.ChangeStatus();
        ClientFactory.Instance.ReturnClient(this);
    }

    public static void TurnOnOff(Client c, bool active = true)
    {
        if (active) c.Reset();
        c.gameObject.SetActive(active);
    }

    public void Reset()
    {
        seated = false;
        barraAsignada = null;
        comidaElegida = null;
        asiento = null;
        manos.SetFree();
        _timerEntrada = Random.Range(10, 15);
        _timerComida = Random.Range(10, 15);
        _timerConsume = Random.Range(3, 6);
    }
}
