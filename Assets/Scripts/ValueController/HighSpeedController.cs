using MusicGameEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HighSpeedController : MonoBehaviour
{
    [SerializeField] Button _up;
    [SerializeField] Button _down;
    [SerializeField] TMP_Text _text;

    [SerializeField] MusicGamePlayer _musicGamePlayer;

    void Start()
    {
        var init = _musicGamePlayer.HighSpeed;
        _text.text = init.ToString();

        _up.onClick.AddListener(() =>
        {
            var v = _musicGamePlayer.SetHS(true);
            _text.text = v.ToString();
        });

        _down.onClick.AddListener(() =>
        {
            var v = _musicGamePlayer.SetHS(false);
            _text.text = v.ToString();
        });
    }
}
