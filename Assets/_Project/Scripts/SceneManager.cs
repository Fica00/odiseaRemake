using UnityEngine;

public class SceneManager : MonoBehaviour
{
    public const string SPLASH_SCREEN = "SplashScreen";
    public const string SIGN_IN = "SignIn";
    public const string CINEMA_VR = "CinemaVr";
    public const string CINEMA_TOUCH = "CinemaTouch";
    
    public static SceneManager Instance;

    private void Awake()
    {
        if (Instance==null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ReloadScene()
    {
        LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }
    
    public void LoadScene(string _name)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(_name);
    }
}
