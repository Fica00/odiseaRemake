using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LanguageImageDisplay : MonoBehaviour
{
    private Image imageDisplay;
    [SerializeField] private List<LanguageImage> images;

    private void Awake()
    {
        imageDisplay = GetComponent<Image>();
    }

    private void OnEnable()
    {
        imageDisplay.sprite = images.Find(_image => _image.Language == LanguageManager.Language).Sprite;
    }
}