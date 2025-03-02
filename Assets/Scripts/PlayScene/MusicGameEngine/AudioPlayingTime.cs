namespace MusicGameEngine
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public static class AudioPlayingTime
    {
        public const double PLAY_DELAY = 5; //　1s以上の値を推奨

        public static double PlayingTime { get; private set; }

        public static void SetPlayingTime(double time)
        {
            PlayingTime = time;
        }
    }
}
