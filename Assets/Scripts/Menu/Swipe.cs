using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swipe : MonoBehaviour
{
    [SerializeField] Camera touchCamera;
    private Vector3 startPos;
    [SerializeField] private float minSwipeSlideX = 1.5f;
    public GameObject cubo;

    void Update()
    {
        HandleSwipe();
    }

    void HandleSwipe()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.touches[0];
            //Finger position at all time
            Vector3 realWorldPos = touchCamera.ScreenToWorldPoint(touch.position);

            switch (touch.phase)
            {
                case TouchPhase.Began:

                    startPos = touchCamera.ScreenToWorldPoint(touch.position);

                    break;
                    
                /*case TouchPhase.Ended:

                    float swipeHorizontalValue = (new Vector3(realWorldPos.x, 0, 0) - new Vector3(startPos.x, 0, 0)).magnitude;

                    if (swipeHorizontalValue > minSwipeSlideX)
                    {
                        float swipeVaule = Mathf.Sign(realWorldPos.x - startPos.x);

                        if (swipeVaule > 0)
                        {
                            Debug.Log("swipe RIGHT");
                            cubo.transform.position = cubo.transform.position + Vector3.right * (2 * swipeHorizontalValue);
                        }
                        else if (swipeVaule < 0)
                        {
                            Debug.Log("swipe LEFT");
                            cubo.transform.position = cubo.transform.position + Vector3.left * (2 * swipeHorizontalValue);
                        }
                    }

                    break;*/

                case TouchPhase.Moved:

                    float swipeHorizontalValue = (new Vector3(realWorldPos.x, 0, 0) - new Vector3(startPos.x, 0, 0)).magnitude;

                    if (swipeHorizontalValue > minSwipeSlideX)
                    {
                        float swipeVaule = Mathf.Sign(realWorldPos.x - startPos.x);

                        if (swipeVaule > 0)
                        {
                            Debug.Log("swipe RIGHT");
                            cubo.transform.position = cubo.transform.position + Vector3.right * (1.5f * swipeHorizontalValue);
                        }
                        else if (swipeVaule < 0)
                        {
                            Debug.Log("swipe LEFT");
                            cubo.transform.position = cubo.transform.position + Vector3.left * (1.5f * swipeHorizontalValue);
                        }
                    }

                    break;

                default:
                    break;
            }
        }
    }
}
