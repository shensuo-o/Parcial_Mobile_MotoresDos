using TMPro;
using UnityEngine;

namespace Managers.Level
{
    public class HintManager : MonoBehaviour
    {
        public string[] hints;
        public TextMeshProUGUI hintText;
    
        public void RandomHint()
        {
            hintText.text = hints[Random.Range(0, hints.Length)];
        }
    }
}
