﻿using HolmonUtility;
using MusicGameEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

interface FixableJudge
{
    /// <summary>
    /// Perfect, Good, Bad, FaseMiss, OverMissの判定を取得することができる
    /// </summary>
    /// <returns></returns>
    public (double, double, double, double, double) GetJudgeRange();

    public void SetJudgeRange(double perfect, double good, double bad, double fastMiss, double overMiss);
}

public class SettingMenuWindow : ExtendPlayReactionBase
{
    [Serializable]
    public class  TransformProperty
    {
        public Vector3 pos;
        public Vector3 rot;
        public Vector3 scale;
    }

    //定数
    //=======================================================================================================
    
    //判定のデフォルト値
    private const double PERFECT_RANGE = 0.06;
    private const double GOOD_RANGE = 0.12;
    private const double BAD_RANGE = 0.18;
    private const double FAST_MISS_RANGE = 0;
    private const double OVER_TIME = 3;

    //フィールド
    //=======================================================================================================

    [Header("ウィンドウそのもの")]
    [SerializeField] GameObject _window;

    [Header("ウィンドウ操作")]
    [SerializeField] Button _openButton;
    [SerializeField] Button _closeButton;

    [Header("表示・非表示設定")]
    [SerializeField] Toggle _comboTgl;
    [SerializeField] GameObject _comboObj;
    [SerializeField] Toggle _judgeTgl;
    [SerializeField] GameObject _judgeObj;
    [SerializeField] Toggle _bombTgl;
    [SerializeField] GameObject _bombObj;
    [SerializeField] Toggle _keyBeamTgl;
    [SerializeField] GameObject _keyBeamObj;

    [Header("2D・3D表示設定")]
    [SerializeField] TMP_Dropdown _displayMode;
    [SerializeField] Transform _Lane;
    [SerializeField] TransformProperty _3D;
    [SerializeField] GameObject _3D_Judge;
    [SerializeField] TransformProperty _2D;
    [SerializeField] GameObject _2D_Judge;

    [Header("判定幅")]
    [SerializeField] TMP_InputField _perfectRange;
    [SerializeField] TMP_InputField _goodRange;
    [SerializeField] TMP_InputField _badRange;
    [SerializeField] TMP_InputField _fastMissRange;
    [SerializeField] TMP_InputField _overMissRange;
    [SerializeField] List<ObjectPool> _notePools;
    [SerializeField] Button _resetButton;

    [Header("OSC設定")]
    [SerializeField] TMP_Dropdown _selectInputMode;
    [SerializeField] List<InputSwitcher> _inputSwitchers;

    //メソッド
    //=======================================================================================================

    public override void StartPlay()
    {
        //ウィンドウが開かれている状態なら閉じる
        if (_window.activeSelf)
        {
            OpenWindow(false);
        }

        //開くボタンを押せないようにする
        _openButton.interactable = false;
    }

    private void Start()
    {
        //ウィンドウ操作
        _openButton.onClick.AddListener(() => OpenWindow(true));
        _closeButton.onClick.AddListener(() => OpenWindow(false));

        //表示・非表示設定
        _comboTgl.onValueChanged.AddListener((bool value) => _comboObj.SetActive(value));
        _judgeTgl.onValueChanged.AddListener((bool value) => _judgeObj.SetActive(value));
        //_bombTgl.onValueChanged.AddListener((bool value) => _bombObj.SetActive(value));
        _keyBeamTgl.onValueChanged.AddListener((bool value) => _keyBeamObj.SetActive(value));

        //2D・3D表示設定
        _displayMode.onValueChanged.AddListener((int value) => ChangeDisplayMode(value));

        //判定幅
        _perfectRange.onEndEdit.AddListener((string value) => SetJudgeRange(double.Parse(value), 0));
        _goodRange.onEndEdit.AddListener((string value) => SetJudgeRange(double.Parse(value), 1));
        _badRange.onEndEdit.AddListener((string value) => SetJudgeRange(double.Parse(value), 2));
        _fastMissRange.onEndEdit.AddListener((string value) => SetJudgeRange(double.Parse(value), 3));
        _overMissRange.onEndEdit.AddListener((string value) => SetJudgeRange(double.Parse(value), 4));
        _resetButton.onClick.AddListener(() =>
        {
            ResetJudgeRange();
        });
        
        //OSC設定
        _selectInputMode.onValueChanged.AddListener((i) => PlaySceneMetaData.InputMode = (eInputMode)i);
        

        //- 値とフィールドの初期化
        ResetJudgeRange();

    }

    /// <summary>
    /// ウィンドウの開閉を行う
    /// </summary>
    /// <param name="open"></param>
    private void OpenWindow(bool open)
    {
        //状態が異なる場合は，変更する
        if (_window.activeSelf != open)
        {
            _window.SetActive(open);
        }
    }

    private void ChangeDisplayMode(int v)
    {
        if (v == 0)
        {
            _Lane.position = _3D.pos;
            _Lane.rotation = Quaternion.Euler(_3D.rot);
            _Lane.localScale = _3D.scale;

            _2D_Judge.SetActive(false);
            _3D_Judge.SetActive(true);
        }
        else if (v == 1)
        {
            _Lane.position = _2D.pos;
            _Lane.rotation = Quaternion.Euler(_2D.rot);
            _Lane.localScale = _2D.scale;

            _2D_Judge.SetActive(true);
            _3D_Judge.SetActive(false);
        }
    }

    private void SetJudgeRange(double v, int type)
    {
        foreach(var pool in _notePools)
        {
            var prefab = pool.GetPrefab();
            var judge = prefab.GetComponent<FixableJudge>();

            var ranges = judge.GetJudgeRange();

            if(type == 0)
            {
                judge.SetJudgeRange(v, ranges.Item2, ranges.Item3, ranges.Item4, ranges.Item5);
            }
            else if(type == 1)
            {
                judge.SetJudgeRange(ranges.Item1, v, ranges.Item3, ranges.Item4, ranges.Item5);
            }
            else if (type == 2)
            {
                judge.SetJudgeRange(ranges.Item1, ranges.Item2, v, ranges.Item4, ranges.Item5);
            }
            else if (type == 3)
            {
                judge.SetJudgeRange(ranges.Item1, ranges.Item2, ranges.Item3, v, ranges.Item5);
            }
            else if (type == 4)
            {
                judge.SetJudgeRange(ranges.Item1, ranges.Item2, ranges.Item3, ranges.Item4, v);
            }

            pool.SetPrefab(prefab);
            pool.ReGeneratePool();
        }
    }

    private void ResetJudgeRange()
    {
        SetJudgeRange(PERFECT_RANGE, 0);
        SetJudgeRange(GOOD_RANGE, 1);
        SetJudgeRange(BAD_RANGE, 2);
        SetJudgeRange(FAST_MISS_RANGE, 3);
        SetJudgeRange(OVER_TIME, 4);
        var prefab = _notePools[0].GetPrefab();
        var judge = prefab.GetComponent<FixableJudge>();
        _perfectRange.text = judge.GetJudgeRange().Item1.ToString();
        _goodRange.text = judge.GetJudgeRange().Item2.ToString();
        _badRange.text = judge.GetJudgeRange().Item3.ToString();
        _fastMissRange.text = judge.GetJudgeRange().Item4.ToString();
        _overMissRange.text = judge.GetJudgeRange().Item5.ToString();
    }

    //使わない
    //=======================================================================================================

    public override void SetHS(float HS)
    {

    }

    public override void EndPlay()
    {

    }
}
