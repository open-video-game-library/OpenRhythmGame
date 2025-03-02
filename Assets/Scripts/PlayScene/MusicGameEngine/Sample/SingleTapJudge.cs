namespace MusicGameEngine
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class SingleTapJudge : JudgeBase, FixableJudge
    {
        [SerializeField] private double PERFECT_RANGE = 0; //3
        [SerializeField] private double GOOD_RANGE = 0; //2
        [SerializeField] private double BAD_RANGE = 0; //1
        [SerializeField] private double FAST_MISS_RANGE = 0;
                                                       //MISS 0
        [SerializeField] private double OVER_TIME = 0; //見逃しミスしたときノートがきえる時間

        private bool _isPause = false;
        private bool _isMissed = false;

        protected override void Init()
        {
            _isPause = false;
            _isMissed = false;
        }

        protected override void Update()
        {
            base.Update();

            if (OVER_TIME < _timingDiscrepancies[0] && _isMissed)
            {
                ProgressComplete();
            }
        }

        public override void Judge(eInputType t)
        {
            if (_isPause) return;

            if (t == eInputType.Off) return;

            if (Math.Abs(_timingDiscrepancies[0]) <= PERFECT_RANGE)
            {
                //Debug.Log(_timingDiscrepancies[0]);
                ExecuteJudgeCallback(3, (float)_timingDiscrepancies[0], true);
                ProgressComplete();
            }
            else if (Math.Abs(_timingDiscrepancies[0]) <= GOOD_RANGE)
            {
                //Debug.Log(_timingDiscrepancies[0]);
                ExecuteJudgeCallback(2, (float)_timingDiscrepancies[0], true);
                ProgressComplete();
            }
            else if (Math.Abs(_timingDiscrepancies[0]) <= BAD_RANGE)
            {
                //Debug.Log(_timingDiscrepancies[0]);
                ExecuteJudgeCallback(1, (float)_timingDiscrepancies[0], true);
                ProgressComplete();
            }
            else if (Math.Abs(_timingDiscrepancies[0]) <= FAST_MISS_RANGE)
            {
                ExecuteJudgeCallback(0, (float)_timingDiscrepancies[0], true);
                ProgressComplete();
            }

        }

        public override void Pause(bool pause)
        {
            _isPause = pause;
        }

        protected override void ThroughJudge()
        {
            if (BAD_RANGE < _timingDiscrepancies[0] && !_isMissed)
            {
                //Debug.Log(BAD_RANGE + " " + _timingDiscrepancies[0]);

                ExecuteJudgeCallback(0, (float)_timingDiscrepancies[0], true);
                _isMissed = true;
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

