using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class FirebaseController : MonoBehaviour
{
    private const string WEB_API_KEY = "AIzaSyCAdV7ApAGWgRNDi1_HcUQp5lB18EtR4vY";
    private string userLocalId;
    private string userIdToken;

    public void TryLoginAndGetData(string _email, string _passwrod, Action<bool> _callBack) {
        string _loginParms = "{\"email\":\"" + _email + "\",\"password\":\"" + _passwrod +
                             "\",\"returnSecureToken\":true}";

        StartCoroutine(Post("https://identitytoolkit.googleapis.com/v1/accounts:signInWithPassword?key=" + WEB_API_KEY,
            _loginParms, (_result) => {
                Debug.Log("Successfully created account");
                SignInResponse _signInResponse = JsonConvert.DeserializeObject<SignInResponse>(_result);
                userIdToken = _signInResponse.IdToken;
                userLocalId = _signInResponse.LocalId;
                _callBack?.Invoke(true);
                //collect data if need, then return callback with true
            }, (_) =>
            {
                Debug.Log("Didn't manage to login, trying to register");
                Register(_callBack, _loginParms);
            }, false));
    }

    private void Register(Action<bool> _callBack, string _parms) {
        StartCoroutine(Post("https://identitytoolkit.googleapis.com/v1/accounts:signUp?key=" + WEB_API_KEY, _parms,
            (_result) => {
                Debug.Log("Successfully registered account");
                RegisterResponse _registerResult = JsonConvert.DeserializeObject<RegisterResponse>(_result);
                userIdToken = _registerResult.IdToken;
                userLocalId = _registerResult.LocalId;
                _callBack?.Invoke(true);
                //collect data if need, then return callback with true
            }, (_result) => {
                Debug.Log("Register failed");
                _callBack?.Invoke(false);
            }));
    }

    private IEnumerator Post(string _uri, string _jsonData, Action<string> _onSuccess, Action<string> _onError,
        bool _includeHeader = true) {
        if (userIdToken != null) {
            if (_includeHeader) {
                _uri = $"{_uri}?auth={userIdToken}";
            }
        }

        using (UnityWebRequest _webRequest = UnityWebRequest.Post(_uri, _jsonData)) {
            byte[] _jsonToSend = new System.Text.UTF8Encoding().GetBytes(_jsonData);
            _webRequest.uploadHandler = new UploadHandlerRaw(_jsonToSend);
            _webRequest.downloadHandler = new DownloadHandlerBuffer();

            yield return _webRequest.SendWebRequest();

            if (_webRequest.result == UnityWebRequest.Result.Success) {
                _onSuccess?.Invoke(_webRequest.downloadHandler.text);
            } else {
                _onError?.Invoke(_webRequest.error);
            }

            _webRequest.uploadHandler.Dispose();
            _webRequest.downloadHandler.Dispose();
            _webRequest.Dispose();
        }
    }
}
