using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class Client : MonoBehaviour
{
    public int RandomComida;
    public int RandomBebida;

    public string comidaElegida;
    public string bebidaElegida;

    public float timerEntrada;
    public float timerPedido;
    public float timerComida;
    public float timerConsume;
    public float timerPago;

    public State state;

    // Start is called before the first frame update
    void Start()
    {
        RandomComida = Random.Range(1, 4);
        RandomBebida = Random.Range(1, 4);

        switch(RandomComida)
        {
            case 1:
                comidaElegida = "Pollo";
                break;

            case 2:
                comidaElegida = "Fideos";
                break;

            case 3:
                comidaElegida = "Milanesa";
                break;

            default:
                break;
        }

        switch (RandomBebida)
        {
            case 1:
                bebidaElegida = "Coca";
                break;

            case 2:
                bebidaElegida = "Agua";
                break;

            case 3:
                bebidaElegida = "Cerveza";
                break;

            default:
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case State.Entrada:
                RunTimer(timerEntrada);
                break;
        }
    }

    public enum State
    {
        Entrada,
        Pedido,
        Comida,
        Consume,
        Pago
    }

    void RunTimer(float TimeLimit)
    {

    }
}
