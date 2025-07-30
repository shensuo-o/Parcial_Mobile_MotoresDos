using UnityEngine;

namespace Gameplay
{
    public class Plato : MonoBehaviour
    {
        public string foodName = "";
        public Seat place;

        public float timeToCook;
        public float timeToFreeze;
        public bool isFrozen;
		public bool onClientHands;
        public Sprite defaultSprite;
        public Sprite frozenSprite;

        private float _timeSinceCooked;

        public int price;

        public Vector3 lastPosition = Vector3.zero;
        public SpriteRenderer spriteRenderer;


        public virtual void Start()
        {
            _timeSinceCooked = 0f;
            isFrozen = false;
            GetComponent<Renderer>().sortingLayerName = "Default";
            GetComponent<Renderer>().sortingOrder = 4;
            spriteRenderer = GetComponent<SpriteRenderer>();
            if (defaultSprite && spriteRenderer)
                spriteRenderer.sprite = defaultSprite;
        }

        private void Update()
        {
            if (!isFrozen && !onClientHands)
            {
                _timeSinceCooked += Time.deltaTime;
                if (_timeSinceCooked >= timeToFreeze)
                {
                    Freeze();
                }
            }
        }

        private void Freeze()
        {
            isFrozen = true;
            if (frozenSprite && spriteRenderer)
                spriteRenderer.sprite = frozenSprite;
        }

        public void MoveTo(Seat seat)
        {
            if(place) place.SetFree();

            place = seat;
            transform.position = place.transform.position;
            lastPosition = place.transform.position;
            place.SetUsed();
        }

        // ReSharper disable Unity.PerformanceAnalysis
        public static void TurnOnOff(Plato f, bool active = true)
        {
            if (active) f.Reset();
            f.gameObject.SetActive(active);
        }

        public void Reset()
        {
            _timeSinceCooked = 0f;
            isFrozen = false;
            onClientHands = false;
            spriteRenderer.sprite = defaultSprite;
            GetComponent<Collider2D>().enabled = true;
            lastPosition = Vector3.zero;
            place = null;
        }
    }
}