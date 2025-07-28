using UnityEngine;
using UnityEngine.EventSystems;

public class Bin : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Sprite normalSprite;
    public Sprite highlightSprite;
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        if (_spriteRenderer && normalSprite)
            _spriteRenderer.sprite = normalSprite;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag && eventData.pointerDrag.GetComponent<Plato>())
        {
            var p = eventData.pointerDrag.GetComponent<Plato>();
            p.place.SetFree();
            FoodFactory.Instance.ReturnFood(p);
            Debug.Log("Comida tirada!");
            
            if (_spriteRenderer && normalSprite)
                _spriteRenderer.sprite = normalSprite;
        }
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.pointerDrag && eventData.pointerDrag.GetComponent<Plato>())
        {
            if (_spriteRenderer && highlightSprite)
                _spriteRenderer.sprite = highlightSprite;
        }
    }
    
    public void OnPointerExit(PointerEventData eventData)
    {
        if (_spriteRenderer && normalSprite)
            _spriteRenderer.sprite = normalSprite;
    }
}