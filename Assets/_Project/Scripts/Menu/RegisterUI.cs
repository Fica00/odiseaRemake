using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RegisterUI : MonoBehaviour
{

    [SerializeField] private TMP_InputField registerEmail;
    [SerializeField] private TMP_InputField registerPassword;
    public void Register(TMP_InputField mail)
    {
        if (MenuController.Instance.IsEmail(mail.text))
        {
            FirebaseController.Instance.Register(registerEmail.text, registerPassword.text, Finished);
        }
    }

    private void Finished(bool _status)
    {
        Debug.Log("Finished auth with result: " + _status);

    }
}
