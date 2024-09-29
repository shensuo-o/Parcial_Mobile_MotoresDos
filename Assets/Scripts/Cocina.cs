using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cocina : MonoBehaviour
{
    public Seat[] placesToPlaceOrders;
    public List<Plato> finishedFoods;

    private void Start()
    {
        placesToPlaceOrders = GetComponentsInChildren<Seat>();
    }

    private void Update()
    {
        if(finishedFoods.Count > 0)
        {
            if (CanPlaceOrders())
            {
                PlaceOrderOnCounter();
            }
        }
    }

    public void GetOrder(Plato plato)
    {
        StartCoroutine(Cook(plato));
    }

    private IEnumerator Cook(Plato plato)
    {
        Debug.Log("Cocinando: " + plato.name);
        yield return new WaitForSeconds(plato.timeToCook);
        Debug.Log("Listo: " + plato.name);
        finishedFoods.Add(plato);
    }

    private void PlaceOrderOnCounter()
    {
        foreach (Seat place in placesToPlaceOrders)
        {
            if (place.isFree())
            {
                Plato platoListo = finishedFoods[0];
                Plato nuevoPlato = FoodFactory.Instance.GetFood(platoListo);
                Debug.Log("Apoyo un " + nuevoPlato.name + " en la mesada!");
                nuevoPlato.SetClient(platoListo.client);
                nuevoPlato.MoveTo(place);
                finishedFoods.Remove(platoListo);
                break;
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
}
