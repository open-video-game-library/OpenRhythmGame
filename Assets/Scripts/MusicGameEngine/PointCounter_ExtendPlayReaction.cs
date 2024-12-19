using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MusicGameEngine;

public class PointCounter_ExtendPlayReaction : ExtendPlayReactionBase
{
    [SerializeField] private PointCounter_ExtendNoteReaction pointCounter;

    public override void StartPlay()
    {
        Debug.Log("ExtendPlayProgress");
        pointCounter.Init(_scoreContainer);
    }

    public override void SetHS(float HS)
    {

    }

    public override void EndPlay()
    {

    }
}
