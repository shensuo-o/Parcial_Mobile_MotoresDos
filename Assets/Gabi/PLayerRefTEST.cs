using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PLayerRefTEST : MonoBehaviour
{
    public int noti;
    public int vibrate;
    public int acelerator;
    public TextMeshProUGUI tMPro1;
    public TextMeshProUGUI tMPro2;
    public TextMeshProUGUI tMPro3;

    // Start is called before the first frame update
    void Start()
    {
        noti = PlayerPrefs.GetInt("isNotificationActive");
        vibrate = PlayerPrefs.GetInt("isVibrationActive");
        acelerator = PlayerPrefs.GetInt("isAcceleratorActive");
    }

    // Update is called once per frame
    void Update()
    {
        noti = PlayerPrefs.GetInt("isNotificationActive");
        vibrate = PlayerPrefs.GetInt("isVibrationActive");
        acelerator = PlayerPrefs.GetInt("isAcceleratorActive");
        if(tMPro1!=null)
        tMPro1.text = noti.ToString();
        if (tMPro2 != null)

            tMPro2.text = tMPro2.ToString();
        if (tMPro3 != null)

            tMPro3.text = noti.ToString();
    }
    public void NotitficationTest()
    {
        NotificationsController.SendTestNotification();
    }
}
