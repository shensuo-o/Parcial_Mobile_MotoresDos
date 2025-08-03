using System.Collections;
using Clients;
using Gameplay;
using TMPro;
using UnityEngine;

namespace Managers.Tutorial
{
    public class TutorialManager : MonoBehaviour
    {
        [Header("Referencias")]
        public GameObject tutorialUI;
        public TextMeshProUGUI tutorialText;
        public GameObject nextButton;
        public GameObject endPanel;
        public Plato foodPrefab;
    
        [Header("Configuración")]
        public float delayBetweenSteps = 0.5f;
        
        private Client _currentClient;
        private Barra _currentBarra;
        public Plato _currentFood;
        private bool _canContinue;
        private bool _waitingForCondition;
    
        void Start()
        {
            GameManager.instance.isTutorialMode = true;
            
            StartCoroutine(TutorialSequence());
        }
    
        private IEnumerator TutorialSequence()
        {
            tutorialUI.SetActive(true);

            ShowTutorialMessage("¡Bienvenido al tutorial! Aprenderas como jugar paso a paso.");
            yield return WaitForNextButton();

            ShowTutorialMessage("Los clientes entraran al restaurante. Tu trabajo es atenderlos.");
            yield return WaitForNextButton();

            ShowTutorialMessage("¡Aqui viene tu primer cliente!");
            nextButton.SetActive(false);
            _canContinue = true;
            yield return new WaitForSeconds(1f);

            _currentClient = GameManager.instance.SpawnTutorialClient();
            yield return new WaitForSeconds(.3f);

            ShowTutorialMessage("Arrastra al cliente a una mesa disponible para sentarlo.");
            yield return WaitUntilConditionMet(() => _currentClient && _currentClient.assignedBar);
            _currentBarra = _currentClient.assignedBar;

            ShowTutorialMessage("¡Bien hecho! Ahora el cliente hara su pedido.\n Entregalo arrastrandolo a la mesa.");
            yield return WaitUntilConditionMet(() => _currentClient && !_currentClient.hands.IsFree());
            
            ShowTutorialMessage("¡Bien hecho! Recuerda no hacerlo esperar mucho al cliente, se podria retirar.");
            yield return WaitUntilConditionMet(() => _currentClient && !_currentClient.clientSeat);

            ShowTutorialMessage("Bien! Ahora recoje el dinero de la mesa.");
            yield return WaitUntilConditionMet(() => _currentBarra && !_currentBarra.coin.activeSelf);
            
            ShowTutorialMessage("Si un cliente se retira y su comida sigue en la barra, deberás tirarla a la basura.");
            yield return WaitForNextButton();
            
            GameManager.instance.NewOrder(foodPrefab);
            yield return new WaitForSeconds(foodPrefab.timeToCook);
            yield return null;
            _currentFood = FindObjectOfType<Plato>();
            
            ShowTutorialMessage("Intentemoslo ahora. Arrasta la comida a la basura.");
            yield return WaitUntilConditionMet(() => !_currentFood.gameObject.activeSelf && _currentFood.place);
            
            ShowTutorialMessage("¡Felicidades! Has completado el tutorial basico.");
            yield return WaitForNextButton();
            
            //pantalla de final
            tutorialUI.SetActive(false);
            endPanel.SetActive(true);
        }
    
        private void ShowTutorialMessage(string message)
        {
            tutorialUI.SetActive(true);
            tutorialText.text = message;
            
            // Solo mostrar el botón Next si no estamos esperando que se cumpla una condición
            nextButton.SetActive(!_waitingForCondition);
        }
    
        private IEnumerator WaitForNextButton()
        {
            _canContinue = false;
            _waitingForCondition = false;
            
            // Esperar hasta que _canContinue sea true (lo que ocurre cuando se presiona Next)
            while (!_canContinue)
            {
                yield return null;
            }
            
            yield return new WaitForSeconds(delayBetweenSteps);
        }
        
        private IEnumerator WaitUntilConditionMet(System.Func<bool> condition)
        {
            _waitingForCondition = true;
            nextButton.SetActive(false);
            
            // Esperar hasta que se cumpla la condición
            while (!condition())
            {
                yield return null;
            }
            
            _waitingForCondition = false;
            
            yield return new WaitForSeconds(delayBetweenSteps);
        }
    
        // Botón de "Siguiente" para UI
        public void NextStep()
        {
            _canContinue = true;
        }
    }
}