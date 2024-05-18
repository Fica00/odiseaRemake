using System;
using System.Linq;
using Google.XR.Cardboard;
using RenderHeads.Media.AVProVideo;
using UnityEngine;
using UnityEngine.UI;

public enum CurrentScreen
{
    Panel,
    Chapter,
    Video
}

public class CinemaHandler : MonoBehaviour
{
    [SerializeField] private XRLoader xrLoader;
    [SerializeField] private CinemaImage[] cinemaImages;
    [SerializeField] private ThemeData[] themes;
    [SerializeField] private Transform world;
    [SerializeField] private MonoBehaviour[] vrScripts;
    [SerializeField] private MediaPlayer mediaPlayer;
    [SerializeField] private GameObject mediaPlayerHolder;
    [SerializeField] private Button goBackButtonSection;
    [SerializeField] private Button goBackButtonVideo;

    private GameObject stage;
    private GameObject environment;
    private Camera mainCamera;
    private CurrentScreen currentScreen;
    private Color cameraColor;
    public static bool IsVR => Application.isMobilePlatform;

    private void OnEnable()
    {
        CinemaImage.OnClickedPanel += ShowEpisodes;
        CinemaImage.OnClickedChapter += PlayChapter;
        BackButtonHandler.OnClicked += GoBack;
        goBackButtonVideo.onClick.AddListener(GoBack);
    }

    private void OnDisable()
    {
        CinemaImage.OnClickedPanel -= ShowEpisodes;
        CinemaImage.OnClickedChapter -= PlayChapter;
        BackButtonHandler.OnClicked -= GoBack;
        goBackButtonVideo.onClick.RemoveListener(GoBack);
    }

    private void GoBack()
    {
        switch (currentScreen)
        {
            case CurrentScreen.Panel:
                StopVR();
                SceneManager.Instance.LoadScene(SceneManager.SIGN_IN);
                break;
            case CurrentScreen.Chapter:
                ShowPrograms();
                break;
            case CurrentScreen.Video:
                StopVideo();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
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

        currentScreen = CurrentScreen.Chapter;
    }

    private void SetTheme(int _themeIndex)
    {
        var _data = themes.SingleOrDefault(_themeData => _themeData.Theme == (Theme)(_themeIndex - 1));
        if (_data == null)
        {
            return;
        }

        if (stage == null || stage.name != _data.Stage.name)
        {
            if (stage != null)
            {
                Destroy(stage);
            }

            stage = Instantiate(_data.Stage, world);
            stage.name = _data.Stage.name;
        }

        if (environment == null || environment.name != _data.Environment.name)
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

        if (_os.StartsWith("Android"))
        {
            int _i = _os.IndexOf("API-", StringComparison.Ordinal) + 4;
            int _j = _os.IndexOf(" ", _i, StringComparison.Ordinal);

            _h265 = Int32.TryParse(_os.Substring(_i, _j - _i), out var _n) && _n >= 26;
        }
        else if (_os.Contains("iPhone OS "))
        {
            if (_model.StartsWith("iPad"))
            {
                int _i = _os.IndexOf("OS ", StringComparison.Ordinal) + 3;
                int _j = _i == -1 ? -1 : _os.IndexOf('.', _i);

                _h265 = _j != -1 && Int32.TryParse(_os.Substring(_i, _j - _i), out var _n) && _n >= 11;
            }
            else if (_model.StartsWith("iPhone"))
            {
                const int I = 6;
                int _j = _model.IndexOf(',', I);

                _h265 = _j != -1 && Int32.TryParse(_os.Substring(I, _j - I), out var _n) && _n >= 8;
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
        currentScreen = CurrentScreen.Video;
        world.gameObject.SetActive(false);
        mediaPlayerHolder.SetActive(true);
        mainCamera.backgroundColor = Color.black;
        mediaPlayer.m_StereoPacking = _stereo ? StereoPacking.TopBottom : StereoPacking.None;
        mediaPlayer.OpenVideoFromFile(MediaPlayer.FileLocation.AbsolutePathOrURL, _url);
        goBackButtonVideo.gameObject.SetActive(true);
    }

    private void StopVideo()
    {
        world.gameObject.SetActive(true);
        mediaPlayerHolder.SetActive(false);
        mainCamera.backgroundColor = cameraColor;
        goBackButtonVideo.gameObject.SetActive(false);
        currentScreen = CurrentScreen.Chapter;
    }

    private void Awake()
    {
        mainCamera = Camera.main;
        cameraColor = mainCamera.backgroundColor;
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
    
    private void StopVR()
    {
        if (IsVR)
        {
            xrLoader.Stop();
            xrLoader.Deinitialize();
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

        currentScreen = CurrentScreen.Panel;
    }

    private void ShowAsEmpty()
    {
        foreach (var _cinemaImage in cinemaImages)
        {
            _cinemaImage.Hide();
        }
    }
    private void OnApplicationQuit()
    {
        StopVR();
    }
}