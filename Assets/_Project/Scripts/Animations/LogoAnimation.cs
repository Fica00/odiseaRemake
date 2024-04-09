using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class LogoAnimation : MonoBehaviour
{
    [SerializeField] private Image image;
    
    public void Setup(Action _callBack)
    {
        Color _whiteColor = new Color(1, 1, 1, 0);
        gameObject.SetActive(true);
        image.color = _whiteColor;
        _whiteColor.a = 1;
        image.DOColor(_whiteColor, 2).OnComplete(() =>
        {
            _whiteColor.a = 0;
            image.DOColor(_whiteColor, 1f).SetDelay(1).OnComplete(() =>
            {
                _callBack?.Invoke();
            });
        });
    }
}
