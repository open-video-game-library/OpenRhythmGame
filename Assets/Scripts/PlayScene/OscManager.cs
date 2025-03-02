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
    
    
    
    //以下テスト
    
    private OscClient client;
    public string ipaddress = "127.0.0.1"; //localhost

    void Start()
    {
        client = new OscClient(ipaddress, OscPort);
    }

    void Update()
    {
        // スペースキーでメッセージを送信
        if (Input.GetKeyDown(KeyCode.Q))
        {
            client.Send(OscAddress, 0);
        }
        if (Input.GetKeyUp(KeyCode.Q))
        {
            client.Send(OscAddress, 1);
        }
        
        if (Input.GetKeyDown(KeyCode.W))
        {
            client.Send(OscAddress, 2);
        }
        if (Input.GetKeyUp(KeyCode.W))
        {
            client.Send(OscAddress, 3);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            client.Send(OscAddress, 4);
        }
        if (Input.GetKeyUp(KeyCode.E))
        {
            client.Send(OscAddress, 5);
        }
        
        if (Input.GetKeyDown(KeyCode.R))
        {
            client.Send(OscAddress, 6);
        }
        if (Input.GetKeyUp(KeyCode.R))
        {
            client.Send(OscAddress, 7);
        }
    }
}
