using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MusicGameEngine;
using System;
using HolmonUtility;

public class NoteTimeDiffRecorder_ExtendNoteReaction : ExtendNoteReactionBase
{
    [Serializable]
    public class NoteTimeDiff
    {
        public int column;
        public int[] timing;
        public double correctTime;
        public int judge;
        public float diff;

        //コンストラクタ
        public NoteTimeDiff(int[] timing, double correctTime, float diff, int column, int judge)
        {
            this.timing = timing;
            this.correctTime = correctTime;
            this.diff = diff;
            this.column = column;
            this.judge = judge;
        }
    }

    public List<NoteTimeDiff> noteTimeDiffs = new List<NoteTimeDiff>();

    private void Start()
    {
        noteTimeDiffs = new List<NoteTimeDiff>();
    }

    public override void ExtendJudgeCallBack(int judge, ScoreData.Note note, float diff)
    {
        var noteTimeDiff = new NoteTimeDiff(note.Beat, note.BeatT, diff, note.Column, judge);

        noteTimeDiffs.Add(noteTimeDiff);
    }

    public override void ExtendlInputOffProgress(ScoreData.Note note)
    {

    }

    public override void ExtendlInputOnProgress(ScoreData.Note note)
    {

    }
}
