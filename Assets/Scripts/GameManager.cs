using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int dinero = 0;
    public static GameManager instance;
    public Cocina cocina;
    public List<Barra> barras;
    public Plato[] menu;
    public Entrada entrada;

    public bool isAlive;

    public int lives = 3;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        
    }

    public void CrearPedido(Plato plato)
    {
        cocina.GetOrder(plato);
    }

    public void EnviarPedido(Plato plato)
    {
        plato.client.GetFood();
    }

    public void JuntarPlata(int d)
    {
        dinero += d;
    }
}
