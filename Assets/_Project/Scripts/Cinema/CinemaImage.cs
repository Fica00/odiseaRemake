using System;
using UnityEngine;
using UnityEngine.UI;

public class CinemaImage : MonoBehaviour
{
    private const int HELD_DELAY = 1;
    public static Action<DatabasePanel> OnClickedPanel;
    public static Action<DatabaseChapter> OnClickedChapter;
    
    [SerializeField] private Image image;
    [SerializeField] private GameObject holder;
    [SerializeField] private Button button;
    
    private DatabasePanel panel;
    private DatabaseChapter chapter;
    private float lastHeldTime;

    private void OnEnable()
    {
        button.onClick.AddListener(HandleClick);
    }

    private void OnDisable()
    {
        button.onClick.RemoveListener(HandleClick);
    }

    public void Setup(DatabasePanel _panelData)
    {
        Setup();

        panel = _panelData;
        
        string _imageUrl;
        switch (LanguageManager.Language)
        {
            case LanguageType.Spain:
                _imageUrl = _panelData.ThumbnailES;
                break;
            case LanguageType.Portuguese:
                _imageUrl = _panelData.ThumbnailPT;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        image.LoadImage(_imageUrl);
    }

    public void Setup(DatabaseChapter _chapterData)
    {
        Setup();

        chapter = _chapterData;
        string _imageUrl;
        switch (LanguageManager.Language)
        {
            case LanguageType.Spain:
                _imageUrl = _chapterData.ThumbnailES;
                break;
            case LanguageType.Portuguese:
                _imageUrl = _chapterData.ThumbnailPT;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        image.LoadImage(_imageUrl);
    }

    private void Setup()
    {
        panel = null;
        chapter = null;
        holder.SetActive(true);
    }
    
    public void Hide()
    {
        holder.SetActive(false);
    }
    
    // This method is called by the Main Camera when it is gazing at this GameObject and the screen is touched
    public void OnPointerClick()
    {
        Debug.Log("----- Click");
        HandleClick();
    }    
    
    // This method is called by the Main Camera when it is gazing at this GameObject for some time
    public void OnPointerHeld()
    {
        Debug.Log("----- Held");
        HandleClick();
    }
    
    private void HandleClick()
    {
        if (lastHeldTime+HELD_DELAY>Time.time)
        {
            return;
        }

        lastHeldTime = Time.time;
        
        if (panel!=null)
        {
            OnClickedPanel?.Invoke(panel);
        }
        else if (chapter != null)
        {
            OnClickedChapter?.Invoke(chapter);
        }
    }
}
