using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public static class Utils
{
    public static void LoadImage(this Image _image, string _url)
    {
        _image.StartCoroutine(LoadImageRoutine(_image, _url));
    }
    
    private static IEnumerator LoadImageRoutine(Image _image, string _url)
    {
        if (string.IsNullOrEmpty(_url))
        {
            yield break;
        }
        
        _image.sprite = null;
        _image.color = new Color(0f, 0f, 0f, 0f);

        for (int _i = 0; _i < 10; ++_i)
        {
            yield return WebRequestHandler.Instance.GetTexture(_url, _texture =>
            {
                _image.type = Image.Type.Simple;
                _image.color = Color.white;
                _image.sprite = Sprite.Create(_texture, new Rect(0, 0, _texture.width, _texture.height), new Vector2(0.5f, 0.5f));
            }, _error => { Debug.LogErrorFormat("Image load failed: {0}", _error); });

            if (_image.sprite != null)
            {
                break;
            }

            yield return new WaitForSeconds(1f);
        }
    }

}
