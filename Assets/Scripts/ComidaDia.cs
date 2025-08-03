using Gameplay;
using UnityEngine;

public class ComidaDia : Plato
{
    public Sprite[] defaultSprites;
    public Sprite[] frozenSprites;

    public override void Start()
    {
        GetComponent<Renderer>().sortingLayerName = "Default";
        GetComponent<Renderer>().sortingOrder = 4;
        Initialize();
    }

    private void Initialize()
    {
        if (RemoteConfigTest.instance.isConfigFetched)
        {
            InitializeVariables();
        }
        else
        {
            RemoteConfigTest.instance.OnConfigFetched += InitializeVariables;
        }
    }

    void InitializeVariables()
    {
        price = RemoteConfigTest.instance.foodPrice;
        timeToCook = RemoteConfigTest.instance.cookTime;
        timeToFreeze = RemoteConfigTest.instance.freezeTime;
        foodName = RemoteConfigTest.instance.foodName;

        defaultSprite = defaultSprites[RemoteConfigTest.instance.spriteNumber];
        frozenSprite = frozenSprites[RemoteConfigTest.instance.spriteNumber];
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = defaultSprite;
    }

    private void OnDestroy()
    {
        RemoteConfigTest.instance.OnConfigFetched -= InitializeVariables;
    }
}
