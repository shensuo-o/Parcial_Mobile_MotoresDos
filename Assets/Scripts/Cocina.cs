using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cocina : MonoBehaviour
{
    public Seat[] placesToPlaceOrders;
    public List<Plato> finishedFoods;

    SoundManager soundManager;
    private void Start()
    {
        placesToPlaceOrders = GetComponentsInChildren<Seat>();
        soundManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<SoundManager>();
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
        Debug.Log("Cocinando: " + plato.foodName);
        yield return new WaitForSeconds(plato.timeToCook);
        Debug.Log("Listo: " + plato.foodName);
        soundManager.PlaySFX(soundManager.foodReady);
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
