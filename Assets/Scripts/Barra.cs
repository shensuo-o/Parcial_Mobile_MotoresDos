using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Barra : MonoBehaviour, IPointerClickHandler
{
    public Seat[] seats;

    public List<Client> clients;

    public int cuenta;

    public GameObject coin;

    public Material[] colors;

    public Renderer Renderer;

    private void Start()
    {
        coin.gameObject.SetActive(false);
        seats = GetComponentsInChildren<Seat>();
        Renderer = GetComponent<Renderer>();

        for (int i = 0; i < colors.Length; i++)
        {
            if (PlayerPrefs.GetInt("BarColor") == i)
            {
                Renderer.material = colors[i];
            }
        }
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
                    client.DisableDrag();
                    clients.Add(client);
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

    public Client WhoOrderedThis(Plato p)
    {
        foreach (Client client in clients)
        {
            if(client.selectedFood.foodName == p.foodName)
            {
                return client;
            }
        }
        Debug.Log("Nadie pidió esto!");
        return null;
    }

    public void DeleteClient(Client client)
    {
        if (clients.Contains(client))
        {
            clients.Remove(client);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (MoneyOnTable())
        {
            GetMoney();
            ClearMoney();
        }
    }
}

