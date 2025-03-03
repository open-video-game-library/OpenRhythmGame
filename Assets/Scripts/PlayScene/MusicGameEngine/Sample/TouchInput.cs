using System;
using System.Collections;
using System.Collections.Generic;
using MusicGameEngine;
using UnityEngine;
using UnityEngine.EventSystems;
using HolmonUtility;

[RequireComponent(typeof(EventTrigger))]
public class TouchInput : InputBase
{
    private EventTrigger _eventTrigger;

    private void Start()
    {
        _eventTrigger = GetComponent<EventTrigger>();

        //eventTriggerに新しいイベントを追加する
        _eventTrigger.triggers.Add(
            EventTriggerGenerator.Generate(
                EventTriggerType.PointerDown,
                () =>
                {
                    Debug.Log("on");
                    ExecuteInputOnCallback();
                })
        );
        _eventTrigger.triggers.Add(
            EventTriggerGenerator.Generate(
                EventTriggerType.PointerUp,
                () =>
                {
                    Debug.Log("off");
                    ExecuteInputOffCallback();
                })
        );
    }
}
