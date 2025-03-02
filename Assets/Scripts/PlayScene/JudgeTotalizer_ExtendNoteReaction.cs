using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MusicGameEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// PointCounter_ExtendNoteReactionクラスのコールバックとして登録されるクラス
/// 判定集計の可視化を行う
/// </summary>
public class JudgeTotalizer : MonoBehaviour
{
    //変数
    //================================================================

    [Header("シリアライズ")]
    [SerializeField] TMP_Text _perfectTex;
    [SerializeField] TMP_Text _goodTex;
    [SerializeField] TMP_Text _badTex;
    [SerializeField] TMP_Text _missTex;

    int _perfect;
    int _good ;
    int _bad;
    int _miss;

    //メソッド
    //================================================================

    private void Start()
    {
        //_perfect等の変数の初期化を行う
        _perfect = 0;
        _good = 0;
        _bad = 0;
        _miss = 0;
    }

    public void ReceiveNewPointData(PointData pointData)
    {
        //判定集計を行う
        //もしpointDataの値が異なる場合は，_perfect等の変数を更新する
        if (_perfect != pointData.perfect)
        {
            _perfect = pointData.perfect;
            _perfectTex.text = _perfect.ToString();
        }
        if (_good != pointData.good)
        {
            _good = pointData.good;
            _goodTex.text = _good.ToString();
        }
        if (_bad != pointData.bad)
        {
            _bad = pointData.bad;
            _badTex.text = _bad.ToString();
        }
        if (_miss != pointData.miss)
        {
            _miss = pointData.miss;
            _missTex.text = _miss.ToString();
        }

    }
}
