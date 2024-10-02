using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barra : MonoBehaviour
{
    
    public Seat[] seats;

    public List<Client> clients;

    public int cuenta;

    public GameObject coin;

    private void Start()
    {
        coin.gameObject.SetActive(false);
        seats = GetComponentsInChildren<Seat>();
    }

    public bool SpaceAvailable()
    {
        foreach (Seat seat in seats)
        {
            if (seat.isFree())
            {
                return true;
            }
        }
        return false;
    }

    public void GetClientToPosition(Client client)
    {
        if(!client.seated)
        {
            foreach (Seat seat in seats)
            {
                if (seat.isFree())
                {
                    client.GoToSeat(seat, this);
                    break;
                }
            }
        }
    }

    public bool MoneyOnTable()
    {
        return cuenta > 0 ? true : false;
    }

    public void GetMoney()
    {
        GameManager.instance.GetBarMoney(cuenta);
    }

    public void ClearMoney()
    {
        cuenta = 0;
        ActivateCoin(false);
    }

    public void AddMoneyToTable(int d)
    {
        cuenta += d;
        ActivateCoin(true);
    }

    public void ActivateCoin(bool v)
    {
        coin.gameObject.SetActive(v);
    }
}

