namespace MusicGameEngine
{
    using HolmonUtility;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using MusicGameEngine;

    public abstract class TimingNotifyBase : MonoBehaviour
    {
        protected Dictionary<string, Action<ScoreData.Note, double[]>> _notifyCallBack = new Dictionary<string, Action<ScoreData.Note, double[]>>();

        protected IndexableQueue<ScoreData.Note> _notes = new IndexableQueue<ScoreData.Note>();
        protected ScoreData.Speed[] _speeds = null;

        protected double _musicOffset = 0;
        protected double _notifyTiming = 0;
        protected float _defBPM = 0;

        /// <summary>
        /// scoreDataにあるノートのタイミング時間-notifyTimingになったらコールバックを呼ぶ
        /// </summary>
        /// <param name="scoreData"></param>
        /// <param name="beforeNotifyTiming">ノートのタイミング時間の何秒前にコールバックを実行するか</param>
        /// <param name="notify"></param>
        public virtual void Play(ScoreData scoreData, double beforeNotifyTiming)
        {
            _notes.Clear();
            _speeds = scoreData.Speeds;

            _musicOffset = scoreData.Offset;
            _notifyTiming = beforeNotifyTiming;
            _defBPM = _speeds[0].BPM;

            foreach (var note in scoreData.Notes)
            {
                _notes.Enqueue(note);
            }
        }

        public string AssignNotifyCallBack(Action<ScoreData.Note, double[]> notifyCallBack)
        {
            string key = RandomStringGenerator.Generate(16);
            this._notifyCallBack.Add(key, notifyCallBack);
            return key;
        }
        public void RemoveNotifyCallBack(string key)
        {
            bool res = this._notifyCallBack.Remove(key);
            if(!res) Debug.LogWarning("コールバック一覧に登録されていないコールバックを解除しようとしました");
        }

        protected void ExecuteNotifyCallBack(ScoreData.Note note, params double[] time)
        {
            foreach(var value in _notifyCallBack.Values)
            {
                value(note, time);
            }
        }

        //BPM変化があるとバグる
        protected double GetNoteTime(ScoreData.Note note)
        {
            return note.BeatT;
        }

        protected double GetEndNoteTIme(ScoreData.Note note)
        {
            return note.EndBeatT;
        }

        /// <summary>
        /// BPM変化による速度変化も考慮したnotifyTimingを加えた生成タイミングの時間を返す
        /// </summary>
        /// <param name="beat"></param>
        /// <returns></returns>
        protected double GetNotifyTime(ScoreData.Note note)
        {
            return note.BeatT - _notifyTiming;
        }

        protected float GetBPM(int[] beat)
        {
            float bpm = _speeds[0].BPM;

            for(int i = _speeds.Length-1; i > 0; i--)
            {
                if (((float)_speeds[i].Beat[0] + ((float)_speeds[i].Beat[1]/ (float)_speeds[i].Beat[2])) <= (beat[0] + ((float)beat[1]/ (float)beat[2])))
                {
                    bpm = _speeds[i].BPM;

                    break;
                }
            }

            return bpm;
        }

        protected double GetBPMT(float bpm)
        {
            return 60f / bpm;
        }
    }
}