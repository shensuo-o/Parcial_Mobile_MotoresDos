using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class Entrada : MonoBehaviour
{
    public Seat[] placesToStand;

    public bool PlaceAvailible()
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
