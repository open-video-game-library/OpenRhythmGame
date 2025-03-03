using System;
using System.Collections;
using System.Collections.Generic;
using MusicGameEngine;
using UnityEngine;
using OscCore;

public class OscManager : MonoBehaviour
{
    [SerializeField] private int OscPort = 9000;
    public string OscAddress = "/test";
    public OscServer _server { get; private set; } = null;
    
    private void Awake()
    {
        _server = new OscServer(OscPort);
    }
}
