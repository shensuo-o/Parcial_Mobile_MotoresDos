using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComidaDia : Plato
{
    public string foodName;
    public Sprite[] sprites;
    public SpriteRenderer spriteRenderer;
    public override void Start()
    {
        this.GetComponent<Renderer>().sortingLayerName = "Default";
        this.GetComponent<Renderer>().sortingOrder = 4;

        if(RemoteConfigTest.instance.isConfigFetched)
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
}
