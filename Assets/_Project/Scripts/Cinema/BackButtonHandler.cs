using System;
using UnityEngine;
using UnityEngine.UI;

public class BackButtonHandler : MonoBehaviour
{
    private const int HELD_DELAY = 1;
    
    public static Action OnClicked;

    [SerializeField] private Button button;
    
    private float lastHeldTime;
    
    private void OnEnable()
    {
        button.onClick.AddListener(HandleClick);
    }

    private void OnDisable()
    {
        button.onClick.RemoveListener(HandleClick);
    }
    
    public void OnPointerClick()
    {
        Debug.Log("----- Click");
        HandleClick();
    }
    
    public void OnPointerHeld()
    {
        Debug.Log("----- Held");
        HandleClick();
    }

    private void HandleClick()
    {
        if (lastHeldTime + HELD_DELAY > Time.time)
        {
            return;
        }

        lastHeldTime = Time.time;
        
        OnClicked?.Invoke();
    }
}
