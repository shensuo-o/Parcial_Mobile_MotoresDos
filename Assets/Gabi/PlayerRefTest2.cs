using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerRefTest2 : MonoBehaviour
{
    public int noti;
    public int vibrate;
    public int acelerator;
    public TextMeshProUGUI tMPros1;
    public TextMeshProUGUI tMPro2;
    public TextMeshProUGUI tMPro3;

    // Start is called before the first frame update
    void Start()
    {
        noti = PlayerPrefs.GetInt("isNotificationActive");
        vibrate = PlayerPrefs.GetInt("isVibrationActive");
        acelerator = PlayerPrefs.GetInt("isAcceleratorActive");
    }

    
    public void NotitficationTest()
    {
        NotificationsController.SendTestNotification();

    }

}
