using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HolmonUtility;
using MusicGameEngine;

[RequireComponent(typeof(Transform))]
public class KeyTapNote : Liner2DTapScroll
{
    [Header("段表現オブジェクトのアタッチ")]
    [SerializeField] private Transform dan_1;
    [SerializeField] private Transform dan_2;
    [SerializeField] private Transform dan_3;

    public override void Play(double animationTime, double animationOffset, params double[] correctInputTimes)
    {
        base.Play(animationTime, animationOffset, correctInputTimes);

        if (_additionalInfo[3] == 1)
        {
            dan_1.gameObject.SetActive(true);
        }
        else if (_additionalInfo[3] == 2)
        {
            dan_2.gameObject.SetActive(true);
        }
        else if (_additionalInfo[3] == 3)
        {
            dan_3.gameObject.SetActive(true);
        }
    }

    protected override void Init()
    {
        base.Init();

        dan_1.gameObject.SetActive(false);
        dan_2.gameObject.SetActive(false);
        dan_3.gameObject.SetActive(false);
    }
}