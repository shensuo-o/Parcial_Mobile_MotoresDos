using UnityEngine;

namespace Managers.Tutorial
{
    public class TutorialManager : MonoBehaviour
    {
        public GameManager gameManager;
        public GameObject[] tutorialSteps;
        private int _currentStep;
    
        void Start()
        {
            // Configurar el GameManager para el tutorial
            if (!gameManager)
                gameManager = GameManager.instance;
        
            // Configuraciones para hacer el tutorial más fácil
            gameManager.lives = 5; // Dar más vidas
            gameManager.delayDifficulty = 0; // Dificultad mínima
        
            // Mostrar solo el primer paso del tutorial
            ShowCurrentTutorialStep();
        }
    
        void ShowCurrentTutorialStep()
        {
            // Ocultar todos los pasos
            foreach (var step in tutorialSteps)
            {
                step.SetActive(false);
            }
        
            // Mostrar el paso actual si está dentro del rango
            if (_currentStep < tutorialSteps.Length)
            {
                tutorialSteps[_currentStep].SetActive(true);
            }
        }
    
        public void NextTutorialStep()
        {
            _currentStep++;
            ShowCurrentTutorialStep();
        
            // Si es el último paso, mostrar un botón para continuar al juego principal
            if (_currentStep >= tutorialSteps.Length)
            {
                ShowCompletionMessage();
            }
        }
    
        private void ShowCompletionMessage()
        {
            // Mostrar mensaje de que el tutorial ha sido completado
            // y dar la opción de ir al juego principal
        }
    
        public void ReturnToMainMenu()
        {
            // Guardar que el tutorial fue completado
            PlayerPrefs.SetInt("TutorialCompleted", 1);
            PlayerPrefs.Save();
        
            // Volver al menú principal
            Level.LevelManager.instance.LoadScene();
        }
    }
}