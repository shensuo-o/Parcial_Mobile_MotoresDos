using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.InputSystem; // Asegúrate de incluir este namespace
using UnityEngine.EventSystems;


public class GatchaSystem : MonoBehaviour, IPointerDownHandler , IPointerUpHandler
{
    public Animator myAnimator; // Referencia al Animator del cofre
    public GameObject gachaRewardPanel; // Panel que muestra las recompensas del gacha
    public float minSwipeDistance = 50f;

    public GameObject chestOpenEffect; // Partículas o efecto visual al abrir el cofre
    private bool isOpen = false;
    private Vector2 touchStartPos; // Posición inicial del toque
    private bool isTouching = false; // Bandera para saber si el dedo sigue presionado

    void OnEnable()
    {
    }

    void OnDisable()
    {
          ShakeSystem.OnSuccess -= OpenChest; // Desuscribirse al evento
    }

    void Start()
    {
        myAnimator = GetComponent<Animator>();
        ShakeSystem.OnSuccess += OpenChest;
        ShakeSystem.OnResetDetected += ResetChest;
        ShakeSystem.OnShakeDetected += ShakeChest;
        if (gachaRewardPanel != null)
        {
            gachaRewardPanel.SetActive(false); // Asegúrate de que el panel de recompensas esté oculto al inicio
        }
    }

    public void OpenChest()
    {
        myAnimator.SetTrigger("Open");
        isOpen = true;

    }
    public void ShakeChest()
    {
        if(!isOpen)
        myAnimator.SetBool("IsShaking", true);
    }
    public void ResetChest()
    {
        if (!isOpen)
            myAnimator.SetBool("IsShaking", false);
    }
    public void CloseChest()
    {
        myAnimator.SetBool("IsShaking", false);

        myAnimator.SetTrigger("Close");
        isOpen = false;

    }
    public void ShowGachaRewards()
    {
        if (gachaRewardPanel != null)
        {
            gachaRewardPanel.SetActive(true); // Muestra el panel de recompensas
            // Aquí puedes llamar a tu script de Gacha para generar las recompensas
            // Ejemplo: FindObjectOfType<GachaManager>().GenerateRewards();
        }
        // Después de mostrar las recompensas, puedes resetear el cofre o desactivarlo
        // Para este ejemplo, lo dejaremos abierto y el usuario puede cerrar el panel de recompensas
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (isOpen)
            return;
        if (!eventData.pointerDrag)
        {
            

        }
        isTouching = true;
        touchStartPos = eventData.position; // Guarda la posición inicial del toque
        ShakeChest();
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
                if (Mathf.Abs(swipeVector.x) < swipeVector.y * 0.5f) // Si el movimiento horizontal es menos de la mitad del vertical
                {
                    myAnimator.SetTrigger("Open");
                    isOpen = true;
                }
                else
                {
                    ResetChest();
                }
            }
            else
            {
                ResetChest();
            }
        }
    }
}
