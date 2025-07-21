using System.Collections;
using System.Collections.Generic;
using Managers.Menu;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdsManager : MonoBehaviour, IUnityAdsInitializationListener, IUnityAdsShowListener, IUnityAdsLoadListener
{
    [SerializeField] private string _gameID = "5740305";
    private string _adID = "Interstitial_Android";

    private void Start()
    {
        Advertisement.Initialize(_gameID, false, this);
    }

    public void OnInitializationComplete()
    {
        Advertisement.Load(_adID);
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.LogWarning(message);
    }

    public void ShowAdd()
    {
        if (!Advertisement.isInitialized)
        {
            Debug.LogWarning("Ad no est� listo.");
            return;
        }
        if (MenuManager.instance.HasFullLife()) return;

        Advertisement.Show(_adID, this);
    }

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        Debug.Log("Ad Errors");
    }

    public void OnUnityAdsShowStart(string placementId)
    {
        Debug.Log("Ad Start");
    }

    public void OnUnityAdsShowClick(string placementId)
    {
        Debug.Log("Ad Click");
    }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        if(showCompletionState == UnityAdsShowCompletionState.COMPLETED)
        {
            MenuManager.instance.AddLife();
            Debug.Log("A�ado stamina");
        }
        else
        {
            Debug.Log("No te doy nada");
        }

        Advertisement.Load(_adID);

    }

    public void OnUnityAdsAdLoaded(string placementId)
    {
        Debug.Log($"Ad Loaded: {placementId}");
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        Debug.LogError($"Failed to load ad {placementId}: {message}");
    }
}
