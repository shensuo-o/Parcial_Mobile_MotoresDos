using System.Collections;
using System.Collections.Generic;
using Audio;
using ObjectPool;
using UnityEngine;

namespace Gameplay
{
    public class Cocina : MonoBehaviour
    {
        public Seat[] placesToPlaceOrders;
        private Queue<Plato> _finishedFoods = new Queue<Plato>();

        SoundManager _soundManager;
        private void Start()
        {
            placesToPlaceOrders = GetComponentsInChildren<Seat>();
            _soundManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<SoundManager>();
        }

        private void Update()
        {
            if(_finishedFoods.Count > 0)
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
            _soundManager.PlaySfx(_soundManager.foodReady);
            _finishedFoods.Enqueue(plato);
        }

        private void PlaceOrderOnCounter()
        {
            foreach (Seat place in placesToPlaceOrders)
            {
                if (place.IsFree())
                {
                    Plato platoListo = _finishedFoods.Dequeue();
                    Plato nuevoPlato = FoodFactory.instance.GetFood(platoListo);
                    nuevoPlato.MoveTo(place);
                    break;
                }
            }
        }

        private bool CanPlaceOrders()
        {
            foreach (Seat place in placesToPlaceOrders)
            {
                if (place.IsFree())
                {
                    return true;
                }
            }
            return false;
        }
    }
}