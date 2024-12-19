namespace HolmonUtility
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public interface IAudioPlayable
    {
        /// <summary>
        /// �Ȃ������n�߂Ă���̌o�ߎ��Ԃ�Ԃ�
        /// </summary>
        double playingTime { get; }

        /// <summary>
        /// �Ȃ̍Đ��ʒu��Ԃ�
        /// </summary>
        float musicPositionTime { get; }

        float volume { get; }
        bool isMute { get; }
        bool isPlaying { get; }

        AudioClip clip { get; }
        eAudioPlayStatus status { get; }

        /// <summary>
        /// ���y���Đ�����
        /// �Ԃ�l��StartTime��n�����
        /// </summary>
        double Play(AudioClip clip);
        /// <summary>
        /// ���y���Đ�����
        /// �Ԃ�l��StartTime��n�����
        /// </summary>
        /// <param name="clip">�N���b�v</param>
        /// <param name="startDelay">�f�B���C����(s)</param>
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


