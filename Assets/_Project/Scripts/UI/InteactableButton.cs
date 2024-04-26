using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InteactableButton : MonoBehaviour
{
    

    [SerializeField] private TMP_InputField email;
    [SerializeField] private TMP_InputField password;

    [SerializeField] private CheckMark check1,check2,check3;

    private void Start() {
        this.GetComponent<Button>().interactable = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (email.text != "" && password.text != "" && check1.IsOn && check2.IsOn && check3.IsOn) 
            {
            this.GetComponent<Button>().interactable = true;
        }
        else this.GetComponent<Button>().interactable = false;
    }
}
