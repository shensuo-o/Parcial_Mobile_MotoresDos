using System.Collections.Generic;
using Clients;
using Gameplay;
using Managers;
using UnityEngine;
using UnityEngine.EventSystems;

public class Barra : MonoBehaviour, IPointerClickHandler
{
    public Seat[] seats;

    public List<Client> clients;

    public int cuenta;

    public GameObject coin;

    public Material[] colors;

    public new Renderer renderer;
    private void Start()
    {
        coin.gameObject.SetActive(false);
        seats = GetComponentsInChildren<Seat>();
        renderer = GetComponent<Renderer>();

        for (int i = 0; i < colors.Length; i++)
        {
            if (PlayerPrefs.GetInt("BarColor") == i)
            {
                renderer.material = colors[i];
            }
        }
    }

    public bool SpaceAvailable()
    {
        foreach (Seat seat in seats)
        {
            if (seat.IsFree())
            {
                return true;
            }
        }
        return false;
    }

    public void GetClientToPosition(Client client)
    {
        if (!client.seated)
        {
            foreach (Seat seat in seats)
            {
                if (seat.IsFree())
                {
                    seat.SetUsed();
                    client.GoToSeat(seat, this);
                    client.DisableDrag();
                    
                    if (clients.Contains(client))
                        clients.Remove(client);

                    clients.Add(client);
                    client.seated = true;
                    break;
                }
            }
        }
    }

    private bool MoneyOnTable()
    {
        return cuenta > 0;
    }

    private void GetMoney()
    {
        GameManager.instance.GetBarMoney(cuenta);
    }

    private void ClearMoney()
    {
        cuenta = 0;
        ActivateCoin(false);
    }

    public void AddMoneyToTable(int d)
    {
        cuenta += d;
        ActivateCoin(true);
    }

    private void ActivateCoin(bool v)
    {
        coin.gameObject.SetActive(v);
    }

    public Client WhoOrderedThis(Plato p)
    {
        if (clients.Count == 0 || !p.enabled)
        {
            Debug.Log("No hay clientes");
            return null;
        }
        
        foreach (Client client in clients)
        {
            if (!client || !p)
            {
                return null;
            }

            if (client.selectedFood.foodName == p.foodName)
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
        if (!eventData.pointerDrag)
        {
            if (MoneyOnTable())
            {
                GetMoney();
                ClearMoney();
            }
        }
    }
}