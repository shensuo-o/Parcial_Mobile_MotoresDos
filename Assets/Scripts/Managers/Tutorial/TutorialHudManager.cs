using System.Collections;
using TMPro;
using UnityEngine;

namespace Managers.Tutorial
{
    public class TutorialHudManager : MonoBehaviour
    {
        public TextMeshProUGUI instructionText;
        public GameObject nextButton;
    
        public void ShowInstruction(string text, bool showNextButton = true)
        {
            instructionText.text = text;
            nextButton.SetActive(showNextButton);
        }
    
        public void ShowTimedInstruction(string text, float duration)
        {
            StartCoroutine(TimedInstructionCoroutine(text, duration));
        }
    
        private IEnumerator TimedInstructionCoroutine(string text, float duration)
        {
            instructionText.text = text;
            nextButton.SetActive(false);
        
            yield return new WaitForSeconds(duration);
        
            instructionText.text = "";
        }
    }
}