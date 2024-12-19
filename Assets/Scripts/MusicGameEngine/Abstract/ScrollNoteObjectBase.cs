using MusicGameEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//スクロールさせるオブジェクトにアタッチする基底クラス
public abstract class ScrollNoteObjectBase : MonoBehaviour
{
    private Action _destroyCallback = () => Debug.Log("オブジェクトが削除されました");

    protected float[] _additionalInfo;

    /// <summary>
    /// オブジェクトが削除された時に呼ばれるコールバックを設定する
    /// </summary>
    /// <param name="destroyCallback"></param>
    public void SetDestroyCallback(Action destroyCallback)
    {
        _destroyCallback = destroyCallback;
    }

    /// <summary>
    /// noteにセットされたAdditional情報を取得する
    /// </summary>
    /// <param name="additionalInfo"></param>
    public void SetAdditionalInfo(float[] additionalInfo)
    {
        _additionalInfo = additionalInfo;
    }

    /// <summary>
    /// ノートのアニメーションを再生する
    /// </summary>
    public abstract void Play(double animationTime, double animationOffset, params double[] correctInputTimes);

    /// <summary>
    /// ハイスピードを設定する
    /// </summary>
    /// <param name="highSpeed"></param>
    public abstract void SetHS(float highSpeed);

    /// <summary>
    /// BPM変化比率を設定する
    /// </summary>
    /// <param name="ratio"></param>
    public abstract void SetBpmRatio(float ratio);

    /// <summary>
    /// 判定のコールバックを受け取る
    /// </summary>
    /// <param name="judge"></param>
    public abstract void ReceiveJudgeCallback(int judge);

    private void OnDestroy()
    {
        _destroyCallback();
    }

    protected abstract void Init();

    private void OnDisable()
    {
        Init();
        _destroyCallback = () => { };
    }
}
