using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoAway : MonoBehaviour
{
    void Start()
    {
        Destroy(this.gameObject, 3);
    }
}
