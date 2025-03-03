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
    public static eInputMode InputMode = eInputMode.Key;
}
