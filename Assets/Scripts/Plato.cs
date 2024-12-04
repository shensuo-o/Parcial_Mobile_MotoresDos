using UnityEngine;

public class Plato : MonoBehaviour
{
    public string foodName = "";
    public Seat place = null;

    public float timeToCook;

    public int price;

    public Vector3 lastPosition = Vector3.zero;
    public SpriteRenderer spriteRenderer;

    public virtual void Start()
    {
        this.GetComponent<Renderer>().sortingLayerName = "Default";
        this.GetComponent<Renderer>().sortingOrder = 4;
        spriteRenderer = this.GetComponent<SpriteRenderer>();
    }

    public void MoveTo(Seat seat)
    {
        if(place != null) place.SetFree();

        place = seat;
        transform.position = place.transform.position;
        lastPosition = place.transform.position;
        place.SetUsed();
    }

    public static void TurnOnOff(Plato f, bool active = true)
    {
        if (active) f.Reset();
        f.gameObject.SetActive(active);
    }

    public void Reset()
    {
        this.GetComponent<Collider2D>().enabled = true;
        lastPosition = Vector3.zero;
        place = null;
    }
}
