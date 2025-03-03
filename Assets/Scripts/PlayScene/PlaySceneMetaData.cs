using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OscCore;

public enum eInputMode
{
    Key,
    Touch,
    OSC
}

public static class PlaySceneMetaData
{
    //OSC
    public static int OscPort = 9000;
    public static string OscAddress = "/test";
    public static OscServer Server {
        get
        {
            if (_server == null)
            {
                _server = new OscServer(OscPort);
            }
            return _server;
        } 
    }
    private static OscServer _server = null;
    
    //各種設定
    public static bool DisplayCombo = true;
    public static bool DisplayJudge = true;
    public static bool DisplayBomb = true;
    public static bool DisplayKeyBeam = true;

    public static int DisplayMode = 0;
    
    public static double PerfectRange = 0.05;
    public static double GoodRange = 0.12;
    public static double BadRange = 0.18;
    public static double FastMissRange = 0;
    public static double OverMissRange = 3;
    private const double PERFECT_RANGE = 0.06;
    private const double GOOD_RANGE = 0.12;
    private const double BAD_RANGE = 0.18;
    private const double FAST_MISS_RANGE = 0;
    private const double OVER_TIME = 3;
    
    public static eInputMode InputMode = eInputMode.Key;

}
