using HolmonUtility;
using MusicGameEngine;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public enum eRank
{
    S,
    A,
    B,
    C,
    D,
    E
}

public class PointData
{
    public int point { get; private set; }
    public float rate { get; private set; }

    public int perfect { get; private set; }
    public int good { get; private set; }
    public int bad { get; private set; }
    public int miss { get; private set; }

    public int fastCount { get; private set; }
    public int slowCount { get; private set; }

    public int maxCombo { get; private set; }

    public eRank rank
    {
        get
        {
            if (rate >= 95) return eRank.S;
            else if (rate >= 90) return eRank.A;
            else if (rate >= 80) return eRank.B;
            else if (rate >= 70) return eRank.C;
            else if (rate >= 60) return eRank.D;
            else return eRank.E;
        }
    }

    public PointData(int point, float rate, int perfect, int good, int bad, int miss, int fastCount, int slowCount, int maxCombo)
    {
        this.point = point;
        this.rate = rate;
        this.perfect = perfect;
        this.good = good;
        this.bad = bad;
        this.miss = miss;
        this.fastCount = fastCount;
        this.slowCount = slowCount;
        this.maxCombo = maxCombo;
    }
}

public class PointCounter_ExtendNoteReaction : ExtendNoteReactionBase
{
    //�N���X��`
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

    //�ϐ�
    //================================================================

    [Header("�p�����[�^�[")]
    [SerializeField] private double MaximumPoint = 1000000;
    [SerializeField] private double PerfectPointRat = 1;
    [SerializeField] private double GoodPointRat = 0.9;
    [SerializeField] private double BadPointRat = 0.5;
    [Header("�R���|�[�l���g")]
    [SerializeField] private PointText pointText;
    [SerializeField] private RateText rateText;
    [Header("�R�[���o�b�N")]
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

    //���ʓ��Ɋ܂܂��S�Ẵm�[�g�̐���Ԃ�
    private int _allNotesCount = 0;

    //�v���p�e�B
    //================================================================

    //�X�R�A����Ԃ�
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

    //�p�[�t�F�N�g���莞�̃X�R�A�l��Ԃ�
    private double _perfectPoint { get 
        {
            return MaximumPoint / _allNotesCount;
        } 
    } 

    //���\�b�h
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
        //���̕����ɂ���Ă���ꍇ�Cslow�Ƃ��Ĕ��肷��
        if (diff > 0) slowCount++;
        //���̕����ɂ���Ă���ꍇ�Cfast�Ƃ��Ĕ��肷��
        else if (diff < 0) fastCount++;
    }

    //�g��Ȃ�
    //================================================================

    public override void ExtendlInputOffProgress(ScoreData.Note note)
    {

    }

    public override void ExtendlInputOnProgress(ScoreData.Note note)
    {

    }

}
