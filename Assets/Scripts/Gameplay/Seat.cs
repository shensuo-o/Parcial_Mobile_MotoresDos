using UnityEngine;

public class Seat : MonoBehaviour
{
    [SerializeField] private bool free = true;

    public bool IsFree()
    {
        return free;
    }

    public void SetUsed()
    {
        free = false;
    }

    public void SetFree()
    {
        free = true;
    }
}
