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
    public static AndroidNotificationChannel channel2 = new AndroidNotificationChannel();
    public static AndroidNotificationChannel channel3 = new AndroidNotificationChannel();
    public static AndroidNotificationChannel channel4 = new AndroidNotificationChannel();
    public static bool IsPermissionPending;
    // Start is called before the first frame update
    public static void NotificationStart()
    {
        channel1 = new AndroidNotificationChannel()
        {
            Id = "remind_notif_ch",
            Name = "Generic Channel",
            Description = "Generic info",
            Importance = Importance.High
        };
        AndroidNotificationCenter.RegisterNotificationChannel(channel1);
        channel2 = new AndroidNotificationChannel()
        {
            Id = "remind_notif_ch2",
            Name = "Generic Channe2",
            Description = "Generic info",
            Importance = Importance.High
        };
        AndroidNotificationCenter.RegisterNotificationChannel(channel2);
        channel3 = new AndroidNotificationChannel()
        {
            Id = "remind_notif_ch3",
            Name = "Generic Channe3",
            Description = "Generic info",
            Importance = Importance.High
        };
        AndroidNotificationCenter.RegisterNotificationChannel(channel3);
        channel4 = new AndroidNotificationChannel()
        {
            Id = "remind_notif_ch4",
            Name = "Generic Channe4",
            Description = "Generic info",
            Importance = Importance.High
        };
        AndroidNotificationCenter.RegisterNotificationChannel(channel4);

    }


    public static void SendNewNotification(string title, string text, double firetime,int id )
    {
        AndroidNotification _notification = new AndroidNotification()
        {
            Title = title,
            Text = text,
            FireTime = DateTime.Now.AddSeconds(firetime),
            SmallIcon= "icon_small",
            LargeIcon= "icon_large",
        };
        if (PlayerPrefs.GetInt("isNotificationActive") == 1)
        {
            AndroidNotificationCenter.SendNotificationWithExplicitID(_notification, channel1.Id, id);
        }
    }
    public static void SendNewRepeatedNotification(string title, string text, double firetime, double repeatTime, int id)
    {
        AndroidNotification _notification = new AndroidNotification()
        {
            Title = title,
            Text = text,
            FireTime = DateTime.Now.AddSeconds(firetime),
            RepeatInterval = TimeSpan.FromSeconds(repeatTime),
            SmallIcon = "icon_small",
            LargeIcon = "icon_large",

        };
        if (PlayerPrefs.GetInt("isNotificationActive") == 1)
        {
            AndroidNotificationCenter.SendNotificationWithExplicitID(_notification, channel2.Id, id);

        }
    }
    public static void SendNewRepeatedNotification2(string title, string text, double firetime, double repeatTime, int id)
    {
        AndroidNotification _notification = new AndroidNotification()
        {
            Title = title,
            Text = text,
            FireTime = DateTime.Now.AddSeconds(firetime),
            RepeatInterval = TimeSpan.FromSeconds(repeatTime),
            SmallIcon = "icon_small",
            LargeIcon = "icon_large",

        };
        if (PlayerPrefs.GetInt("isNotificationActive") == 1)
        {
            AndroidNotificationCenter.SendNotificationWithExplicitID(_notification, channel3.Id, id);

        }
    }
    public static void SendNewRepeatedNotification3(string title, string text, double firetime, double repeatTime, int id)
    {
        AndroidNotification _notification = new AndroidNotification()
        {
            Title = title,
            Text = text,
            FireTime = DateTime.Now.AddSeconds(firetime),
            RepeatInterval = TimeSpan.FromSeconds(repeatTime),
            SmallIcon = "icon_small",
            LargeIcon = "icon_large",

        };
        if (PlayerPrefs.GetInt("isNotificationActive") == 1)
        {
            AndroidNotificationCenter.SendNotificationWithExplicitID(_notification, channel4.Id, id);

        }
    }
    public static void SetNotificationActive(bool active)
    {
        if (active)
        {
            PlayerPrefs.SetInt("isNotificationActive", 1);
            PlayerPrefs.Save();

        }
        else
        {
            PlayerPrefs.SetInt("isNotificationActive", 0);
            PlayerPrefs.Save();

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
            SetNotificationActive(false);

        }
        else if (permissionRequest.Status == PermissionStatus.Allowed)
        {
            SetNotificationActive(true);

        }
        IsPermissionPending = false;
    }
    public static void SendTestNotification()
    {

        SendNewNotification("PROBANDO", "123 123", 10,0);
    }
    public static void SendTestNotification2()
    {
        SendNewRepeatedNotification("PROBANDO", "156 456", 60,120,1);
    }
    public static void SendTestNotification3()
    {

        SendNewRepeatedNotification("PROBANDO", "uygyuyhuinmoi 456", 60, 80,2);
    }


    public static void CancelNotifications()
    {
        AndroidNotificationCenter.CancelAllNotifications();
        AndroidNotificationCenter.CancelAllScheduledNotifications();

    }
    public static void NotitficationTest3()
    {
        NotificationsController.SendTestNotification2();
    }
    public static void NotitficationTest4()
    {
        NotificationsController.SendTestNotification3();
    }

}

