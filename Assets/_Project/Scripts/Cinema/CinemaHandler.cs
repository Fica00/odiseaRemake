using System;
using System.Linq;
using Google.XR.Cardboard;
using RenderHeads.Media.AVProVideo;
using UnityEngine;

public class CinemaHandler : MonoBehaviour
{
    [SerializeField] private XRLoader xrLoader;
    [SerializeField] private CinemaImage[] cinemaImages;
    [SerializeField] private ThemeData[] themes;
    [SerializeField] private Transform world;
    [SerializeField] private MonoBehaviour[] vrScripts;
    [SerializeField] private MediaPlayer mediaPlayer;
    [SerializeField] private GameObject mediaPlayerHolder;

    private GameObject stage;
    private GameObject environment;

    public static bool IsVR => Application.isMobilePlatform;

    private void OnEnable()
    {
        CinemaImage.OnClickedPanel += ShowEpisodes;
        CinemaImage.OnClickedChapter += PlayChapter;
    }

    private void OnDisable()
    {
        CinemaImage.OnClickedPanel -= ShowEpisodes;
        CinemaImage.OnClickedChapter -= PlayChapter;
    }

    private void ShowEpisodes(DatabasePanel _panel)
    {
        SetTheme(_panel.Theme);
        ShowAsEmpty();

        for (int _i = 0; _i < _panel.Chapters.Length; _i++)
        {
            DatabaseChapter _panelChapter = _panel.Chapters[_i];
            cinemaImages[_i].Setup(_panelChapter);
        }
    }

    private void SetTheme(int _themeIndex)
    {
        var _data = themes.SingleOrDefault(_themeData => _themeData.Theme == (Theme)(_themeIndex-1));
        if (_data == null)
        {
            return;
        }
        
        if(stage == null || stage.name != _data.Stage.name)
        {
            if (stage != null)
            {
                Destroy(stage);
            }
            stage = Instantiate(_data.Stage, world);
            stage.name = _data.Stage.name;
        }
        
        if(environment == null || environment.name != _data.Environment.name)
        {
            if (environment != null)
            {
                Destroy(environment);
            }
            environment = Instantiate(_data.Environment, world);
            environment.name = _data.Environment.name;
        }
    }

    private void PlayChapter(DatabaseChapter _chapterData)
    {
        Debug.Log("--- Should play: " + _chapterData.ID);
            
        bool _h265 = false;

        string _os = SystemInfo.operatingSystem;
        string _model = SystemInfo.deviceModel;

        Debug.LogFormat("OS: {0}", _os);
        Debug.LogFormat("Model: {0}", _model);

        if(_os.StartsWith("Android"))
        {
            int _i = _os.IndexOf("API-", StringComparison.Ordinal)+4;
            int _j = _os.IndexOf(" ", _i, StringComparison.Ordinal);

            _h265 = Int32.TryParse(_os.Substring(_i, _j-_i), out var _n) && _n >= 26;
        }
        else if(_os.Contains("iPhone OS "))
        {
            if(_model.StartsWith("iPad"))
            {
                int _i = _os.IndexOf("OS ", StringComparison.Ordinal)+3;
                int _j = _i == -1 ? -1 : _os.IndexOf('.', _i);

                _h265 = _j != -1 && Int32.TryParse(_os.Substring(_i, _j-_i), out var _n) && _n >= 11;
            }
            else if(_model.StartsWith("iPhone"))
            {
                const int I = 6;
                int _j = _model.IndexOf(',', I);

                _h265 = _j != -1 && Int32.TryParse(_os.Substring(I, _j-I), out var _n) && _n >= 8;
            }
        }

        string _url264;
        string _url265;
        switch (LanguageManager.Language)
        {
            case LanguageType.Spain:
                _url264 = _chapterData.VideoPT264;
                _url265 = _chapterData.VideoPT265;
                break;
            case LanguageType.Portuguese:
                _url264 = _chapterData.VideoES264;
                _url265 = _chapterData.VideoES265;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        PlayVideo(_h265 && !string.IsNullOrEmpty(_url265) ? _url265 : _url264, _chapterData.Stereoscopic);
    }

    private void PlayVideo(string _url, bool _stereo)
    {
        mediaPlayer.m_StereoPacking = _stereo ? StereoPacking.TopBottom : StereoPacking.None;
        mediaPlayer.OpenVideoFromFile(MediaPlayer.FileLocation.AbsolutePathOrURL, _url);
        mediaPlayerHolder.SetActive(true);
    }

    private void Awake()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        if (!IsVR)
        {
            foreach (var _vrScript in vrScripts)
            {
                Destroy(_vrScript);
            }
        }
        Init();
    }

    private void Init()
    {
        if (IsVR)
        {
            xrLoader.Initialize();
            xrLoader.Start();
        }
    }

    private void Start()
    {
        ShowPrograms();
    }

    private void ShowPrograms()
    {
        ShowAsEmpty();
        
        for (int _i = 0; _i < Database.Instance.Data.Length; _i++)
        {
            DatabasePanel _panel = Database.Instance.Data[_i];
            cinemaImages[_i].Setup(_panel);
        }
    }

    private void ShowAsEmpty()
    {
        foreach (var _cinemaImage in cinemaImages)
        {
            _cinemaImage.Hide();
        }
    }
}
