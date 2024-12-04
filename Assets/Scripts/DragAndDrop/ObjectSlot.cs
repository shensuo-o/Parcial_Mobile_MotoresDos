using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ObjectSlot : MonoBehaviour, IDropHandler
{
    private Barra _bar;

    private void Awake()
    {
        _bar = GetComponent<Barra>();
    }

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("Drop " + this.name);
        if (eventData.pointerDrag != null)
        {
            if (eventData.pointerDrag.GetComponent<Client>() != null)
            {
                var c = eventData.pointerDrag.GetComponent<Client>();
                if (_bar.SpaceAvailable())
                {
                    eventData.pointerDrag.GetComponent<DragDrop>().canDrop = true;
                    c.clientSeat.SetFree();
                    _bar.GetClientToPosition(c);
                }
                else
                {
                    eventData.pointerDrag.GetComponent<DragDrop>().canDrop = false;
                }
            }

            if (eventData.pointerDrag.GetComponent<Plato>() != null)
            {
                var p = eventData.pointerDrag.GetComponent<Plato>();
                Debug.Log("Yo ordene " + p.foodName);
                if (_bar.WhoOrderedThis(p) != null)
                {
                    eventData.pointerDrag.GetComponent<DragDrop>().canDrop = true;
                    GameManager.instance.DeliverOrder(_bar.WhoOrderedThis(p), p, _bar);
                }
                else
                {
                    eventData.pointerDrag.GetComponent<DragDrop>().canDrop = false;
                }
            }
        }
    }
}
