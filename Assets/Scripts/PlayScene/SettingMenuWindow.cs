using HolmonUtility;
using MusicGameEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
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

    [Header("入力設定")]
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
        _comboTgl.onValueChanged.AddListener((bool value) =>
        {
            PlaySceneMetaData.DisplayCombo = value;
            _comboObj.SetActive(value);
        });
        _judgeTgl.onValueChanged.AddListener((value) =>
        {
            PlaySceneMetaData.DisplayJudge = value;
            _judgeObj.SetActive(value);
        });
        _bombTgl.onValueChanged.AddListener((bool value) =>
        {
            PlaySceneMetaData.DisplayBomb = value;
            _bombObj.SetActive(value);
        });
        _keyBeamTgl.onValueChanged.AddListener((bool value) =>
        {
            PlaySceneMetaData.DisplayKeyBeam = value;
            _keyBeamObj.SetActive(value);
        });

        //2D・3D表示設定
        _displayMode.onValueChanged.AddListener((int value) =>
        {
            PlaySceneMetaData.DisplayMode = value;
            ChangeDisplayMode(value);
        });

        //判定幅
        _perfectRange.onEndEdit.AddListener((string value) =>
        {
            PlaySceneMetaData.PerfectRange = double.Parse(value);
            SetJudgeRange(double.Parse(value), 0);
        });
        _goodRange.onEndEdit.AddListener((string value) =>
        {
            PlaySceneMetaData.GoodRange = double.Parse(value);
            SetJudgeRange(double.Parse(value), 1);
        });
        _badRange.onEndEdit.AddListener((string value) =>
        {
            PlaySceneMetaData.BadRange = double.Parse(value);
            SetJudgeRange(double.Parse(value), 2);
        });
        _fastMissRange.onEndEdit.AddListener((string value) =>
        {
            PlaySceneMetaData.FastMissRange = double.Parse(value);
            SetJudgeRange(double.Parse(value), 3);
        });
        _overMissRange.onEndEdit.AddListener((string value) =>
        {
            PlaySceneMetaData.OverMissRange = double.Parse(value);
            SetJudgeRange(double.Parse(value), 4);
        });
        
        //入力設定
        _selectInputMode.onValueChanged.AddListener((i) =>
        {
            PlaySceneMetaData.InputMode = (eInputMode)i;
        });

        //各種値の反映
        _comboTgl.isOn = PlaySceneMetaData.DisplayCombo;
        _judgeTgl.isOn = PlaySceneMetaData.DisplayJudge;
        _bombTgl.isOn = PlaySceneMetaData.DisplayBomb;
        _keyBeamTgl.isOn = PlaySceneMetaData.DisplayKeyBeam;
        
        _displayMode.value = PlaySceneMetaData.DisplayMode;
        ChangeDisplayMode(PlaySceneMetaData.DisplayMode);
        
        _perfectRange.text = PlaySceneMetaData.PerfectRange.ToString();
        SetJudgeRange(PlaySceneMetaData.PerfectRange, 0);
        _goodRange.text = PlaySceneMetaData.GoodRange.ToString();
        SetJudgeRange(PlaySceneMetaData.GoodRange, 1);
        _badRange.text = PlaySceneMetaData.BadRange.ToString();
        SetJudgeRange(PlaySceneMetaData.BadRange, 2);
        _fastMissRange.text = PlaySceneMetaData.FastMissRange.ToString();
        SetJudgeRange(PlaySceneMetaData.FastMissRange, 3);
        _overMissRange.text = PlaySceneMetaData.OverMissRange.ToString();
        SetJudgeRange(PlaySceneMetaData.OverMissRange, 4);
        
        _selectInputMode.value = (int)PlaySceneMetaData.InputMode;
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

    //使わない
    //=======================================================================================================

    public override void SetHS(float HS)
    {

    }

    public override void EndPlay()
    {

    }
}
