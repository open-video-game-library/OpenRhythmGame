namespace MusicGameEngine
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class SingleHoldJudge : JudgeBase, FixableJudge
    {
        [SerializeField] private double PERFECT_RANGE = 0; //3
        [SerializeField] private double GOOD_RANGE = 0; //2
        [SerializeField] private double BAD_RANGE = 0; //1
        [SerializeField] private double FAST_MISS_RANGE = 0; //MISS 0
        [SerializeField] private double OVER_TIME = 0; //見逃しミスしたときノートがきえる時間

        private bool _inputing = false; //入力中かどうかを示すフラグ
        private bool _isPause = false;

        private bool _isMissed = false;

        protected override void Init()
        {
            _inputing = false;
            _isPause = false;
            _isMissed = false;
        }

        protected override void Update()
        {
            base.Update();

            if (OVER_TIME < _timingDiscrepancies[1] && _isMissed)
            {
                ProgressComplete();
            }
        }

        public override void Judge(eInputType t)
        {
            if (_isPause) return;

            if (t == eInputType.On)
            {
                if (_inputing) return;

                if (Math.Abs(_timingDiscrepancies[0]) <= PERFECT_RANGE)
                {
                    ExecuteJudgeCallback(3, (float)_timingDiscrepancies[0], false);
                }
                else if (Math.Abs(_timingDiscrepancies[0]) <= GOOD_RANGE)
                {
                    ExecuteJudgeCallback(2, (float)_timingDiscrepancies[0], false);
                }
                else if (Math.Abs(_timingDiscrepancies[0]) <= BAD_RANGE)
                {
                    ExecuteJudgeCallback(1, (float)_timingDiscrepancies[0], false);
                }
                else if (Math.Abs(_timingDiscrepancies[0]) <= FAST_MISS_RANGE)
                {
                    ExecuteJudgeCallback(0, (float)_timingDiscrepancies[0], false);
                    _isMissed = true;
                    return;
                }
                else return; //判定範囲外の場合これ以下の処理は行わない

                _inputing = true;
            }
            else
            {
                if (!_inputing) return;

                if (Math.Abs(_timingDiscrepancies[1]) <= PERFECT_RANGE)
                {
                    ExecuteJudgeCallback(3, (float)_timingDiscrepancies[1], true);
                    ProgressComplete();
                }
                else if (Math.Abs(_timingDiscrepancies[1]) <= GOOD_RANGE)
                {
                    ExecuteJudgeCallback(2, (float)_timingDiscrepancies[1], true);
                    ProgressComplete();
                }
                else if (Math.Abs(_timingDiscrepancies[1]) <= BAD_RANGE)
                {
                    ExecuteJudgeCallback(1, (float)_timingDiscrepancies[1], true);
                    ProgressComplete();
                }
                else
                {
                    ExecuteJudgeCallback(0, (float)_timingDiscrepancies[1], true);
                    _isMissed = true;
                }

                _inputing = false;
            }
        }

        public override void Pause(bool pause)
        {
            _isPause = pause;
        }

        protected override void ThroughJudge()
        {
            //ホールド起点
            if(!_inputing)
            {
                if (BAD_RANGE < _timingDiscrepancies[0] && !_isMissed)
                {
                    ExecuteJudgeCallback(0, (float)_timingDiscrepancies[0], true);
                    _isMissed = true;
                }
            }
            //ホールド終点
            else
            {
                if (BAD_RANGE < _timingDiscrepancies[1] && !_isMissed)
                {
                    ExecuteJudgeCallback(0, (float)_timingDiscrepancies[1], true);
                    _isMissed = true;
                }
            }
        }

        //インターフェース
        //=======================================================================================================

        public (double, double, double, double, double) GetJudgeRange()
        {
            return (PERFECT_RANGE, GOOD_RANGE, BAD_RANGE, FAST_MISS_RANGE, OVER_TIME);
        }

        public void SetJudgeRange(double perfect, double good, double bad, double fastMiss, double overMiss)
        {
            PERFECT_RANGE = perfect;
            GOOD_RANGE = good;
            BAD_RANGE = bad;
            FAST_MISS_RANGE = fastMiss;
            OVER_TIME = overMiss;
        }
    }
}

