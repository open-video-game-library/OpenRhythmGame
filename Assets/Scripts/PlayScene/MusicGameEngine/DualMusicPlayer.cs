namespace MusicGameEngine
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEditor;
    using UnityEngine;
    using HolmonUtility;
    using System.Threading.Tasks;

    public class DualMusicPlayer : MonoBehaviour, IAudioPlayable
    {
        [SerializeField] private GameObject _player1Object;
        [SerializeField] private GameObject _player2Object;

        private IAudioPlayable _playingPlayer;
        private IAudioPlayable _player1;
        private IAudioPlayable _player2;

        public double playingTime { get; private set; }
        public float musicPositionTime => _playingPlayer.musicPositionTime;

        public float volume => _playingPlayer.volume;
        public  AudioClip clip => _playingPlayer.clip; 
        public eAudioPlayStatus status { get; private set; } = eAudioPlayStatus.playable;

        public bool isMute => _playingPlayer.isMute;
        public bool isPlaying => _playingPlayer.isPlaying;

        private double _startTime;
        private double _pauseStartTime = 0;
        private double _totalPauseTime = 0;

        public void Start()
        {
            try
            {
                if(!_player1Object || !_player2Object)
                {
                    Debug.LogError("ミュージックプレイヤーオブジェクトが設定されていません");
                    return;
                }
                _player1 = _player1Object.GetComponent<IAudioPlayable>();
                _player2 = _player2Object.GetComponent<IAudioPlayable>();
            }
            catch
            {
                Debug.LogError("ミュージックプレイヤーオブジェクトのいずれかにIAudioPlayableがアタッチされていません");
            }

            _playingPlayer = _player1;
        }

        public void Update()
        {
            if (!_player1.isMute || _player2.isMute) _playingPlayer = _player1;
            else if (_player1.isMute || !_player2.isMute) _playingPlayer = _player2;
            else _playingPlayer = _player1;

            double pauseTime = 0;
            if (_pauseStartTime != 0) pauseTime = AudioSettings.dspTime - _pauseStartTime;

            playingTime = AudioSettings.dspTime - _startTime - _totalPauseTime - pauseTime;
            AudioPlayingTime.SetPlayingTime(playingTime);
        }

        public double Play(AudioClip clip)
        {
            if (status != eAudioPlayStatus.playable)
            {
                Debug.LogWarning("音楽は再生中です");
                return　0;
            }

            //Playを実行すると200ms程度のタイムラグが発生する
            //それらを考慮した処理を行う
            _startTime = AudioSettings.dspTime + AudioPlayingTime.PLAY_DELAY;
            double p1 = _player1.Play(clip);
            double diff = AudioSettings.dspTime + AudioPlayingTime.PLAY_DELAY - p1;
            //Debug.Log(diff);
            _player2.Play(clip, AudioPlayingTime.PLAY_DELAY - diff);

            status = eAudioPlayStatus.playing;

            _player2.Mute(true);

            return _startTime;
        }
        public double Play(AudioClip clip, double startDelay)
        {
            if (status != eAudioPlayStatus.playable)
            {
                Debug.LogWarning("音楽は再生中です");
                return 0;
            }

            _startTime = AudioSettings.dspTime + startDelay;
            double p1 = _player1.Play(clip, startDelay);
            double diff = AudioSettings.dspTime + startDelay - p1;
            _player2.Play(clip, startDelay - diff);

            status = eAudioPlayStatus.playing;

            _player2.Mute(true);

            return _startTime;
        }

        public void Pause(bool enable)
        {
            if (status == eAudioPlayStatus.playable)
            {
                Debug.LogWarning("音楽は再生されていません");
                return;
            }

            if (enable)
            {
                _pauseStartTime = AudioSettings.dspTime;

                status = eAudioPlayStatus.pausing;
            }
            else
            {
                _totalPauseTime += AudioSettings.dspTime - _pauseStartTime;
                _pauseStartTime = 0;

                status = eAudioPlayStatus.playing;
            }

            _player1.Pause(enable);
            _player2.Pause(enable);
        }

        public void Stop()
        {
            _player1.Stop();
            _player2.Stop();

            _pauseStartTime = 0;
            _totalPauseTime = 0;

            status = eAudioPlayStatus.playable;
        }

        public void SetPlayPosition(float time)
        {
            _player1.SetPlayPosition(time);
            _player2.SetPlayPosition(time);
        }

        public void SetVolume(float volume)
        {
            _player1.SetVolume(volume);
            _player2.SetVolume(volume);
        }

        public void Mute(bool enable)
        {
            _player1.Mute(enable);
            _player2.Mute(enable);
        }



        //----------
        [Header("デバッグ")]
        public AudioClip debugClip;
    }


#if UNITY_EDITOR
    //()の中にクラス名を入力
    [CustomEditor(typeof(DualMusicPlayer))]
    public class DualMusicPlayerEditor : Editor
    {
        //OnInspectorGUIでカスタマイズのGUIに変更する
        public override void OnInspectorGUI()
        {
            //元のクラスを取得
            DualMusicPlayer musicPlayer = target as DualMusicPlayer;

            //元のInspector部分を表示する
            base.OnInspectorGUI();

            //元のInspector部分の下にボタンを表示
            if (GUILayout.Button("Play"))
            {
                musicPlayer.Play(musicPlayer.debugClip);
            }
            if (GUILayout.Button("Stop"))
            {
                musicPlayer.Stop();
            }
            if (GUILayout.Button("Pause"))
            {
                if (musicPlayer.status == eAudioPlayStatus.pausing)
                {
                    musicPlayer.Pause(false);
                }
                else
                {
                    musicPlayer.Pause(true);
                }
            }
        }
    }
#endif
}