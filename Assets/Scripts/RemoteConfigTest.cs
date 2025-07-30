using UnityEngine;
using Unity.Services.RemoteConfig;
using Unity.Services.Core;
using Unity.Services.Authentication;
using System.Threading.Tasks;
using System;

public class RemoteConfigTest : MonoBehaviour
{
    public static RemoteConfigTest instance;

    private struct UserAttributes { }

    private struct AppAttributes { }

    public int spawnRate;
    public float cookTime;
    public float freezeTime;
    public string foodName;
    public int foodPrice;
    public int spriteNumber;
    public bool isConfigFetched;

    public event Action OnConfigFetched;


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

        RemoteConfigService.Instance.FetchCompleted += Fetch;
        RemoteConfigService.Instance.FetchConfigs(new UserAttributes(), new AppAttributes());
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
        freezeTime = RemoteConfigService.Instance.appConfig.config.Value<float>("FreezeTime_FofD");
        spriteNumber = RemoteConfigService.Instance.appConfig.config.Value<int>("SpriteNumber_FofD");

        isConfigFetched = true;
        OnConfigFetched?.Invoke();
    }
}
