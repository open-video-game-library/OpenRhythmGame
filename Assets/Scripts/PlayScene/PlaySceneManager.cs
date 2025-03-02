using System;
using System.Collections;
using System.Collections.Generic;
using HolmonUtility;
using MusicGameEngine;
using UnityEngine;

public class PlaySceneManager : ExtendPlayReactionBase
{
    [SerializeField] private LoadSceneUtility _loadSceneUtility;
    [SerializeField] private PointCounter_ExtendNoteReaction _pointCounter;
    
    public override void EndPlay()
    {
        var score = _scoreContainer;
        var point = _pointCounter.pointData;
        
        _loadSceneUtility.LoadScene("ResultScene", score, point);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _loadSceneUtility.LoadScene("PlayScene_Demo");
        }
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //EndPlay();
        }
    }

    //以下使わない
    //--------------------------------------------------------------------------------

    public override void StartPlay()
    {
        
    }

    public override void SetHS(float HS)
    {
        
    }
}
