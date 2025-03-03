using System;
using System.Collections;
using System.Collections.Generic;
using MusicGameEngine;
using UnityEngine;
using OscCore;

public class OscManager : MonoBehaviour
{
    [SerializeField] private int OscPort = 9000;
    [SerializeField] private string OscAddress = "/test";
    
    private void Awake()
    {
        PlaySceneMetaData.OscPort = OscPort;
        PlaySceneMetaData.OscAddress = OscAddress;
    }
}
