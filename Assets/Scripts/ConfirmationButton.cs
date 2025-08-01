using Managers.Menu;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ConfirmationButton : MonoBehaviour
{
    private Button _button;
    public UnityEvent onConfirm;
        
    private void Awake()
    {
        _button = GetComponent<Button>();
            
        // Reemplaza el click del botón con nuestra función
        _button.onClick.RemoveAllListeners();
        _button.onClick.AddListener(ShowConfirmation);
    }
        
    private void ShowConfirmation()
    {
        // Guarda el evento original
        MenuManager.instance.ShowConfirmationWithEvent(onConfirm);
    }
}