using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cocina : MonoBehaviour
{
    public Seat[] placesToPlaceOrders;
    public Stack<Plato> finishedFoods;

    private void Start()
    {
        placesToPlaceOrders = GetComponentsInChildren<Seat>();
    }

    private void Update()
    {
        if(finishedFoods != null)
        {
            if (CanPlaceOrders())
            {
                PlaceOrder();
            }
        }
    }

    public void GetOrder(Plato plato)
    {
        StartCoroutine(Cook(plato));
    }

    private IEnumerator Cook(Plato plato)
    {
        yield return new WaitForSeconds(plato.timeToCook);
        finishedFoods.Push(plato);
    }

    private void PlaceOrder()
    {
        foreach (Seat place in placesToPlaceOrders)
        {
            if (place.isFree())
            {
                Plato plato = finishedFoods.Pop();
                Instantiate(plato, place.transform.position, Quaternion.identity);
                place.ChangeStatus();
                continue;
            }
        }
    }

    private bool CanPlaceOrders()
    {
        foreach (Seat place in placesToPlaceOrders)
        {
            if (place.isFree())
            {
                return true;
            }
        }
        return false;
    }

    public Plato DeliverOrder(Plato plato)
    {
        plato.transform.position = plato.client.transform.position;
        return plato;
    }
}
