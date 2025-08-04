using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Managers.Menu;


public class SettingsController : MonoBehaviour
{
    [SerializeField] Slider volumeSlider;
    public bool isAccelerometerActive;
    public bool isVibrationActive;
    public bool isNotificationActive;
    [Header("SettingsButtons")]
    public Image AccelerometerImage;
    public Image NotificationImage;
    public Image VibrationImage;
    public Sprite AcceptImage;
    public Sprite NotAcceptImage;
    void Awake()
    {
        if (!PlayerPrefs.HasKey("musicVolume"))
        {
            PlayerPrefs.SetFloat("musicVolume", 1);
            PlayerPrefs.SetInt("isVibrationActive", 1);
            if(SystemInfo.supportsAccelerometer)
            PlayerPrefs.SetInt("isAcceleratorActive", 1);
            NotificationsController.SetNotificationActive(true);
            NotificationsController.NotificationStart();
            NotificationPermissionAsk();
            Load();

        }

        else
        {
            Load();
        }
    }
    public void ResetAllData()
    {
        AudioListener.volume = 1;
        volumeSlider.value = 1;
        if (SystemInfo.supportsAccelerometer)
        {
            PlayerPrefs.SetInt("isAcceleratorActive", 1);
            isAccelerometerActive = true;
            AccelerometerImage.sprite = AcceptImage;

        }
        NotificationsController.SetNotificationActive(true);
        PlayerPrefs.SetInt("isVibrationActive", 1);
        isVibrationActive = true;
        VibrationImage.sprite = AcceptImage;
        isNotificationActive = true;
        NotificationImage.sprite = AcceptImage;
    }
    public void ChangeVolume()
    {
        AudioListener.volume = volumeSlider.value;
        Save();
    }

    public void EnableVibration()
    {
        if (!isVibrationActive)
        {
            isVibrationActive = true;
            if (MenuManager.instance != null)
            MenuManager.instance.PlaySound("SFX_UI_Confirm");
            PlayerPrefs.SetInt("isVibrationActive", 1);
            PlayerPrefs.Save();

            VibrationImage.sprite = AcceptImage;
        }
        else
        {
            isVibrationActive = false;
            if (MenuManager.instance != null)

                MenuManager.instance.PlaySound("SFX_UI_Exit");
            PlayerPrefs.SetInt("isVibrationActive", 0);
            PlayerPrefs.Save();

            VibrationImage.sprite = NotAcceptImage;
        }
    }
    public void NotificationPermissionAsk()
    {
        if(!NotificationsController.IsPermissionPending)
        StartCoroutine(NotificationRoutine());

    }
    public IEnumerator NotificationRoutine()
    {
        IEnumerator enumerator = NotificationsController.NotificationPermission();
        StartCoroutine(enumerator);
        yield return new WaitForEndOfFrame();
        while (NotificationsController.IsPermissionPending)
        {
            yield return null;
        }
        if (PlayerPrefs.GetInt("isNotificationActive") == 1)
        {
            isNotificationActive = true;
            NotificationImage.sprite = AcceptImage;
            MenuManager.instance.PlaySound("SFX_UI_Confirm");

        }
        else
        {
            isNotificationActive = false;
            NotificationImage.sprite = NotAcceptImage;
            MenuManager.instance.PlaySound("SFX_UI_Exit");

        }
    }
    public void EnableNotifications()
    {
        if (PlayerPrefs.GetInt("isNotificationActive") == 1)
        {
            NotificationsController.SetNotificationActive(false);
            isNotificationActive = false;
            NotificationImage.sprite = NotAcceptImage;
            MenuManager.instance.PlaySound("SFX_UI_Exit");
        }
        else
        {
            NotificationsController.SetNotificationActive(true);
            isNotificationActive = true;
            NotificationImage.sprite = AcceptImage;
            MenuManager.instance.PlaySound("SFX_UI_Confirm");

        }
    }
    public void EnableAccelerator()
    {
        if (!SystemInfo.supportsAccelerometer)
        {
            isAccelerometerActive = false;
            AccelerometerImage.sprite = NotAcceptImage;
            PlayerPrefs.SetInt("isAcceleratorActive", 0);
            PlayerPrefs.Save();

            return;
        }
        else
        {
            if (!isAccelerometerActive)
            {
                isAccelerometerActive = true;
                PlayerPrefs.SetInt("isAcceleratorActive", 1);
                PlayerPrefs.Save();
                if (MenuManager.instance != null)

                    MenuManager.instance.PlaySound("SFX_UI_Confirm");
                AccelerometerImage.sprite = AcceptImage;
            }
            else
            {
                isAccelerometerActive = false;
                if(MenuManager.instance!=null)
                    if (MenuManager.instance != null)

                        MenuManager.instance.PlaySound("SFX_UI_Exit");
                PlayerPrefs.SetInt("isAcceleratorActive", 0);
                PlayerPrefs.Save();

                AccelerometerImage.sprite = NotAcceptImage;
            }
        }
    }
    private void Load()
    {
        volumeSlider.value = PlayerPrefs.GetFloat("musicVolume");
        if (!SystemInfo.supportsAccelerometer || PlayerPrefs.GetInt("isAcceleratorActive") != 0)
        {
            isAccelerometerActive = false;
            AccelerometerImage.sprite = NotAcceptImage;
        }
        if (PlayerPrefs.GetInt("isAcceleratorActive") == 1)
        {
            isAccelerometerActive = true;
            AccelerometerImage.sprite = AcceptImage;
        }
        else
        {
            isAccelerometerActive = false;
            AccelerometerImage.sprite = NotAcceptImage;
        }
        if (PlayerPrefs.GetInt("isVibrationActive") == 1)
        {
            isVibrationActive = true;
            VibrationImage.sprite = AcceptImage;
        }
        else
        {
            isVibrationActive = false;
            VibrationImage.sprite = NotAcceptImage;
        }
        if (PlayerPrefs.GetInt("isNotificationActive") == 1)
        {
            isNotificationActive = true;
            NotificationImage.sprite = AcceptImage;
        }
        else
        {
            isNotificationActive = false;
            NotificationImage.sprite = NotAcceptImage;
        }
    }
    private void Save()
    {
        PlayerPrefs.SetFloat("musicVolume", volumeSlider.value);
        PlayerPrefs.Save();
    }
}