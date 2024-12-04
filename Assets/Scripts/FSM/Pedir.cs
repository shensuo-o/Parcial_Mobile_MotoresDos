using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pedir : IState
{
    private FSM _fsm;
    private Client _client;

    public Pedir(FSM fsm, Client client)
    {
        _fsm = fsm;
        _client = client;
    }

    public void OnEnter()
    {
        _client.seated = true;
        _client.GetComponent<Collider2D>().enabled = false;

        //activar dialogo y asignar sprite
        

        _client.soundManager.PlaySFX(_client.soundManager.askFood);
        if (_client.co != null)
        {
            _client.EndCoroutine(); // Stop any previous coroutine
        }
        
        _client.selectedFood = FoodFactory.Instance.foodPrefabs[Random.Range(0, FoodFactory.Instance.foodPrefabs.Length)];

        if (_client.selectedFood.GetComponent<ComidaDia>() != null)
        {
            Debug.Log(_client.selectedFood.foodName);
        }

        _client.dialogo.SetActive(true);
        _client.dialogo.GetComponentInChildren<ComidaEstetica>().SetSprite(_client.selectedFood.spriteRenderer.sprite);

        Debug.Log("Pido un: " + _client.selectedFood.foodName);
        _client.StartWaitFood();
        Debug.Log("Espero la comida loco");
        GameManager.instance.NewOrder(_client.selectedFood);
    }

    public void OnUpdate()
    {

    }

    public void OnExit()
    {
        _client.EndCoroutine();
    }
}
