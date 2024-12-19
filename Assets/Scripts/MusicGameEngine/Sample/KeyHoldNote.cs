using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MusicGameEngine;

public class KeyHoldNote : Liner2DHoldScroll
{
    [Header("段表現オブジェクトのアタッチ")]
    [SerializeField] private Transform dan_1;
    [SerializeField] private Transform dan_2;
    [SerializeField] private Transform dan_3;

    private Transform _activeDan = null;

    protected override void Update()
    {

        //if (Input.GetKeyDown(KeyCode.L)) Play();

        if (_scrollStartTime != 0 && _prevPlayingTime != 0)
        {
            _scrollRatioNumerator += _bpmRatio * (AudioPlayingTime.PlayingTime - _prevPlayingTime);
            this.transform.localPosition = _notePosition;
            if (_isLongNote)
            {
                _endNote.transform.localPosition = _endNoteDiff;
                _fillNote.transform.localPosition = _endNoteDiff / 2;
                _activeDan.transform.localPosition = _endNoteDiff / 2;

                float lsx = _endNoteDiff.x == 0 ? 1 : _endNoteDiff.x;
                float lsy = _endNoteDiff.x == 0 ? 1 : _endNoteDiff.x;

                if (_fillNote.transform.localScale != new Vector3(lsx - 0.13f, lsy - 0.13f, 1.0f))
                {
                    _fillNote.transform.localScale = new Vector3(lsx - 0.13f, lsy - 0.13f, 1.0f);
                }

                lsx = _endNoteDiff.x == 0 ? _activeDan.transform.localScale.x : _endNoteDiff.x;
                lsy = _endNoteDiff.y == 0 ? _activeDan.transform.localScale.y : _endNoteDiff.y;

                if (_activeDan.transform.localScale != new Vector3(lsx, lsy, 1.0f))
                {
                    _activeDan.transform.localScale = new Vector3(lsx, lsy, 1.0f);
                }

            }

            double r = (_correctInputTimes[1] - _correctInputTimes[0]) / _scrollTime;
        }

        _prevPlayingTime = AudioPlayingTime.PlayingTime;
    }

    public override void Play(double animationTime, double animationOffset, params double[] correctInputTimes)
    {
        _scrollTime = animationTime;
        _scrollRatioNumerator = animationOffset * _bpmRatio;
        this.gameObject.SetActive(true);
        _scrollStartTime = AudioPlayingTime.PlayingTime;
        _correctInputTimes = correctInputTimes;

        if (_isLongNote)
        {
            _endNote.SetActive(true);
            _fillNote.SetActive(true);
            float lsx = _endNoteDiff.x == 0 ? 1 : _endNoteDiff.x - 0.13f;
            float lsy = _endNoteDiff.y == 0 ? 1 : _endNoteDiff.y - 0.13f;
            _fillNote.transform.localScale = new Vector3(lsx, lsy, 1.0f);
        }



        if (_additionalInfo[3] == 1)
        {
            dan_1.gameObject.SetActive(true);
            _activeDan = dan_1;
        }
        else if (_additionalInfo[3] == 2)
        {
            dan_2.gameObject.SetActive(true);
            _activeDan = dan_2;
        }
        else if (_additionalInfo[3] == 3)
        {
            dan_3.gameObject.SetActive(true);
            _activeDan = dan_3;
        }
    }

    protected override void Init()
    {
        base.Init();

        dan_1.gameObject.SetActive(false);
        dan_2.gameObject.SetActive(false);
        dan_3.gameObject.SetActive(false);

        _activeDan = null;
    }
}
