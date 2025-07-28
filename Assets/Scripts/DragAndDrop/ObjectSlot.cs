using Managers;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DragAndDrop
{
    public class ObjectSlot : MonoBehaviour, IDropHandler
    {
        private Barra _bar;

        private void Awake()
        {
            _bar = GetComponent<Barra>();
        }

        public void OnDrop(PointerEventData eventData)
        {
            Debug.Log("Drop " + name);
            if (eventData.pointerDrag)
            {
                if (eventData.pointerDrag.GetComponent<Client>())
                {
                    var c = eventData.pointerDrag.GetComponent<Client>();
                    if (_bar.SpaceAvailable())
                    {
                        eventData.pointerDrag.GetComponent<DragDrop>().canDrop = true;
                        
                        if (c.clientSeat)
                        {
                            c.clientSeat.SetFree();
                            c.clientSeat = null;
                            c.seated = false;
                        }

                        _bar.GetClientToPosition(c);
                    }
                    else
                    {
                        eventData.pointerDrag.GetComponent<DragDrop>().canDrop = false;
                    }
                }

                if (eventData.pointerDrag.GetComponent<Plato>())
                {
                    var p = eventData.pointerDrag.GetComponent<Plato>();
                    Debug.Log("Yo ordene " + p.foodName);
                    if (_bar.WhoOrderedThis(p))
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
}