using System.Collections;
using System.Collections.Generic;
using Unity.Notifications.Android;
using UnityEngine;
using System;
using UnityEngine.UI;

//using Unity.Notifications.Android;
public static class NotificationsController
{
    public static AndroidNotificationChannel channel1 = new AndroidNotificationChannel();
    public static bool IsPermissionPending;
    // Start is called before the first frame update
    public static void NotificationStart()
    {
        channel1 = new AndroidNotificationChannel()
        {
            Id = "remind_notif_ch",
            Name = "Generic Channel",
            Description = "Generic info",
            Importance = Importance.Low
        };
        AndroidNotificationCenter.RegisterNotificationChannel(channel1);
        //timeToPlayNotification = new AndroidNotification()
        //{
        //    Title = "lovo volve",
        //    Text = "paso mucho tiempoo te extra",
        //    FireTime = DateTime.Now,
        //    SmallIcon = "icon_small",
        //    LargeIcon = "icon_large"
        //};
        //AndroidNotificationCenter.SendNotification(timeToPlayNotification, channel1.Id);

    }
    
    //public static void SendNotification(AndroidNotification notif, AndroidNotificationChannel channel)
    //{
    //    if (PlayerPrefs.GetInt("isNotificationActive")==1)
    //    AndroidNotificationCenter.SendNotification(notif, channel.Id);

    //}
    public static void SendNewNotification(string title, string text, float firetime)
    {
        AndroidNotification _notification = new AndroidNotification()
        {
            Title = title,
            Text = text,
            FireTime = DateTime.Now.AddSeconds(firetime),
        };
        if (PlayerPrefs.GetInt("isNotificationActive") == 1)
        {
            AndroidNotificationCenter.SendNotification(_notification, channel1.Id);

        }
    }

    public static IEnumerator NotificationPermission()
    {
        var permissionRequest = new PermissionRequest();
        IsPermissionPending = true;
        while (permissionRequest.Status== PermissionStatus.RequestPending)
        {
            yield return null;
        }
        if (permissionRequest.Status == PermissionStatus.Denied)
        {
            PlayerPrefs.SetInt("isNotificationActive", 0);
            PlayerPrefs.Save();

        }
        else if (permissionRequest.Status == PermissionStatus.Allowed)
        {
            PlayerPrefs.SetInt("isNotificationActive", 1);
            PlayerPrefs.Save();
        }
        IsPermissionPending = false;
    }
    public  static void SendTestNotification()
    {

        SendNewNotification("PROBANDO","123 123",0);
    }
}

