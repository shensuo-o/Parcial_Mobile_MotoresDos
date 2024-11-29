using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragDrop : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerEnterHandler
{

    public void OnBeginDrag(PointerEventData eventData)
    {
        int LayerIgnoreRaycast = LayerMask.NameToLayer("Ignore Raycast");
        gameObject.layer = LayerIgnoreRaycast;
    }

    public void OnDrag(PointerEventData eventData)
    {
        float zOnScreen = Camera.main.WorldToScreenPoint(transform.position).z;
        var position = Camera.main.ScreenToWorldPoint(new Vector3(eventData.position.x, eventData.position.y, zOnScreen));
        transform.position = position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        int LayerObjects = LayerMask.NameToLayer("Objects");
        gameObject.layer = LayerObjects;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("OnPointerEnter");
    }
}
