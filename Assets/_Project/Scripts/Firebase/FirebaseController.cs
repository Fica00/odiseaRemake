using Newtonsoft.Json;
using System;
using UnityEngine;

public class FirebaseController : MonoBehaviour
{
    public static FirebaseController Instance;
    public bool IsLoggedIn = false;

    private const string WEB_API_KEY = "AIzaSyCAdV7ApAGWgRNDi1_HcUQp5lB18EtR4vY";

    private string userLocalId;
    private string userIdToken;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        TryAutoLogin();
    }

    public void TryLoginAndGetData(string _email, string _password, Action<bool> _callBack)
    {
        string _loginParms = "{\"email\":\"" + _email + "\",\"password\":\"" + _password + "\",\"returnSecureToken\":true}";

        WebRequests.Instance.Post("https://identitytoolkit.googleapis.com/v1/accounts:signInWithPassword?key=" + WEB_API_KEY, _loginParms,
            (_result) =>
            {
                Debug.Log("Successfully logged in");
                SignInResponse _signInResponse = JsonConvert.DeserializeObject<SignInResponse>(_result);
                userIdToken = _signInResponse.IdToken;
                userLocalId = _signInResponse.LocalId;

                PlayerPrefs.SetString("email", _email);
                PlayerPrefs.SetString("password", _password);
                PlayerPrefs.Save();

                IsLoggedIn = true;
                _callBack?.Invoke(true);
                WebRequests.Instance.SetUserToken(userIdToken);
                MenuController.Instance.FirstImage();
            }, (_) => { MenuController.Instance.SetStatusText("Didn't manage to login, trying to register"); }, false);
    }

    public void Register(string _email, string _password, Action<bool> _callBack)
    {
        string _loginParms = "{\"email\":\"" + _email + "\",\"password\":\"" + _password + "\",\"returnSecureToken\":true}";

        WebRequests.Instance.Post("https://identitytoolkit.googleapis.com/v1/accounts:signUp?key=" + WEB_API_KEY, _loginParms, (_result) =>
        {
            RegisterResponse _registerResult = JsonConvert.DeserializeObject<RegisterResponse>(_result);
            userIdToken = _registerResult.IdToken;
            userLocalId = _registerResult.LocalId;

            PlayerPrefs.SetString("email", _email);
            PlayerPrefs.SetString("password", _password);
            PlayerPrefs.Save();

            IsLoggedIn = true;
            _callBack?.Invoke(true);
            WebRequests.Instance.SetUserToken(userIdToken);
            MenuController.Instance.FirstImage();
        }, (_result) =>
        {
            MenuController.Instance.SetStatusText("Register failed");
            _callBack?.Invoke(false);
        });
    }

    public void ResetPassword(string _email, Action<bool> _callback)
    {
        Debug.Log(_email);
        string _resetParms = "{\"requestType\":\"PASSWORD_RESET\",\"email\":\"" + _email + "\"}";

        WebRequests.Instance.Post("https://identitytoolkit.googleapis.com/v1/accounts:sendOobCode?key=" + WEB_API_KEY, _resetParms, (_result) =>
        {
            MenuController.Instance.SetStatusText("Reset password email sent successfully");
            _callback?.Invoke(true);
        }, (_) =>
        {
            Debug.Log("Failed to send reset password email" + _);
            MenuController.Instance.SetStatusText("Failed to send reset password email");
            _callback?.Invoke(false);
        }, false);
    }

    private void TryAutoLogin()
    {
        if (!PlayerPrefs.HasKey("email") || !PlayerPrefs.HasKey("password"))
            return;

        string _savedEmail = PlayerPrefs.GetString("email");
        string _savedPassword = PlayerPrefs.GetString("password");

        TryLoginAndGetData(_savedEmail, _savedPassword, (_success) => { Debug.Log(_success ? "Auto-login successful" : "Auto-login failed"); });
    }

    public void DeleteLocalUserData()
    {
        PlayerPrefs.DeleteKey("email");
        PlayerPrefs.DeleteKey("password");
        PlayerPrefs.Save();

        IsLoggedIn = false;

        if (!PlayerPrefs.HasKey("email") || !PlayerPrefs.HasKey("password"))
            Debug.Log("Local user data deleted successfully");
    }
}