using MusicGameEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoHoldJudge : JudgeBase
{
    [SerializeField] private double PERFECT_RANGE = 0; //3
    [SerializeField] private double GOOD_RANGE = 0; //2
    [SerializeField] private double BAD_RANGE = 0; //1
                                                   //MISS 
    [SerializeField] private double OVER_TIME = 0; //見逃しミスしたときノートがきえる時間

    private AutoInput _reaction;

    private bool _inputing = false; //入力中かどうかを示すフラグ
    private bool _isPause = false;

    private bool _isMissed = false;

    protected override void Init()
    {
        _inputing = false;
        _isPause = false;
        _isMissed = false;
    }

    private void Start()
    {
        try
        {
            _reaction = this.transform.parent.parent.GetComponentInChildren<AutoInput>();
        }
        catch
        {
            Debug.LogError("AutoInputが見つかりませんでした");
        }
    }

    protected override void Update()
    {
        base.Update();

        if (OVER_TIME < _timingDiscrepancies[1] && _isMissed)
        {
            ProgressComplete();
        }

        if (_correctInputTimes.Length != 0)
        {
            if (_isPause) return;

            if (!_inputing)
            {
                if (_timingDiscrepancies[0] >= -PERFECT_RANGE)
                {
                    _reaction.ReceiveReaction(eInputType.On);
                    ExecuteJudgeCallback(3, (float)_timingDiscrepancies[0], false);
                    _inputing = true;
                }
            }
            else
            {
                if (_timingDiscrepancies[1] >= -PERFECT_RANGE)
                {
                    _reaction.ReceiveReaction(eInputType.Off);
                    ExecuteJudgeCallback(3, (float)_timingDiscrepancies[1], true);
                    _inputing = false;
                    ProgressComplete();
                }
            }
        }
    }

    public override void Judge(eInputType t)
    {

    }

    public override void Pause(bool pause)
    {
        _isPause = pause;
    }

    protected override void ThroughJudge()
    {
        if (!_inputing)
        {
            if (BAD_RANGE < _timingDiscrepancies[0] && !_isMissed)
            {
                ExecuteJudgeCallback(0, (float)_timingDiscrepancies[0], true);
                ProgressComplete();
                _isMissed = true;
            }
        }
        //ホールド終点
        else
        {
            if (BAD_RANGE < _timingDiscrepancies[1] && _isMissed)
            {
                ExecuteJudgeCallback(0, (float)_timingDiscrepancies[1], true);
                ProgressComplete();
                _isMissed = true;
            }
        }
    }
}
