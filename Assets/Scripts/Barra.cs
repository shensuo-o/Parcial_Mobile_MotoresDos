using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barra : MonoBehaviour
{
    public Seat[] seats;

    public List<Client> clients;

    public int cuenta;

    private void GetClientToPosition(Client client)
    {
        foreach (Seat seat in seats)
        {
            if (seat.isFree())
            {
                client.transform.position = seat.transform.position;
                client.barraAsignada = this;
                seat.ChangeStatus();
                client.Seated();
                continue;
            }
        }
        //Deseleccionar el cliente
    }

    public void AddToTable(int d)
    {
        cuenta += d;
    }
}

