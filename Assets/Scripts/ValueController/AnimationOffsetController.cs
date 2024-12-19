using MusicGameEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AnimationOffsetController : MonoBehaviour
{
    [SerializeField] Button _up;
    [SerializeField] Button _down;
    [SerializeField] TMP_Text _text;

    [SerializeField] MusicGamePlayer _musicGamePlayer;
    [SerializeField] private double incresement = 0.001;

    void Start()
    {
        var init = _musicGamePlayer.AnimationOffset;
        _text.text = init.ToString();

        _up.onClick.AddListener(() =>
        {
            var v = _musicGamePlayer.SetAnimationOffset(_musicGamePlayer.AnimationOffset + incresement);
            _text.text = v.ToString();
        });

        _down.onClick.AddListener(() =>
        {
            var v = _musicGamePlayer.SetAnimationOffset(_musicGamePlayer.AnimationOffset - incresement);
            _text.text = v.ToString();
        });
    }
}
