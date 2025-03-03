using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OscCore;
using MusicGameEngine;
using UniRx;

public class OSCInput : InputBase
{
    [Header("osc信号を受信した際、ボタン入力として受け取る値")]
    [SerializeField] private int _actuatedValue;
    [SerializeField] private OscManager _oscManager;

    private bool _onCall = false;
    private bool _offCall = false;

    private void Start()
    {
        PlaySceneMetaData.Server.TryAddMethod(PlaySceneMetaData.OscAddress, ReadValues);
    }

    private void ReadValues(OscMessageValues values)
    {
        Debug.Log("Receive: " + values.ReadIntElement(0));
        
        int v = values.ReadIntElement(0);
        var inputButton = Mathf.FloorToInt(v/2f);
        var inputType = v % 2;

        if (inputButton != _actuatedValue) return;
        
        if (inputType == 1)
        {
            _offCall = true;
        }
        else if (inputType == 0)
        {
            _onCall = true;
        }
    }

    private void Update()
    {
        if (_onCall)
        {
            Debug.Log("On");
            ExecuteInputOnCallback();
            _onCall = false;
        }

        if (_offCall)
        {
            Debug.Log("Off");
            ExecuteInputOffCallback();
            _offCall = false;
        }
    }
    
    private void OnDestroy()
    {
        PlaySceneMetaData.Server.RemoveMethod(PlaySceneMetaData.OscAddress, ReadValues);
    }
}
