using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using HolmonUtility;
using MusicGameEngine;

public class JudgeDisplay_ExtendNoteReaction : ExtendNoteReactionBase
{
    [SerializeField] private ObjectPool _perfect;
    [SerializeField] private ObjectPool _good;
    [SerializeField] private ObjectPool _bad;
    [SerializeField] private ObjectPool _miss;
    [SerializeField] private int _animationMS;

    public override void ExtendJudgeCallBack(int judge, ScoreData.Note note, float diff)
    {
        PlayJudgeTextAnimation(judge);
    }

    public override void ExtendlInputOffProgress(ScoreData.Note note)
    {
    }

    public override void ExtendlInputOnProgress(ScoreData.Note note)
    {
    }

    private async void PlayJudgeTextAnimation(int judge)
    {
        //オブジェクト自体がアクティブでないなら処理を行わない
        if (!this.gameObject.activeInHierarchy)
        {
            return;
        }

        if (judge == 3)
        {
            var obj = _perfect.GetObject(true);
            obj.transform.SetAsLastSibling();

            obj.GetComponent<Animation>().Play();

            await Task.Delay(_animationMS);

            _perfect.ReturnObject(obj);
        }
        else if(judge == 2)
        {
            var obj = _good.GetObject(true);
            obj.transform.SetAsLastSibling();

            obj.GetComponent<Animation>().Play();

            await Task.Delay(_animationMS);

            _good.ReturnObject(obj);
        }
        else if (judge == 1)
        {
            var obj = _bad.GetObject(true);
            obj.transform.SetAsLastSibling();

            obj.GetComponent<Animation>().Play();

            await Task.Delay(_animationMS);

            _bad.ReturnObject(obj);
        }
        else if (judge == 0)
        {
            var obj = _miss.GetObject(true);
            obj.transform.SetAsLastSibling();

            obj.GetComponent<Animation>().Play();

            await Task.Delay(_animationMS);

            _miss.ReturnObject(obj);
        }
    }
}
