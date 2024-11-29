using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdsManager : MonoBehaviour, IUnityAdsInitializationListener, IUnityAdsShowListener
{
    [SerializeField] private string _gameID = "5740305";
    [SerializeField] private string _adID = "Rewarded_Android";

    private void Start()
    {
        Advertisement.Initialize(_gameID, true, this);
    }

    public void OnInitializationComplete()
    {
        Advertisement.Load(_adID);
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        throw new System.NotImplementedException();
    }

    public void ShowAdd()
    {
        if (!Advertisement.isInitialized) return;
        if (MenuManager.instance.HasFullLife()) return;

        Advertisement.Show(_adID, this);
    }

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        
    }

    public void OnUnityAdsShowStart(string placementId)
    {
        Debug.Log("StartAd");
    }

    public void OnUnityAdsShowClick(string placementId)
    {
        
    }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        if(showCompletionState == UnityAdsShowCompletionState.COMPLETED)
        {
            MenuManager.instance.AddLife();
            Debug.Log("Añado stamina");
        }
        else
        {
            Debug.Log("No te doy nada");
        }

        Advertisement.Load(_adID);

    }
}
