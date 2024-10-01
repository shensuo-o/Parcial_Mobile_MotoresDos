using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.RemoteConfig;
using Unity.Services.Core;
using Unity.Services.Authentication;
using System.Threading.Tasks;

public class RemoteConfigTest : MonoBehaviour
{
    public struct userAttributes { }
    public struct appAttributes { }

    public int spawnRate;
    public float cookTime;
    public string foodName;
    public int foodPrice;
    public int spriteNumber;
    public static RemoteConfigTest instance;


    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        StartProcess();
    }
    async void StartProcess()
    {
        if(Utilities.CheckForInternetConnection())
        {
           await InitializeRemoteConfig();
        }
        else 
        {
            
        }

        RemoteConfigService.Instance.FetchCompleted += Fetch;
        RemoteConfigService.Instance.FetchConfigs(new userAttributes(), new appAttributes());
    }

    async Task InitializeRemoteConfig()
    {
        await UnityServices.InitializeAsync();

        if (!AuthenticationService.Instance.IsSignedIn)
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }
    }

    void Fetch(ConfigResponse response)
    {
        spawnRate = RemoteConfigService.Instance.appConfig.config.Value<int>("SpawnRate");
        foodName = RemoteConfigService.Instance.appConfig.config.Value<string>("FofD_name");
        foodPrice = RemoteConfigService.Instance.appConfig.config.Value<int>("Price_FofD");
        cookTime = RemoteConfigService.Instance.appConfig.config.Value<float>("CookTime_FofD");
        spriteNumber = RemoteConfigService.Instance.appConfig.config.Value<int>("SpriteNumber_FofD");
    }
}
