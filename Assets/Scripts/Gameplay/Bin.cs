using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using Gameplay;
using ObjectPool;

public class Bin : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Sprite normalSprite;
    public Sprite highlightSprite;
    public Sprite[] throwSprites;
    private SpriteRenderer _spriteRenderer;
    public AudioSource audiosource;
    public AudioClip abreTacho;
    public AudioClip tiraAlgo;


    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        if (_spriteRenderer && normalSprite)
            _spriteRenderer.sprite = normalSprite;

        audiosource = GetComponent<AudioSource>();
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag && eventData.pointerDrag.GetComponent<Plato>())
        {
            var p = eventData.pointerDrag.GetComponent<Plato>();
            p.place.SetFree();
            FoodFactory.instance.ReturnFood(p);
            Debug.Log("Comida tirada!");
            audiosource.PlayOneShot(tiraAlgo);

            StartCoroutine(ThrowAnimation(0.016f));
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.pointerDrag && eventData.pointerDrag.GetComponent<Plato>())
        {
            if (_spriteRenderer && highlightSprite)
            {
                _spriteRenderer.sprite = highlightSprite;
                audiosource.PlayOneShot(abreTacho);
            }
        }
    }
    
    public void OnPointerExit(PointerEventData eventData)
    {
        if (_spriteRenderer && normalSprite)
        {
            _spriteRenderer.sprite = normalSprite;
        }
    }

    private IEnumerator ThrowAnimation(float frameTime = 0.1f)
    {
        if (!_spriteRenderer) yield break;

        foreach (var sprite in throwSprites)
        {
            if (sprite)
            {
                _spriteRenderer.sprite = sprite;
                yield return new WaitForSeconds(frameTime);
            }
        }

        _spriteRenderer.sprite = normalSprite;
    }
}