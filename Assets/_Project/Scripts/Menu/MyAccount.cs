using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MyAccount : MonoBehaviour
{
    public static LanguageType Language;
    [SerializeField] private Button spain;
    [SerializeField] private Button portuguese;
    private Action callBack;

    [SerializeField] private TMP_Text personEmail;

    public static MyAccount Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else
        {
            Destroy(gameObject);
        }
    }
    public void SetPersonEmailText(string _email)
    {
        personEmail.text = _email;
    }
    public void Setup(Action _callBack)
    {
        callBack = _callBack;
        gameObject.SetActive(true);
    }

    private void OnEnable()
    {
        spain.onClick.AddListener(SetSpain);
        portuguese.onClick.AddListener(SetPortuguese);
    }

    private void OnDisable()
    {
        spain.onClick.RemoveListener(SetSpain);
        portuguese.onClick.RemoveListener(SetPortuguese);
    }

    private void SetSpain()
    {
        Language = LanguageType.Spain;
        Finis();
    }

    private void SetPortuguese()
    {
        Language = LanguageType.Portuguese;
        Finis();
    }

    private void Finis()
    {
        callBack?.Invoke();
    }
}
