namespace MusicGameEngine
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using UnityEditor;
    using UnityEngine;
    using HolmonUtility;
    using System.Linq;
    using UnityEngine.UIElements;

    //ここはMonoでもよくね？
    //判定の基底クラス
    public abstract　class JudgeBase : MonoBehaviour
    {
        protected double[] _correctInputTimes { get; private set; } = new double[] { };

        protected Dictionary<string, Action<int, float, bool>> _judgeCallBack = new Dictionary<string, Action<int, float, bool>>(); //判定結果を通知する
        protected Dictionary<string, Action> _completeCallBack = new Dictionary<string, Action>(); //ノートが役割を終えたことを通知する

        // 音楽の再生時間と正しい入力時間との差分を取得する
        protected double[] _timingDiscrepancies
        {
            get
            {
                double[] discrepancies = new double[_correctInputTimes.Length];
                for (int i = 0; i < _correctInputTimes.Length; i++)
                {
                    discrepancies[i] = AudioPlayingTime.PlayingTime - _correctInputTimes[i];
                }
                return discrepancies;
            }
        }

        protected float[] _additionalInfo;
        protected bool _endJudgeProgress = false;
        public bool endJudgeProgress => _endJudgeProgress;

        protected abstract void Init();

        /// <summary>
        /// 押してほしいタイミングを設定する
        /// </summary>
        /// <param name="correctInputTime"></param>
        public void SetCorrectInputTimes(params double[] correctInputTimes)
        {
            _endJudgeProgress = false;
            _correctInputTimes = correctInputTimes;

            Init();
        }

        /// <summary>
        /// noteにセットされたAdditional情報を取得する
        /// </summary>
        /// <param name="additionalInfo"></param>
        public void SetAdditionalInfo(float[] additionalInfo)
        {
            _additionalInfo = additionalInfo;
        }

        protected virtual void Update()
        {
            if(_correctInputTimes.Length == 0)
            {
                Debug.LogError("CorrectInputTimeの設定が完了していません");
                return;
            }

            if(!_endJudgeProgress) ThroughJudge();

            //このオブジェクトが破棄されないまま放置されていた場合は、自動で破棄する?
            if (_timingDiscrepancies.Length != 0 && 10 < _timingDiscrepancies.Max())
            {
                Debug.LogWarning("破棄されず放置されている可能性のある判定オブジェクトが存在します");
            }

#if UNITY_EDITOR
            if (!EditorApplication.isPlaying) return;
#endif
        }

        /// <summary>
        /// 判定を受け取るコールバックを設定する
        /// </summary>
        /// <param name="judgeCallBack"></param>
        /// <returns></returns>
        public string AssignJudgeResultCallback(Action<int, float, bool> judgeCallBack)
        {
            string key = RandomStringGenerator.Generate(16);
            this._judgeCallBack.Add(key, judgeCallBack);
            return key;
        }
        /// <summary>
        /// 判定を受け取るコールバックを解除する
        /// </summary>
        /// <param name="key"></param>
        public void RemoveJudgeResultCallback(string key)
        {
            bool res = this._judgeCallBack.Remove(key);
            if (!res) Debug.LogWarning("コールバック一覧に登録されていないコールバックを解除しようとしました");
        }
        /// ノートオブジェクトが役割を終えたことを受け取るコールバックを設定する
        /// </summary>
        /// <param name="completeCallBack"></param>
        /// <returns></returns>
        public string AssignNotesObjectCompleteCallBack(Action completeCallBack)
        {
            string key = RandomStringGenerator.Generate(16);
            this._completeCallBack.Add(key, completeCallBack);
            return key;
        }
        /// <summary>
        /// ノートオブジェクトが役割を終えたことを受け取るコールバックを解除する
        /// </summary>
        /// <param name="key"></param>
        public void RemoveNotesObjectCompleteCallBack(string key)
        {
            bool res = this._completeCallBack.Remove(key);
            if (!res) Debug.LogWarning("コールバック一覧に登録されていないコールバックを解除しようとしました");
        }

        /// <summary>
        /// 任意のタイミングで判定処理を行う
        /// </summary>
        public abstract void Judge(eInputType type);

        /// <summary>
        /// 判定処理の一時停止を行う
        /// </summary>
        /// <param name="pause"></param>
        public abstract void Pause(bool pause);

        //コールバックを呼び出す
        protected void ExecuteJudgeCallback(int judge, float diff, bool end)
        {
            if (_endJudgeProgress)
            {
                Debug.LogWarning("このノートオブジェクトの判定処理は既に終わっています");
                return;
            }

            if (end) //これならいけるか・・？
            {
                if (_endJudgeProgress)
                {
                    Debug.LogError("ほげ");
                }

                //値A変更
                _endJudgeProgress = true;
            }

            foreach (var action in _judgeCallBack.Values)
            {
                //アクションA
                action(judge, diff, end);
            }

            /*
            if(end)
            {
                if(_endJudgeProgress)
                {
                    Debug.LogError("ほげ");
                }

                //値A変更
                _endJudgeProgress = true;
            }
            */
        }
        private void ExecuteNotesObjectCompleteCallback()
        {
            foreach (var action in _completeCallBack.Values)
            {
                action();
            }
        }

        protected void ProgressComplete()
        {
            if(!_endJudgeProgress)
            {
                Debug.LogWarning("ノートの判定処理が終わっていないのにかかわらず，ノーツを無効化しようとしています");
            }

            ExecuteNotesObjectCompleteCallback();
        }

        //スルー判定処理を行う
        protected abstract void ThroughJudge();

        private async void OnDisable()
        {
            _judgeCallBack.Clear();
            _correctInputTimes = new double[] { };

            await Task.Delay(2);

            _completeCallBack.Clear();
        }

    }
}


