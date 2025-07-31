using System.Collections;
using System.Collections.Generic;
using Unity.Notifications.Android;
using UnityEngine;
using System;

//using Unity.Notifications.Android;
public class NotificationsController : MonoBehaviour
{

    public SpriteRenderer _gameObject;
    AndroidNotification timeToPlayNotification = new AndroidNotification();
    AndroidNotificationChannel channel1 = new AndroidNotificationChannel();
    // Start is called before the first frame update
    void Start()
    {
        //AndroidNotificationCenter.CancelAllDisplayedNotifications();
        //AndroidNotificationCenter.CancelAllNotifications();
        //AndroidNotificationCenter.CancelAllScheduledNotifications();
        channel1 = new AndroidNotificationChannel()
        {
            Id = "remind_notif_ch",
            Name = "Generic Channel",
            Description = "zarasa",
            Importance = Importance.Low
        };
        AndroidNotificationCenter.RegisterNotificationChannel(channel1);
        timeToPlayNotification = new AndroidNotification()
        {
            Title = "lovo volve",
            Text = "paso mucho tiempoo te extra",
            FireTime = DateTime.Now,
            SmallIcon = "icon_small",
            LargeIcon = "icon_large"
        };

        AndroidNotificationCenter.SendNotification(timeToPlayNotification, channel1.Id);

    }
    
    public void SendNotification(AndroidNotification notif, AndroidNotificationChannel channel)
    {
        AndroidNotificationCenter.SendNotification(notif, channel.Id);

    }
    public void SendTestNotification()
    {
        AndroidNotificationCenter.SendNotification(timeToPlayNotification, channel1.Id);
        _gameObject.color = Color.blue;

    }

    
}

