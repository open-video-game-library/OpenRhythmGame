namespace HolmonUtility.AudioEffecter
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Audio;
    using UnityEngine.EventSystems;
    using UnityEngine.UI;

    public class SingleAudioEffector : AudioEffector
    {
        [Header("Additional")]
        [SerializeField] private GameObject _audioPlayableObject;

        private bool _loopable = false;
        private float _loopStartTime = 0;
        private float _loopEndTime = 0;

        private bool _cuttable = false;
        private float _span = 0;
        private float _cutTime = 0;

        private IAudioPlayable _audioPlayable;

        protected override void Start()
        {
            //IAudioPlayableの取得
            try
            {
                if (_audioPlayableObject == null)
                {
                    Debug.LogError("AudioPlayableObjectが設定されていません");
                    return;
                }
                _audioPlayable = _audioPlayableObject.GetComponent<IAudioPlayable>();
            }
            catch
            {
                Debug.LogError(_audioPlayableObject.name + "にはIAudioPlayableを実装したスクリプトがアタッチされていません");
                return;
            }

            base.Start();
        }

        private void FixedUpdate()
        {
            if (_loopable)
            {
                //Debug.Log(_audioPlayable.musicPositionTime  + ">=" + _loopEndTime);
                if (_audioPlayable.musicPositionTime >= _loopEndTime)
                {
                    _audioPlayable.SetPlayPosition(_loopStartTime);
                    //Debug.Log("Loop");
                }
            }

            if (_cuttable)
            {
                if (_audioPlayable.musicPositionTime >= _cutTime)
                {
                    if (_audioPlayable.isMute) _audioPlayable.Mute(false);
                    else _audioPlayable.Mute(true);

                    Debug.Log("ExpectedTime:" + _cutTime + " NowTime:" + _audioPlayable.musicPositionTime + " Diff:" + (_audioPlayable.musicPositionTime - _cutTime));

                    float nowTime = _audioPlayable.musicPositionTime - _musicOffset;
                    float nowDivBeat = (int)Mathf.Floor(nowTime / _span);
                    _cutTime = (nowDivBeat + 1) * _span + _musicOffset;
                }
            }
        }

        public override void OnLoop(float divBeatDuration) //分子, 分母
        {
            if (_audioPlayable.status != eAudioPlayStatus.playing) return;

            _loopable = true;

            float nowTime = _audioPlayable.musicPositionTime - _musicOffset;
            if (nowTime < 0) return;

            float oneDivBeatTime = oneBeatTime / divBeatDuration;
            float nowDivBeat = (int)Mathf.Floor(nowTime / oneDivBeatTime);
            Debug.Log(nowDivBeat);
            _loopStartTime = nowDivBeat * oneDivBeatTime + _musicOffset;
            _loopEndTime = (nowDivBeat + 1) * oneDivBeatTime + _musicOffset;
        }

        public override void OnLoop(float divBeatDuration, float loopStartClipTime)
        {
            if (_audioPlayable.status != eAudioPlayStatus.playing) return;

            _loopable = true;

            float nowTime = _audioPlayable.musicPositionTime - _musicOffset;
            if (nowTime < 0) return;

            float oneDivBeatTime = oneBeatTime / divBeatDuration;
            _loopStartTime = loopStartClipTime;
            _loopEndTime = loopStartClipTime + oneDivBeatTime;
        }
        public override void OffLoop()
        {
            _loopable = false;

            _loopStartTime = 0;
            _loopEndTime = 0;

            _audioPlayable.SetPlayPosition((float)_audioPlayable.playingTime);
        }

        public override void OnCut(float divBeatDuration)
        {
            if (_audioPlayable.status != eAudioPlayStatus.playing) return;

            _cuttable = true;

            float nowTime = _audioPlayable.musicPositionTime - _musicOffset;
            if (nowTime < 0) return;

            float oneDivBeatTime = oneBeatTime / divBeatDuration;
            float nowDivBeat = (int)Mathf.Floor(nowTime / oneDivBeatTime);
            //Debug.Log(nowDivBeat);
            _cutTime = (nowDivBeat + 1) * oneDivBeatTime + _musicOffset;
            _span = oneDivBeatTime;
        }
        public override void OnCut(float divBeatDuration, float cutStartClipTime)
        {
            if (_audioPlayable.status != eAudioPlayStatus.playing) return;

            _cuttable = true;

            float nowTime = _audioPlayable.musicPositionTime - _musicOffset;
            if (nowTime < 0) return;

            float oneDivBeatTime = oneBeatTime / divBeatDuration;
            _cutTime = cutStartClipTime + oneDivBeatTime;
            _span = oneDivBeatTime;
        }
        public override void OffCut()
        {
            _cuttable = false;

            _span = 0;
            _cutTime = 0;

            _audioPlayable.Mute(false);
        }
    }
}

