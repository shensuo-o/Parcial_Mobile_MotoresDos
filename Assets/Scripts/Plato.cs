using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

public class Plato : MonoBehaviour
{
    public Seat place = null;

    public int timeToCook;

    public int price;

    public Client client;

    private void Start()
    {
        this.GetComponent<Renderer>().sortingLayerName = "Default";
        this.GetComponent<Renderer>().sortingOrder = 4;
    }

    public void SetClient(Client c)
    {
        if(client == null)
        {
            client = c;
        }
    }

    public void MoveTo(Seat seat)
    {
        if(place != null)
        {
            place.ChangeStatus();
        }
        place = seat;
        transform.position = place.transform.position;
        place.ChangeStatus();
    }

    public static void TurnOnOff(Plato f, bool active = true)
    {
        if (active) f.Reset();
        f.gameObject.SetActive(active);
    }

    public void Reset()
    {
        place = null;
        client = null;
    }
}
