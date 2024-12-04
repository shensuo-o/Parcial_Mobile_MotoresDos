using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComidaEstetica : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetSprite(Sprite comidaSeleccionada)
    {
        
        spriteRenderer.sprite = comidaSeleccionada;
    }
}
