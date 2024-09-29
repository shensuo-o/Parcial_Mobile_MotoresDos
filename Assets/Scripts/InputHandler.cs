using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public InputHandler instance;
    private Client selectedClient;
    private Barra selectedBarra;
    private Plato selectedPlato;

    private void Awake()
    {
        instance = this;
    }

    public void SelectClient(Client client)
    {
        selectedClient = client;
        Debug.Log("Elegí un: " + client.name);
    }

    public void DeselectClient()
    {
        selectedClient = null;
    }

    public void SelectBarra(Barra barra)
    {
        selectedBarra = barra;
        Debug.Log("Elegí una: " + barra.name);
    }

    public void DeselectBarra()
    {
        selectedBarra = null;
    }
    public void SelectPlato(Plato plato)
    {
        selectedPlato = plato;
        Debug.Log("Elegí un: " + plato.name);
    }

    public void DeselectPlato()
    {
        selectedPlato = null;
    }

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                Vector2 fingerRay = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
                RaycastHit2D objectHit = Physics2D.Raycast(fingerRay, Vector2.zero);
                if (objectHit)
                {
                    //We hit something
                    if (objectHit.collider.GetComponent<Client>() && objectHit.collider.GetComponent<Client>().gameObject.activeSelf)
                    {
                        SelectClient(objectHit.collider.GetComponent<Client>());
                    }
                    if (objectHit.collider.GetComponent<Barra>())
                    {
                        SelectBarra(objectHit.collider.GetComponent<Barra>());
                        if(selectedClient != null && selectedClient.gameObject.activeSelf && !selectedClient.seated)
                        {
                            if (selectedBarra.SpaceAvailable())
                            {
                                selectedClient.asiento.ChangeStatus();
                                selectedBarra.GetClientToPosition(selectedClient);
                            }
                            DeselectClient();
                            DeselectBarra();
                        }
                        else
                        {
                            DeselectClient();
                            if(selectedBarra.MoneyOnTable())
                            {
                                selectedBarra.GetMoney();
                                selectedBarra.ClearMoney();
                            }
                            DeselectBarra();
                        }
                    }
                    if (objectHit.collider.GetComponent<Plato>() && objectHit.collider.GetComponent<Plato>().gameObject.activeSelf)
                    {
                        SelectPlato(objectHit.collider.GetComponent<Plato>());
                        if (selectedPlato.client.gameObject.activeSelf)
                        {
                            GameManager.instance.DeliverOrder(selectedPlato);
                        }
                        else
                        {
                            FoodFactory.Instance.ReturnFood(selectedPlato);
                            DeselectPlato();
                        }
                    }
                }
            }
        }
    }
}
