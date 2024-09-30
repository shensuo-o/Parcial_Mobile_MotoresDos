using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenManager : MonoBehaviour
{
    //TODO LO QUE SE CONFIGURE DE LA PANTALLA LO PONEMOS ACÁ

    public ScreenManager instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
    }
}
