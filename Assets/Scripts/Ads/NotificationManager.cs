using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Notifications.Android;
using System;

public class NotificationManager : MonoBehaviour
{
    void Start()
    {
        AndroidNotificationCenter.CancelAllDisplayedNotifications();
        AndroidNotificationCenter.CancelAllScheduledNotifications();
        AndroidNotificationCenter.CancelAllNotifications();

        var notifChannel = new AndroidNotificationChannel()
        {
            Id = "remind_notif_ch",
            Name = "Generic Channel",
            Description = "Descripcion",
            Importance = Importance.Default
        };

        AndroidNotificationCenter.RegisterNotificationChannel(notifChannel);

        var notif = new AndroidNotification()
        {
            Title = "Vuelve a jugar!",
            Text = "Te extrañamos :P",
            SmallIcon = "icon_small",
            LargeIcon = "icon_large",
            FireTime = DateTime.Now.AddMinutes(5)
        };

        AndroidNotificationCenter.SendNotification(notif, notifChannel.Id);
    }
}
