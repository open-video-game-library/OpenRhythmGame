using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MusicGameEngine;

public class AutoInput : InputBase
{
    public void ReceiveReaction(eInputType eInputType)
    {
        if(eInputType == eInputType.On)
        {
            ExecuteInputOnCallback();
        }
        else
        {
            ExecuteInputOffCallback();
        }
    }
}
