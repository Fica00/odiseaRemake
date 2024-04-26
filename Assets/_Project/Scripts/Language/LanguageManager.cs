using System;
using UnityEngine;
using UnityEngine.UI;

public class LanguageManager : MonoBehaviour
{
    public static LanguageType Language;
    [SerializeField] private Button spain;
    [SerializeField] private Button portuguese;
    private Action callBack;

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
        Finish();
    }

    private void SetPortuguese()
    {
        Language = LanguageType.Portuguese;
        Finish();
    }

    private void Finish()
    {
        callBack?.Invoke();
        gameObject.SetActive(false);
    }
}
