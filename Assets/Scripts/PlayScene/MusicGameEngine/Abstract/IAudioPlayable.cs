namespace HolmonUtility
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public interface IAudioPlayable
    {
        /// <summary>
        /// 曲をかけ始めてからの経過時間を返す
        /// </summary>
        double playingTime { get; }

        /// <summary>
        /// 曲の再生位置を返す
        /// </summary>
        float musicPositionTime { get; }

        float volume { get; }
        bool isMute { get; }
        bool isPlaying { get; }

        AudioClip clip { get; }
        eAudioPlayStatus status { get; }

        /// <summary>
        /// 音楽を再生する
        /// 返り値にStartTimeを渡される
        /// </summary>
        double Play(AudioClip clip);
        /// <summary>
        /// 音楽を再生する
        /// 返り値にStartTimeを渡される
        /// </summary>
        /// <param name="clip">クリップ</param>
        /// <param name="startDelay">ディレイ時間(s)</param>
        double Play(AudioClip clip, double startDelay);

        void Stop();

        void Pause(bool enable);

        void SetPlayPosition(float time);

        void SetVolume(float volume);

        void Mute(bool enable);
    }

    public enum eAudioPlayStatus
    {
        playable,
        playing,
        pausing
    }
}


