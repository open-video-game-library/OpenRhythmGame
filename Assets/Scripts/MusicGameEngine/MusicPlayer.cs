namespace MusicGameEngine
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEditor;
    using UnityEngine;
    using UnityEngine.Profiling;
    using HolmonUtility;
    using System.Threading.Tasks;

    [RequireComponent(typeof(AudioSource))]
    public class MusicPlayer : MonoBehaviour, IAudioPlayable
    {
        /// <summary>
        /// 曲の再生時間を取得する
        /// </summary>
        public double playingTime {get; private set; }
        public float musicPositionTime => _audioSource.time;
        public float volume => _audioSource.volume;
        public eAudioPlayStatus status { get; private set; } = eAudioPlayStatus.playable;
        public AudioClip clip => _audioSource.clip;
        public bool isMute => _audioSource.mute;
        public bool isPlaying => _audioSource.isPlaying;

        private AudioSource _audioSource;
        private double _startTime;
        private double _pauseStartTime = 0;
        private double _totalPauseTime = 0;

        public void Start()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        public void Update()
        {
            double pauseTime = 0;
            if (_pauseStartTime != 0) pauseTime = AudioSettings.dspTime - _pauseStartTime;

            playingTime = AudioSettings.dspTime - _startTime - _totalPauseTime - pauseTime;
            AudioPlayingTime.SetPlayingTime(playingTime);
        }

        public double Play(AudioClip clip)
        {
            if(status != eAudioPlayStatus.playable)
            {
                Debug.LogWarning("音楽は再生中です");
                return 0;
            }

            _audioSource.clip = clip;
            _startTime = AudioSettings.dspTime + AudioPlayingTime.PLAY_DELAY;
            //Debug.Log("Single " + _startTime);
            _audioSource.PlayScheduled(_startTime);


            status = eAudioPlayStatus.playing;

            return _startTime;
        }
        public double Play(AudioClip clip, double startDelay)
        {
            if (status != eAudioPlayStatus.playable)
            {
                Debug.LogWarning("音楽は再生中です");
                return 0;
            }

            _audioSource.clip = clip;
            _startTime = AudioSettings.dspTime + startDelay;
            //Debug.Log("Single " + _startTime);
            _audioSource.PlayScheduled(_startTime);


            status = eAudioPlayStatus.playing;

            return _startTime;
        }

        public void Stop()
        {
            if(status == eAudioPlayStatus.playable)
            {
                Debug.LogWarning("音楽は再生されていません");
                return;
            };

            _audioSource.Stop();

            _pauseStartTime = 0;
            _totalPauseTime = 0;

            status = eAudioPlayStatus.playable;
        }

        public void Pause(bool enable)
        {
            if(status == eAudioPlayStatus.playable)
            {
                Debug.LogWarning("音楽は再生されていません");
                return;
            }

            if (enable)
            {
                _pauseStartTime = AudioSettings.dspTime;
                _audioSource.Pause();

                status = eAudioPlayStatus.pausing;
            }
            else
            {
                _totalPauseTime += AudioSettings.dspTime - _pauseStartTime;
                _pauseStartTime = 0;
                _audioSource.UnPause();

                status = eAudioPlayStatus.playing;
            }
        }

        public void SetPlayPosition(float time)
        {
            _audioSource.time = (float)time;
        }

        public void SetVolume(float volume)
        {
            _audioSource.volume = volume;
        }

        public void Mute(bool enable)
        {
            _audioSource.mute = enable;
        }



        //----------
        [Header("デバッグ")]
        public AudioClip debugClip;
    }

#if UNITY_EDITOR
    //()の中にクラス名を入力
    [CustomEditor(typeof(MusicPlayer))]
    public class MusicPlayerEditor : Editor
    {
        //OnInspectorGUIでカスタマイズのGUIに変更する
        public override void OnInspectorGUI()
        {
            //元のクラスを取得
            MusicPlayer musicPlayer = target as MusicPlayer;

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
            if(GUILayout.Button("Pause"))
            {
                if(musicPlayer.status == eAudioPlayStatus.pausing)
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

