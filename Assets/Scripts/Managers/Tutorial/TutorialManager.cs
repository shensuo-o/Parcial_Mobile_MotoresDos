using System.Collections;
using Clients;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Managers.Tutorial
{
    public class TutorialManager : MonoBehaviour
    {
        [Header("Referencias")]
        public GameObject tutorialUI;
        public TextMeshProUGUI tutorialText;
        public GameObject nextButton;
    
        [Header("Configuración")]
        public string[] tutorialSteps;
        public float delayBetweenSteps = 0.5f;
    
        private int _currentStep = 0;
        private Client _currentClient;
        private bool _waitingForInput;
    
        void Start()
        {
            // Configurar modo tutorial en GameManager
            GameManager.instance.isTutorialMode = true;
        
            // Iniciar el tutorial
            StartCoroutine(TutorialSequence());
        }
    
        private IEnumerator TutorialSequence()
        {
            yield return new WaitForSeconds(1f);
        
            // Paso 1: Bienvenida al tutorial
            ShowTutorialMessage("¡Bienvenido al tutorial! Aprenderás cómo jugar paso a paso.");
            yield return WaitForPlayerInput();
        
            // Paso 2: Explicar sobre los clientes
            ShowTutorialMessage("Los clientes entrarán al restaurante. Tu trabajo es atenderlos.");
            yield return WaitForPlayerInput();
        
            // Paso 3: Generar primer cliente
            ShowTutorialMessage("¡Aquí viene tu primer cliente!");
            yield return new WaitForSeconds(.1f);
        
            _currentClient = GameManager.instance.SpawnTutorialClient();
            yield return new WaitForSeconds(.5f); // Esperar a que el cliente se mueva
        
            // Paso 4: Explicar cómo sentar al cliente
            ShowTutorialMessage("Arrastra al cliente a una mesa disponible para sentarlo.");
            yield return WaitUntilClientIsSeated();
        
            
        
            // Finalizar tutorial
            ShowTutorialMessage("¡Felicidades! Has completado el tutorial básico.");
            yield return WaitForPlayerInput();
        
            // Volver al modo normal
            GameManager.instance.isTutorialMode = false;
            tutorialUI.SetActive(false);
        }
    
        private void ShowTutorialMessage(string message)
        {
            tutorialUI.SetActive(true);
            tutorialText.text = message;
            nextButton.SetActive(true);
            _waitingForInput = true;
        }
    
        private IEnumerator WaitForPlayerInput()
        {
            while (_waitingForInput)
            {
                if (Input.GetMouseButtonDown(0) || 
                    (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began))
                {
                    _waitingForInput = false;
                    nextButton.SetActive(false);
                }
                yield return null;
            }
        
            yield return new WaitForSeconds(delayBetweenSteps);
            _waitingForInput = true;
        }
    
        private IEnumerator WaitUntilClientIsSeated()
        {
            // Esperar hasta que el cliente esté sentado
            while (_currentClient != null && !_currentClient.seated)
            {
                yield return null;
            }
        
            yield return new WaitForSeconds(delayBetweenSteps);
        }
    
        // Botón de "Siguiente" para UI
        public void NextStep()
        {
            _waitingForInput = false;
            nextButton.SetActive(false);
        }
    }
}