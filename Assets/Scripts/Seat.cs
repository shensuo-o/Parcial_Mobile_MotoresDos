using UnityEngine;

public class Seat : MonoBehaviour
{
    private bool _free = true;

    public bool IsFree()
    {
        return _free;
    }

    public void SetUsed()
    {
        _free = false;
    }

    public void SetFree()
    {
        _free = true;
    }
}
