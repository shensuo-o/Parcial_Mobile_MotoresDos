using System.Collections;
using Audio;
using DragAndDrop;
using Managers;
using UnityEngine;

public class Client : MonoBehaviour
{
    private float _timerEntrance;
    private float _timerFoodWait;
    private float _timerConsume;

    public Plato selectedFood;
    public Plato onHandFood;
    public Barra assignedBar;
    public Seat clientSeat;
    public Seat hands;

    public bool seated;

    private FSM _fsm;

    public DragDrop dragdrop;

    public IEnumerator Co;
    public SoundManager soundManager;

    public int clientTag;
    public GameObject dialogo;

    private void Awake()
    {
        dragdrop = GetComponent<DragDrop>();
        hands = GetComponentInChildren<Seat>();
        soundManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<SoundManager>();
        _fsm = new FSM();
    }

    void Start()
    {
        GetComponent<Renderer>().sortingLayerName = "Default";
        GetComponent<Renderer>().sortingOrder = 3;
        _timerEntrance = Random.Range(10, 15);
        _timerFoodWait = Random.Range(10, 15);
        _timerConsume = Random.Range(3, 6);
        _fsm.CreateState(FSM.ClientStates.Spawn, new Spawn(_fsm, this));
        _fsm.CreateState(FSM.ClientStates.Pidiendo, new Pedir(_fsm, this));
        _fsm.CreateState(FSM.ClientStates.Comiendo, new Comer(_fsm, this));
        _fsm.ChangeState(FSM.ClientStates.Spawn);
        soundManager.PlaySfx(soundManager.enter);
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

    private void Seated()
    {
        _fsm.ChangeState(FSM.ClientStates.Pidiendo);
    }

    public void GoToSeat(Seat seat, Barra barra = null)
    {
        if (barra)
        {
            Seated();
        }
        assignedBar = barra;
        clientSeat = seat;
        transform.position = seat.transform.position;
        seat.SetUsed();
    }

    // ReSharper disable Unity.PerformanceAnalysis
    public void StartWaitSeat()
    {
        if (!gameObject.activeInHierarchy)
        {
            gameObject.SetActive(true);
        }

        Co = Wait(_timerEntrance - GameManager.instance.delayDifficulty);
        StartCoroutine(Co);
    }
    
    public void StartWaitFood()
    {
        if (!gameObject.activeInHierarchy)
        {
            gameObject.SetActive(true);
        }

        Co = Wait(_timerFoodWait);
        StartCoroutine(Co);
    }

    public void EndCoroutine()
    {
        if (Co != null)
        {
            StopCoroutine(Co);
            Co = null;
        }
    }

    // ReSharper disable Unity.PerformanceAnalysis
    IEnumerator Wait(float time)
    {
        yield return new WaitForSeconds(time);
        GameManager.instance.ClientGotOut();
        Debug.Log("Me voy");
        Exit();
    }

    // ReSharper disable Unity.PerformanceAnalysis
    public void StartWaitEat()
    {
        Co = WaitEat(_timerConsume);
        StartCoroutine(Co);
    }

    IEnumerator WaitEat(float time)
    {
        yield return new WaitForSeconds(time);
        Debug.Log("Ya comí");
        soundManager.PlaySfx(soundManager.pays);
        _fsm.ExitState();
    }

    // ReSharper disable Unity.PerformanceAnalysis
    public void Exit()
    {
        EndCoroutine();
        if(clientSeat) clientSeat.SetFree();
        if(assignedBar) assignedBar.DeleteClient(this);
        if (onHandFood) FoodFactory.Instance.ReturnFood(onHandFood);
        dialogo.SetActive(false);
        GameManager.instance.TryIncreaseDifficulty();
        ClientFactory.Instance.ReturnClient(this);
    }

    // ReSharper disable Unity.PerformanceAnalysis
    public static void TurnOnOff(Client c, bool active = true)
    {
        if (!active) c.EndCoroutine();
        if (active) c.Reset();
        c.gameObject.SetActive(active);
    }

    private void Reset()
    {
        EndCoroutine();
        EnableDrag();
        seated = false;
        assignedBar = null;
        selectedFood = null;
        onHandFood = null;
        clientSeat = null;
        hands.SetFree();
        _timerEntrance = Random.Range(10, 15);
        _timerFoodWait = Random.Range(10, 15);
        _timerConsume = Random.Range(3, 6);
        this.gameObject.GetComponent<Collider2D>().enabled = true;
        dragdrop.canDrop = false;
        int layerObjects = LayerMask.NameToLayer("Objects");
        gameObject.layer = layerObjects;
        dialogo.gameObject.SetActive(false);
    }

    public void DisableDrag()
    {
        dragdrop.enabled = false;
    }

    private void EnableDrag()
    {
        dragdrop.enabled = true;
    }
}
