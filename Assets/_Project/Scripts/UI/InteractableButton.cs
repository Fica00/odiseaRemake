using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InteractableButton : MonoBehaviour
{
    [SerializeField] private TMP_InputField email;
    [SerializeField] private TMP_InputField password;

    [SerializeField] private CheckMark check2,check3;
    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    private void Start() 
    {
        button.interactable = false;
    }

    private void FixedUpdate()
    {
        if (email.text != "" && password.text != "" && check2.IsOn && check3.IsOn)
        {
            button.interactable = true;
        }
        else
        {
            button.interactable = false;
        }
    }
}
