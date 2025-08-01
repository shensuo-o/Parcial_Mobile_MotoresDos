using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;
using TMPro;
public class ShakeSystem : MonoBehaviour , IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public float shakeDetectionThreshold = 0.2f; // Umbral de detección de sacudidas
    public float shakeDetectionDuration = 0.5f; // Duración para considerar una sacudida
    public float cooldownTime = 1.0f; // Tiempo de espera para evitar múltiples detecciones
    bool _isReseting = false;
    bool _isOpen = false;
    private bool isShaking = false;
    public TextMeshProUGUI pro;
    public TextMeshProUGUI pro2;
    public static ShakeSystem instance;
    // Evento para notificar cuando se detecta una sacudida
    public event Action OnSuccess;
    public event Action OnResetDetected;
    public event Action OnShakeDetected;
    public bool isTouching = false; // Bandera para saber si el dedo sigue presionado
    private Vector2 touchStartPos; // Posición inicial del toque
    public float minSwipeDistance = 150f;
    public float failSwipeDistance = 80f;
    //bool isAccelerometerActive;
    //bool isVibrationActive;
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        print("hola");
        //PlayerPrefs.SetInt("isAcceleratorActive", 1);
        //PlayerPrefs.SetInt("isVibrationActive", 1);
        //if (!SystemInfo.supportsAccelerometer)
        //    isAccelerometerActive = false;
        //else if (PlayerPrefs.GetInt("isAcceleratorActive") != 0)
        //    isAccelerometerActive = true;
        //else
        //    isAccelerometerActive = false;
        //if (PlayerPrefs.GetInt("isVibrationActive") == 0)
        //    isVibrationActive = true;
        //else
        //    isVibrationActive = false;
    }
    void Update()
    {
        if (_isOpen)
            return;
        int orientationModifier=-1;
        float acceleration = Input.acceleration.x;
        
        if(Screen.orientation == ScreenOrientation.LandscapeRight)
        {
            orientationModifier = -1;
        }
        else if (Screen.orientation == ScreenOrientation.LandscapeLeft)
        {
            orientationModifier = 1;
        }
        if (PlayerPrefs.GetInt("isAcceleratorActive") ==0|| !SystemInfo.supportsAccelerometer)
            return;
        if (acceleration * orientationModifier< -0.3f && !isTouching)
        {
            if (!isShaking)
                SetShake();
        }
        else if (acceleration * orientationModifier > 0.8f &&isShaking && !isTouching)
        {
            SetOpen();
        }
        else if(isShaking && !isTouching)
        {
            if(!_isReseting)
            StartCoroutine(ResetRoutine(1f));
        }

        pro.text = Input.acceleration.x.ToString();

    }
    public void SetOpen()
    {
        _isOpen = true;
        StopAllCoroutines();
        OnSuccess.Invoke();
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (_isOpen)
            return;
        isTouching = true;
        touchStartPos = eventData.position; // Guarda la posición inicial del toque
        SetShake();
        print(2);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!eventData.pointerDrag)
        {

        }
        if (isTouching)
        {
            isTouching = false;
            Vector2 touchEndPos = eventData.position;
            Vector2 swipeVector = touchEndPos - touchStartPos;

            print(1);
            if (swipeVector.y > 0 && swipeVector.y >= minSwipeDistance)
            {
                // Para asegurarnos de que no sea un swipe diagonal demasiado pronunciado hacia los lados
                // Puedes ajustar este umbral según tus necesidades.
                //if (Mathf.Abs(swipeVector.x) < swipeVector.y * 0.8f) // Si el movimiento horizontal es menos de la mitad del vertical
                //{
                    SetOpen();
                    _isOpen = true;
                //}
                //else
                //{
                //    if (!_isReseting)
                //        StartCoroutine(ResetRoutine(0.2f));
                //}
            }
            else
            {
                if (!_isReseting)
                    StartCoroutine(ResetRoutine(0.2f));
            }
        }
    }
    public void Vibrate()
    {
        if(PlayerPrefs.GetInt("isVibrationActive") == 1)
            Handheld.Vibrate();
    }
    public void SetShake()
    {
        //pro2.text = Input.acceleration.x.ToString();
        isShaking = true;
        OnShakeDetected.Invoke();

    }
    IEnumerator ResetRoutine(float time)
    {
        _isReseting = true;
        yield return new WaitForSeconds(time);
        isShaking = false;
        _isReseting = false;
        OnResetDetected.Invoke();
    }
    public void ForceReset()
    {
        isShaking = false;
        _isReseting = false;
        _isOpen = false;
    }

    

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 touchEndPos = eventData.position;
        pro2.text = (touchStartPos.y - touchEndPos.y).ToString();
        if ( touchEndPos.y+failSwipeDistance<touchStartPos.y)
        {
            isTouching = false;
            if (!_isReseting)
                StartCoroutine(ResetRoutine(0.2f));

        }
    }
    void OnDrawGizmos()
    {
        if (isTouching)
        {
            // Dibuja la posición inicial del toque en la escena
            // Esto solo es útil para depuración visual si tu objeto es 3D y está en la vista de escena
             Gizmos.color = Color.red;
             Gizmos.DrawSphere(Camera.main.ScreenToWorldPoint(new Vector3(touchStartPos.x, touchStartPos.y, 10)), minSwipeDistance);
        }
    }
    //void Update()
    //{
    //    if (Time.time < lastShakeTime + cooldownTime)
    //    {
    //        pro2.text = "return";

    //        return; // Espera el tiempo de enfriamiento
    //    }
    //    pro2.text = "noreturn";

    //    Vector3 currentAcceleration = InputSystem.GetDevice<Accelerometer>().acceleration.ReadValue();
    //    Vector3 deltaAcceleration = currentAcceleration - lastAcceleration;

    //    // Calcula la magnitud del cambio de aceleración
    //    float shakeMagnitude = deltaAcceleration.magnitude;
    //    pro.text = shakeMagnitude.ToString();
    //    if (shakeMagnitude >= shakeDetectionThreshold)
    //    {
    //        if (!shaking)
    //        {
    //            shaking = true;
    //            lastShakeTime = Time.time;
    //        }

    //        // Si ha estado sacudiéndose lo suficiente
    //        if (Time.time >= lastShakeTime + shakeDetectionDuration)
    //        {
    //            if (OnShakeDetected != null)
    //            {
    //                OnShakeDetected?.Invoke(); // Dispara el evento
    //            }
    //            lastShakeTime = Time.time; // Reinicia el tiempo de la última sacudida
    //            shaking = false; // Reinicia el estado de sacudida
    //        }
    //    }
    //    else
    //    {
    //        shaking = false; // Si la sacudida no es lo suficientemente fuerte, reinicia el estado
    //    }
    //    lastAcceleration = currentAcceleration; // Actualiza la última aceleración
    //}
}
