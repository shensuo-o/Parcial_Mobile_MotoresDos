using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seat : MonoBehaviour
{
    public bool free = true;

    public bool isFree()
    {
        return free;
    }

    public void ChangeStatus()
    {
        free = !free;
    }

    public void SetFree()
    {
        free = true;
    }
}
