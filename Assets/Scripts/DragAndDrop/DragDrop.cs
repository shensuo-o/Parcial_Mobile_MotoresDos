using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragDrop : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerEnterHandler
{
    bool canDrag = false;
    public bool canDrop = false;
    Vector3 lastPos = Vector3.zero;

    public void OnBeginDrag(PointerEventData eventData)
    {
        int LayerIgnoreRaycast = LayerMask.NameToLayer("Ignore Raycast");
        gameObject.layer = LayerIgnoreRaycast;
        GetInitialPosition();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!canDrag) return;
        float zOnScreen = Camera.main.WorldToScreenPoint(transform.position).z;
        var position = Camera.main.ScreenToWorldPoint(new Vector3(eventData.position.x, eventData.position.y, zOnScreen));
        transform.position = position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        int LayerObjects = LayerMask.NameToLayer("Objects");
        gameObject.layer = LayerObjects;
        canDrag = false;
        if(!canDrop)
            transform.position = lastPos;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("OnPointerEnter");
    }

    async void GetInitialPosition()
    {
        try
        {
            Task<Vector3> getPos = GetPosition();
            await getPos;
            canDrag = true;
        }
        catch (Exception e)
        {
            Debug.Log($"error : {e.Message}");
        }
    }

    private async Task<Vector3> GetPosition()
    {
        await Task.CompletedTask;
        return lastPos = transform.position;
    }
}
