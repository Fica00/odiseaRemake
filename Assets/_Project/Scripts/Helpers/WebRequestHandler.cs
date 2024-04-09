using System;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Text;

public class WebRequestHandler : MonoBehaviour
{
    public static WebRequestHandler Instance;

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

    public IEnumerator GetRequest(string _url, Action<string> _callbackOnSuccess, Action<string> _callbackOnError)
    {
        using UnityWebRequest _webRequest = UnityWebRequest.Get(_url);
        yield return _webRequest.SendWebRequest();

        if (_webRequest.result == UnityWebRequest.Result.ConnectionError || _webRequest.result == UnityWebRequest.Result.ProtocolError)
        {
            _callbackOnError?.Invoke(_webRequest.error);
        }
        else
        {
            _callbackOnSuccess?.Invoke(_webRequest.downloadHandler.text);
        }
    }
    
    public IEnumerator GetTexture(string _url, Action<Texture2D> _callbackOnSuccess, Action<string> _callbackOnError)
    {
        using UnityWebRequest _www = UnityWebRequestTexture.GetTexture(_url);
        yield return _www.SendWebRequest();

        if (_www.result == UnityWebRequest.Result.ConnectionError || _www.result == UnityWebRequest.Result.ProtocolError)
        {
            _callbackOnError?.Invoke(_www.error);
        }
        else
        {
            Texture2D _texture = DownloadHandlerTexture.GetContent(_www);
            _callbackOnSuccess?.Invoke(_texture);
        }
    }
    
    public IEnumerator PostRequest(string _url, string _jsonData, Action<string> _callbackOnSuccess, Action<string> _callbackOnError)
    {
        using UnityWebRequest _webRequest = new UnityWebRequest(_url, "POST");
        byte[] _bodyRaw = Encoding.UTF8.GetBytes(_jsonData);
        _webRequest.uploadHandler = new UploadHandlerRaw(_bodyRaw);
        _webRequest.downloadHandler = new DownloadHandlerBuffer();
        _webRequest.SetRequestHeader("Content-Type", "application/json");

        yield return _webRequest.SendWebRequest();

        if (_webRequest.result == UnityWebRequest.Result.ConnectionError || _webRequest.result == UnityWebRequest.Result.ProtocolError)
        {
            _callbackOnError?.Invoke(_webRequest.error);
        }
        else
        {
            _callbackOnSuccess?.Invoke(_webRequest.downloadHandler.text);
        }
    }
    
    
}
