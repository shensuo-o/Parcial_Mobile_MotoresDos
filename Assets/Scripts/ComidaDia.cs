using UnityEngine;

public class ComidaDia : Plato
{
    public Sprite[] sprites;

    public override void Start()
    {
        this.GetComponent<Renderer>().sortingLayerName = "Default";
        this.GetComponent<Renderer>().sortingOrder = 4;
        Initialize();
    }

    public void Initialize()
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
        foodName = RemoteConfigTest.instance.foodName;

        spriteRenderer.sprite = sprites[RemoteConfigTest.instance.spriteNumber];
    }

    private void OnDestroy()
    {
        RemoteConfigTest.instance.OnConfigFetched -= InitializeVariables;
    }
}
