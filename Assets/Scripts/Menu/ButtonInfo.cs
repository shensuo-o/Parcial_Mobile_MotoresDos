using Managers.Menu;
using UnityEngine;

namespace Menu
{
    public class ButtonInfo : MonoBehaviour
    {
        public int buttonTag;
        public int buttonNumber;
    
        public void SetPp()
        {
            PlayerPrefs.SetInt("UsedButton" + buttonNumber, 1);
        }

        public void ByeButton()
        {
            if (MenuManager.instance.savedMoney >= 15)
            {
                Destroy(gameObject, 1);
            }
        }
    }
}
