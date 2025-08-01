using UnityEngine;

namespace Menu
{
    public class DestroyButtons : MonoBehaviour
    {
        public GameObject[] buttons;

        void Start()
        {
            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i].GetComponent<ButtonInfo>().buttonTag = PlayerPrefs.GetInt("UsedButton" + buttons[i].GetComponent<ButtonInfo>().buttonNumber);

                if (buttons[i].GetComponent<ButtonInfo>().buttonTag == 1)
                {
                    buttons[i].SetActive(false);
                }
                else if (buttons[i].GetComponent<ButtonInfo>().buttonTag == 0)
                {
                    buttons[i].SetActive(true);
                }
            }
        }
    }
}
