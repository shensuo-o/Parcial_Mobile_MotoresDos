using System.Collections;
using Audio;
using DragAndDrop;
using FSMs;
using Gameplay;
using Managers;
using ObjectPool;
using UnityEngine;

namespace Clients
{
    public class Client : MonoBehaviour
    {
        [HideInInspector] public float timerEntrance;
        [HideInInspector] public float timerFoodWait;
        [HideInInspector] public float timerConsume;
        [HideInInspector] public float TVTimeModifier;

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
            if (PlayerPrefs.GetInt("ClientTv") > 0)
            {
                TVTimeModifier = 1.5f;
            }
            else TVTimeModifier = 1;
            GetComponent<Renderer>().sortingLayerName = "Default";
            GetComponent<Renderer>().sortingOrder = 3;
            timerEntrance = Random.Range(10, 15);
            timerFoodWait = Random.Range(10, 15);
            timerConsume = Random.Range(3, 6);
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
            plato.onClientHands = true;
            
            // Verificar que el plato recibido coincide con el pedido
            if (selectedFood && plato.foodName != selectedFood.foodName)
            {
                Debug.LogWarning($"Cliente recibió {plato.foodName} pero pidió {selectedFood.foodName}");
                // Podrías implementar alguna penalización o comportamiento especial aquí
            }
            
            _fsm.ChangeState(FSM.ClientStates.Comiendo);
        }

        private void Seated()
        {
            _fsm.ChangeState(FSM.ClientStates.Pidiendo);
        }

        // ReSharper disable Unity.PerformanceAnalysis
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

            Co = Wait((timerEntrance - GameManager.instance.delayDifficulty));
            StartCoroutine(Co);
        }
    
        public void StartWaitFood()
        {
            if (!gameObject.activeInHierarchy)
            {
                gameObject.SetActive(true);
            }

            Co = Wait(timerFoodWait);
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
        private IEnumerator Wait(float time)
        {
            yield return new WaitForSeconds(time* TVTimeModifier);
            
            GameManager.instance.ClientGotOut();
            Debug.Log("Me voy");
            Exit();
        }

        // ReSharper disable Unity.PerformanceAnalysis
        public void StartWaitEat(float extraTime = 0)
        {
            Co = WaitEat(timerConsume + extraTime);
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
            clientSeat?.SetFree();
            clientSeat = null;
            assignedBar?.DeleteClient(this);
            assignedBar = null;
            selectedFood = null;
            hands.SetFree();
        
            if (onHandFood) FoodFactory.instance.ReturnFood(onHandFood);
            onHandFood = null;
        
            dialogo.SetActive(false);
            GameManager.instance.TryIncreaseDifficulty();
            ClientFactory.Instance.ReturnClient(this);
        }

        // ReSharper disable Unity.PerformanceAnalysis
        public static void TurnOnOff(Client c, bool active = true)
        {
            if(active) c.EndCoroutine();
            if(!active) c.Reset();

            c.gameObject.SetActive(active);
        }

        private void Reset()
        {
            EndCoroutine();
            EnableDrag();
            seated = false;
        
            if (clientSeat)
            {
                clientSeat.SetFree();
                clientSeat = null;
            }

            timerEntrance = Random.Range(10, 15);
            timerFoodWait = Random.Range(10, 15);
            timerConsume = Random.Range(3, 6);

            gameObject.GetComponent<Collider2D>().enabled = true;
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
}