namespace MusicGameEngine
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class NoteData
    {
        public ScoreData.Note _noteData;

        private JudgeBase _judgeBase;
        public bool endJudgeProgress => _judgeBase.endJudgeProgress;
        private ScrollNoteObjectBase _scrollBase;

        private float _bpmRatio = 1.0f;

        public NoteData(JudgeBase judgeBase, ScrollNoteObjectBase scrollBase, ScoreData.Note note)
        {
            _judgeBase = judgeBase;
            _scrollBase = scrollBase;

            _noteData = note;

            _scrollBase.SetAdditionalInfo(note.Additional);
            _judgeBase.SetAdditionalInfo(note.Additional);
        }

        public void Play(double defAnimationTime, double animationOffset, params double[] correctInputTimes)
        {
            _scrollBase.Play(defAnimationTime, animationOffset, correctInputTimes);
        }

        public void Pause(bool pause)
        {
            _judgeBase.Pause(pause);
            if(pause) _scrollBase.SetBpmRatio(0);
            else _scrollBase.SetBpmRatio(_bpmRatio);
        }

        public void SetHS(float highSpeed)
        {
            _scrollBase.SetHS(highSpeed);
        }
        
        public void SetBPM(float ratio)
        {
            _bpmRatio = ratio;
            _scrollBase.SetBpmRatio(ratio);
        }

        public void Judge(eInputType type)
        {
            _judgeBase.Judge(type);
        }
    }
}