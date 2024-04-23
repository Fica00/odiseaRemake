using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenUrlOnClick : MonoBehaviour
{
    [SerializeField] private List<LanguageText> urls;
    private Button button;
    
    private void Awake()
    {
        button = GetComponent<Button>();
        if (button==null)
        {
            Destroy(this);
        }
    }

    private void OnEnable()
    {
        button.onClick.AddListener(OpenUrl);
    }

    private void OnDisable()
    {
        button.onClick.RemoveListener(OpenUrl);
    }

    private void OpenUrl()
    {
        var _url = urls.Find(_url => _url.Language == LanguageManager.Language);
        if (_url==default)
        {
            return;
        }
        
        Application.OpenURL(_url.Text);
    }
}
