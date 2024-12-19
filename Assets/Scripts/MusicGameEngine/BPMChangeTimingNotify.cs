namespace MusicGameEngine
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class BPMChangeTimingNotify : TimingNotifyBase
    {
        int SpeedRef = 0;

        public override void Play(ScoreData scoreData, double notifyTiming)
        {
            base.Play(scoreData, notifyTiming);

            SpeedRef = 0;
        }

        void FixedUpdate()
        {
            if (_notes.Count != 0)
            {
                //Debug.Log(GetNoteTime(_notes[0].beat));
                if (GetNoteTime(_notes[0]) + _musicOffset < AudioPlayingTime.PlayingTime)
                {
                    //Debug.Log(GetBPM(_notes[0].beat) + " / " + _defBPM);
                    ExecuteNotifyCallBack(_notes[0], GetBPM(_speeds[SpeedRef].Beat)/_defBPM);

                    _notes.Dequeue();
                    SpeedRef++;
                }
            }
        }
    }
}

