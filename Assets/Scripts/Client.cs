using System.Collections;
using UnityEngine;

public class Client : MonoBehaviour
{
    private float _timerEntrance;
    private float _timerFoodWait;
    private float _timerConsume;

    public Vector3 lastPosition;

    public Plato selectedFood = null;
    public Plato onHandFood = null;
    public Barra assignedBar = null;
    public Seat clientSeat = null;
    public Seat hands = null;

    public bool seated = false;

    private FSM _fsm;

    public DragDrop dragdrop;

    public IEnumerator co;

    private void Awake()
    {
        dragdrop = GetComponent<DragDrop>();
        hands = GetComponentInChildren<Seat>();
        _fsm = new FSM();
    }

    void Start()
    {
        this.GetComponent<Renderer>().sortingLayerName = "Default";
        this.GetComponent<Renderer>().sortingOrder = 3;
        _timerEntrance = Random.Range(10, 15);
        _timerFoodWait = Random.Range(10, 15);
        _timerConsume = Random.Range(3, 6);
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
        onHandFood = plato;
        _fsm.ChangeState(FSM.ClientStates.Comiendo);
    }

    public void Seated()
    {
        _fsm.ChangeState(FSM.ClientStates.Pidiendo);
    }

    public void GoToSeat(Seat seat, Barra barra = null)
    {
        if (barra != null)
        {
            Seated();
        }
        seated = true;
        assignedBar = barra;
        clientSeat = seat;
        lastPosition = seat.transform.position;
        transform.position = seat.transform.position;        
        seat.ChangeStatus();
    }

    public void StartWaitSeat()
    {
        if (!gameObject.activeInHierarchy)
        {
            gameObject.SetActive(true);
        }

        co = Wait(_timerEntrance);
        StartCoroutine(co);
    }
    
    public void StartWaitFood()
    {
        if (!gameObject.activeInHierarchy)
        {
            gameObject.SetActive(true);
        }

        co = Wait(_timerFoodWait);
        StartCoroutine(co);
    }

    public void EndCoroutine()
    {
        if (co != null)
        {
            StopCoroutine(co);
            co = null;
        }
    }

    IEnumerator Wait(float time)
    {
        yield return new WaitForSeconds(time);
        GameManager.instance.ClientGotOut();
        Exit();
    }

    public void StartWaitEat()
    {
        co = WaitEat(_timerConsume);
        StartCoroutine(co);
    }

    IEnumerator WaitEat(float time)
    {
        yield return new WaitForSeconds(time);
        Debug.Log("Ya comí");
        _fsm.ExitState();
    }

    public void Exit()
    {
        EndCoroutine();
        clientSeat.ChangeStatus();
        assignedBar.DeleteClient(this);
        ClientFactory.Instance.ReturnClient(this);
    }

    public static void TurnOnOff(Client c, bool active = true)
    {
        if (!active) c.EndCoroutine();
        if (active) c.Reset();
        c.gameObject.SetActive(active);
    }

    private void Reset()
    {
        dragdrop.enabled = true;
        EndCoroutine();
        seated = false;
        assignedBar = null;
        lastPosition = Vector3.zero;
        selectedFood = null;
        onHandFood = null;
        clientSeat = null;
        hands.SetFree();
        _timerEntrance = Random.Range(10, 15);
        _timerFoodWait = Random.Range(10, 15);
        _timerConsume = Random.Range(3, 6);
    }

    internal void DisableDrag()
    {
        dragdrop.enabled = false;
    }
}
