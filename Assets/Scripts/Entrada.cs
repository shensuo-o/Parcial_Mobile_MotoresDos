using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class Entrada : MonoBehaviour
{
    private Seat[] placesToStand;

    private void Start()
    {
        placesToStand = GetComponentsInChildren<Seat>();
        foreach (var place in placesToStand)
        {
            place.free = true;
        }
    }

    public void SpawnClient(Client client)
    {
        foreach (var place in placesToStand)
        {
            if (place.isFree())
            {
                client.GoToSeat(place);
                break;
            }
        }
    }

    public bool PlaceAvailable()
    {
        foreach (Seat seat in placesToStand)
        {
            if (seat.isFree())
            {
                return true;
            }
        }
        return false;
    }
}
