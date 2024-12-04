using System.Collections;
using UnityEditor.PackageManager;
using UnityEngine;

public class Client : MonoBehaviour
{
    private float _timerEntrance;
    private float _timerFoodWait;
    private float _timerConsume;

    public Plato selectedFood = null;
    public Plato onHandFood = null;
    public Barra assignedBar = null;
    public Seat clientSeat = null;
    public Seat hands = null;

    public bool seated = false;

    private FSM _fsm;

    public DragDrop dragdrop;

    public IEnumerator co;
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
        this.GetComponent<Renderer>().sortingLayerName = "Default";
        this.GetComponent<Renderer>().sortingOrder = 3;
        _timerEntrance = Random.Range(10, 15);
        _timerFoodWait = Random.Range(10, 15);
        _timerConsume = Random.Range(3, 6);
        _fsm.CreateState(FSM.ClientStates.Spawn, new Spawn(_fsm, this));
        _fsm.CreateState(FSM.ClientStates.Pidiendo, new Pedir(_fsm, this));
        _fsm.CreateState(FSM.ClientStates.Comiendo, new Comer(_fsm, this));
        _fsm.ChangeState(FSM.ClientStates.Spawn);
        soundManager.PlaySFX(soundManager.enter);
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
        assignedBar = barra;
        clientSeat = seat;
        transform.position = seat.transform.position;
        seat.SetUsed();
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
        Debug.Log("Me voy");
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
        soundManager.PlaySFX(soundManager.pays);
        _fsm.ExitState();
    }

    public void Exit()
    {
        EndCoroutine();
        if(clientSeat != null) clientSeat.SetFree();
        if(assignedBar != null) assignedBar.DeleteClient(this);
        if (onHandFood != null) FoodFactory.Instance.ReturnFood(onHandFood);
        dialogo.SetActive(false);
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
        int LayerObjects = LayerMask.NameToLayer("Objects");
        gameObject.layer = LayerObjects;
        dialogo.gameObject.SetActive(false);
    }

    public void DisableDrag()
    {
        dragdrop.enabled = false;
    }
    public void EnableDrag()
    {
        dragdrop.enabled = true;
    }
}
