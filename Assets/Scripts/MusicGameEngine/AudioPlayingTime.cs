namespace MusicGameEngine
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public static class AudioPlayingTime
    {
        public const double PLAY_DELAY = 5; //Å@1sà»è„ÇÃílÇêÑèß

        public static double PlayingTime { get; private set; }

        public static void SetPlayingTime(double time)
        {
            PlayingTime = time;
        }
    }
}
