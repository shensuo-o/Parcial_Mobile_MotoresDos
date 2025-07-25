using UnityEngine;

public class Plato : MonoBehaviour
{
    public string foodName = "";
    public Seat place;

    public float timeToCook;

    public int price;

    public Vector3 lastPosition = Vector3.zero;
    public SpriteRenderer spriteRenderer;

    public virtual void Start()
    {
        GetComponent<Renderer>().sortingLayerName = "Default";
        GetComponent<Renderer>().sortingOrder = 4;
        spriteRenderer = GetComponent<SpriteRenderer>();
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
        GetComponent<Collider2D>().enabled = true;
        lastPosition = Vector3.zero;
        place = null;
    }
}
