using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DragAndDrop
{
    public class DragDrop : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerEnterHandler
    {
        public bool canDrag;
        public bool canDrop;
        public Vector3 lastPos = Vector3.zero;

        public void OnBeginDrag(PointerEventData eventData)
        {
            int layerIgnoreRaycast = LayerMask.NameToLayer("Ignore Raycast");
            gameObject.layer = layerIgnoreRaycast;
            GetInitialPosition();
        }

        public void OnDrag(PointerEventData eventData)
        {
            //Debug.Log(eventData.pointerDrag.gameObject);
            //if (!canDrag) return;
            if (Camera.main)
            {
                float zOnScreen = Camera.main.WorldToScreenPoint(transform.position).z;
                var position = Camera.main.ScreenToWorldPoint(new Vector3(eventData.position.x, eventData.position.y, zOnScreen));
                transform.position = position;
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            int layerObjects = LayerMask.NameToLayer("Objects");
            gameObject.layer = layerObjects;
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
}
