using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LanguageTextDisplay : MonoBehaviour
{
    private TextMeshProUGUI textDisplay;
    [SerializeField] private List<LanguageText> texts;

    private void Awake()
    {
        textDisplay = GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        LanguageManager.OnLanguageChanged += ShowText;
        
        ShowText();
    }

    private void OnDisable()
    {
        LanguageManager.OnLanguageChanged -= ShowText;
    }

    private void ShowText()
    {
        textDisplay.text = texts.Find(_text => _text.Language == LanguageManager.Language).Text;
    }
}
