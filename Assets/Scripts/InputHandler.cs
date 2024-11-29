using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public InputHandler instance;
    private Barra selectedBarra;

    private void Awake()
    {
        instance = this;
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
                    if (objectHit.collider.GetComponent<Barra>())
                    {
                        if (selectedBarra.MoneyOnTable())
                        {
                            selectedBarra.GetMoney();
                            selectedBarra.ClearMoney();
                        }
                    }
                }
            }
        }
    }
}
