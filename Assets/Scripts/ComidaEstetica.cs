using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComidaEstetica : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Plato comidaSeleccionada;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = comidaSeleccionada.spriteRenderer.sprite;
    }
}
