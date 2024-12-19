namespace HolmonUtility.AudioEffecter
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Audio;
    using UnityEngine.EventSystems;
    using UnityEngine.UI;

    public class DualAudioEffector : AudioEffector
    {
        [Header("Additional")]
        [SerializeField] private GameObject _supervisionAudioPlayableObject; //2つのAudioPlayableを操作しているAudioPlayable
        [SerializeField] private GameObject _audioPlayableObject1;
        [SerializeField] private GameObject _audioPlayableObject2;

        private bool _loopable = false;
        private float _loopStartTime = 0;
        private float _loopEndTime = 0;
        private float _playerChangeTime = 0;

        private bool _cuttable = false;
        private bool _muting = false;
        private float _span = 0;
        private float _cutTime = 0;

        private IAudioPlayable _supervisionAudioPlayable;
        private IAudioPlayable _audioPlayable1;
        private IAudioPlayable _audioPlayable2;

        protected override void Start()
        {
            //IAudioPlayableの取得
            try
            {
                if (_supervisionAudioPlayableObject == null)
                {
                    Debug.LogError("AudioPlayableObjectが設定されていません");
                    return;
                }
                _supervisionAudioPlayable = _supervisionAudioPlayableObject.GetComponent<IAudioPlayable>();
            }
            catch
            {
                Debug.LogError(_supervisionAudioPlayableObject.name + "にはIAudioPlayableを実装したスクリプトがアタッチされていません");
                return;
            }
            try
            {
                if (_audioPlayableObject1 == null)
                {
                    Debug.LogError("AudioPlayableObjectが設定されていません");
                    return;
                }
                _audioPlayable1 = _audioPlayableObject1.GetComponent<IAudioPlayable>();
            }
            catch
            {
                Debug.LogError(_audioPlayableObject1.name + "にはIAudioPlayableを実装したスクリプトがアタッチされていません");
                return;
            }
            try
            {
                if (_audioPlayableObject2 == null)
                {
                    Debug.LogError("AudioPlayableObjectが設定されていません");
                    return;
                }
                _audioPlayable2 = _audioPlayableObject2.GetComponent<IAudioPlayable>();
            }
            catch
            {
                Debug.LogError(_audioPlayableObject2.name + "にはIAudioPlayableを実装したスクリプトがアタッチされていません");
                return;
            }

            base.Start();
        }

        private void FixedUpdate()
        {
            if (_loopable)
            {
                if (_supervisionAudioPlayable.playingTime >= _playerChangeTime)
                {
                    _playerChangeTime = _playerChangeTime + (_loopEndTime - _loopStartTime);

                    if (!_audioPlayable1.isMute || _audioPlayable2.isMute)
                    {
                        _audioPlayable1.Mute(true);
                        _audioPlayable2.Mute(false);

                        _audioPlayable1.Stop();
                        _audioPlayable1.SetPlayPosition(_loopStartTime);
                        _audioPlayable1.Play(_supervisionAudioPlayable.clip, _playerChangeTime - _supervisionAudioPlayable.playingTime);
                    }
                    else if (_audioPlayable1.isMute || !_audioPlayable2.isMute)
                    {
                        _audioPlayable1.Mute(false);
                        _audioPlayable2.Mute(true);

                        _audioPlayable2.Stop();
                        _audioPlayable2.SetPlayPosition(_loopStartTime);
                        _audioPlayable2.Play(_supervisionAudioPlayable.clip, _playerChangeTime - _supervisionAudioPlayable.playingTime);
                    }
                }
            }

            if (_cuttable)
            {
                if (_supervisionAudioPlayable.playingTime >= _cutTime)
                {
                    _cutTime = _cutTime + _span;

                    IAudioPlayable _controll = null;

                    if (!_audioPlayable1.isMute || _audioPlayable2.isMute) _controll = _audioPlayable1;
                    else if (_audioPlayable1.isMute || !_audioPlayable2.isMute) _controll = _audioPlayable2;

                    Debug.Log(_cutTime + " " + eAudioPlayStatus.playing);

                    if (_muting == false)
                    {
                        _controll.Stop();
                        _controll.SetPlayPosition(_cutTime);
                        _controll.Play(_supervisionAudioPlayable.clip, _cutTime - _supervisionAudioPlayable.playingTime);
                        _muting = true;
                    }
                    else
                    {
                        _muting = false;
                    }

                }
            }
        }

        public override void OnLoop(float divBeatDuration) //分子, 分母
        {
            if (_supervisionAudioPlayable.status != eAudioPlayStatus.playing) return;

            _loopable = true;

            float nowMusicPositionTime = _supervisionAudioPlayable.musicPositionTime - _musicOffset;
            if (nowMusicPositionTime < 0) return;

            float oneDivBeatTime = oneBeatTime / divBeatDuration;
            float nowDivBeat = (int)Mathf.Floor(nowMusicPositionTime / oneDivBeatTime);
            _loopStartTime = nowDivBeat * oneDivBeatTime + _musicOffset;
            _loopEndTime = (nowDivBeat + 1) * oneDivBeatTime + _musicOffset;
            _playerChangeTime = _loopEndTime;

            if (!_audioPlayable1.isMute || _audioPlayable2.isMute)
            {
                _audioPlayable2.Stop();
                _audioPlayable2.SetPlayPosition(_loopStartTime);
                _audioPlayable2.Play(_supervisionAudioPlayable.clip, _playerChangeTime - _supervisionAudioPlayable.playingTime);
            }
            else if (_audioPlayable1.isMute || !_audioPlayable2.isMute)
            {
                _audioPlayable1.Stop();
                _audioPlayable1.SetPlayPosition(_loopStartTime);
                _audioPlayable1.Play(_supervisionAudioPlayable.clip, _playerChangeTime - _supervisionAudioPlayable.playingTime);
            }
        }
        public override void OnLoop(float divBeatDuration, float loopStartClipTime)
        {
            if (_supervisionAudioPlayable.status != eAudioPlayStatus.playing) return;

            _loopable = true;

            float nowMusicPositionTime = _supervisionAudioPlayable.musicPositionTime - _musicOffset;
            if (nowMusicPositionTime < 0) return;

            float oneDivBeatTime = oneBeatTime / divBeatDuration;
            _loopStartTime = loopStartClipTime;
            _loopEndTime = loopStartClipTime + oneDivBeatTime;
            _playerChangeTime = _loopEndTime;

            if (!_audioPlayable1.isMute || _audioPlayable2.isMute)
            {
                _audioPlayable2.Stop();
                _audioPlayable2.SetPlayPosition(_loopStartTime);
                _audioPlayable2.Play(_supervisionAudioPlayable.clip, _playerChangeTime - _supervisionAudioPlayable.playingTime);
            }
            else if (_audioPlayable1.isMute || !_audioPlayable2.isMute)
            {
                _audioPlayable1.Stop();
                _audioPlayable1.SetPlayPosition(_loopStartTime);
                _audioPlayable1.Play(_supervisionAudioPlayable.clip, _playerChangeTime - _supervisionAudioPlayable.playingTime);
            }
        }
        public override void OffLoop()
        {
            _loopable = false;

            _loopStartTime = 0;
            _loopEndTime = 0;
            _playerChangeTime = 0;

            _supervisionAudioPlayable.SetPlayPosition((float)_supervisionAudioPlayable.playingTime);
        }

        public override void OnCut(float divBeatDuration)
        {
            if (_supervisionAudioPlayable.status != eAudioPlayStatus.playing) return;

            _cuttable = true;

            float nowTime = _supervisionAudioPlayable.musicPositionTime - _musicOffset;
            if (nowTime < 0) return;

            float oneDivBeatTime = oneBeatTime / divBeatDuration;
            float nowDivBeat = (int)Mathf.Floor(nowTime / oneDivBeatTime);
            _span = oneDivBeatTime;
            _cutTime = (nowDivBeat + 1) * oneDivBeatTime + _musicOffset;
        }
        public override void OnCut(float divBeatDuration, float cutStartClipTime)
        {
            if (_supervisionAudioPlayable.status != eAudioPlayStatus.playing) return;

            _cuttable = true;

            float nowTime = _supervisionAudioPlayable.musicPositionTime - _musicOffset;
            if (nowTime < 0) return;

            float oneDivBeatTime = oneBeatTime / divBeatDuration;
            _span = oneDivBeatTime;
            _cutTime = cutStartClipTime + oneDivBeatTime;
        }
        public override void OffCut()
        {
            _cuttable = false;

            _muting = false;
            _span = 0;
            _cutTime = 0;

            IAudioPlayable _controll = null;
            if (!_audioPlayable1.isMute || _audioPlayable2.isMute) _controll = _audioPlayable1;
            else if (_audioPlayable1.isMute || !_audioPlayable2.isMute) _controll = _audioPlayable2;

            if (_controll.status != eAudioPlayStatus.playing)
            {
                _controll.Play(_supervisionAudioPlayable.clip);
                _supervisionAudioPlayable.SetPlayPosition((float)_supervisionAudioPlayable.playingTime);
            }
        }
    }
}