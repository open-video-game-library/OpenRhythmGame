using HolmonUtility;
using MusicGameEngine;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class PointCounter_ExtendNoteReaction : ExtendNoteReactionBase
{
    //クラス定義
    //================================================================

    [Serializable]
    private class PointText
    {
        [SerializeField] private Animator animator;
        [SerializeField] private TextMeshProUGUI pointText;

        public void SetPoint(int point)
        {
            pointText.text = point.ToString("D7");
            if(animator != null) animator.SetTrigger("CountUp");
        }
        
        public void ResetPoint()
        {
            pointText.text = "0";
        }
    }

    [Serializable]
    private class RateText
    {
        [SerializeField] private TextMeshProUGUI rateText;

        public void SetRate(float rate)
        {
            if(!rateText.gameObject.activeSelf) rateText.gameObject.SetActive(true);

            rateText.text = rate.ToString("F2") + "%";
        }

        public void ResetRate()
        {
            rateText.gameObject.SetActive(false);
        }
    }

    //変数
    //================================================================

    [Header("パラメーター")]
    [SerializeField] private double MaximumPoint = 1000000;
    [SerializeField] private double PerfectPointRat = 1;
    [SerializeField] private double GoodPointRat = 0.9;
    [SerializeField] private double BadPointRat = 0.5;
    [Header("コンポーネント")]
    [SerializeField] private PointText pointText;
    [SerializeField] private RateText rateText;
    [Header("コールバック")]
    [SerializeField] private UnityEvent<PointData> receiveNewPointData;

    private int perfect = 0;
    private int good = 0;
    private int bad = 0;
    private int miss = 0;

    private int fastCount = 0;
    private int slowCount = 0;

    private int nowCombo = 0;
    private int maxCombo = 0;

    private ScoreData.Note[] _notes = null;

    //譜面内に含まれる全てのノートの数を返す
    private int _allNotesCount = 0;

    //プロパティ
    //================================================================

    //スコア情報を返す
    public PointData pointData { get
        {
            return new PointData(abstractPoint, rate, perfect, good, bad, miss, fastCount, slowCount, 0);
        } }

    private double detailPoint
    {
        get
        {
            double perf = perfect * _perfectPoint * PerfectPointRat;
            double gd = good * _perfectPoint * GoodPointRat;
            double bd = bad * _perfectPoint * BadPointRat;
            return perf + gd + bd;
        }
    }
    private int abstractPoint { get 
        {
            int roundedValue = (int)Math.Round(detailPoint);
            return Mathf.Min(roundedValue, (int)MaximumPoint);
        } 
    }
    private float rate { get 
        {
            double ret = 0;

            int now = perfect + good + bad + miss;
            if(now != 0)
            {
                ret = detailPoint / (now * _perfectPoint);
            }

            return (float)RoundToDecimalPlace.Round(ret, 4) * 100;
        }
    }

    //パーフェクト判定時のスコア値を返す
    private double _perfectPoint { get 
        {
            return MaximumPoint / _allNotesCount;
        } 
    } 

    //メソッド
    //================================================================

    public override void ExtendJudgeCallBack(int judge, ScoreData.Note note, float diff)
    {
        if(judge == 3)
        {
            perfect++;
            nowCombo++;
            if(nowCombo > maxCombo) maxCombo = nowCombo;

            pointText.SetPoint(abstractPoint);
            rateText.SetRate(rate);
        }
        else if(judge == 2)
        {
            good++;
            nowCombo++;
            if (nowCombo > maxCombo) maxCombo = nowCombo;

            FastSlow(diff);
            pointText.SetPoint(abstractPoint);
            rateText.SetRate(rate);
        }
        else if(judge == 1)
        {
            bad++;
            nowCombo = 0;

            FastSlow(diff);
            pointText.SetPoint(abstractPoint);
            rateText.SetRate(rate);
        }
        else if(judge == 0)
        {
            miss++;
            nowCombo = 0;

            rateText.SetRate(rate);
        }

        receiveNewPointData.Invoke(pointData);
    }

    public void Init(ScoreContainer scoreContainer)
    {
        _notes = scoreContainer.GetScore().Notes;

        _allNotesCount = 0;
        int ret = 0;

        foreach (var note in _notes)
        {
            if(note.Endbeat == null)
            {
                ret += 1;
            }
            else
            {
                ret += 2;
            }
        }
        _allNotesCount = ret;

        receiveNewPointData.Invoke(pointData);
    }

    private void FastSlow(float diff)
    {
        //正の方向にずれている場合，slowとして判定する
        if (diff > 0) slowCount++;
        //負の方向にずれている場合，fastとして判定する
        else if (diff < 0) fastCount++;
    }

    //使わない
    //================================================================

    public override void ExtendlInputOffProgress(ScoreData.Note note)
    {

    }

    public override void ExtendlInputOnProgress(ScoreData.Note note)
    {

    }

}
