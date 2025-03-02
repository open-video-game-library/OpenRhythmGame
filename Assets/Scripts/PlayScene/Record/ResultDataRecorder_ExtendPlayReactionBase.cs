using HolmonUtility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MusicGameEngine;
using System;

public class ResultDataRecorder_ExtendPlayReactionBase : ExtendPlayReactionBase
{
    [Serializable]
    public class NoteTimeDiffs
    {
        public NoteTimeDiffRecorder_ExtendNoteReaction.NoteTimeDiff[] diffs;

        public NoteTimeDiffs(NoteTimeDiffRecorder_ExtendNoteReaction.NoteTimeDiff[] diffs)
        {
            this.diffs = diffs;
        }
    }

    [SerializeField] private NoteTimeDiffRecorder_ExtendNoteReaction _noteTimeDiffRecorder;
    [SerializeField] private bool recordable = false;

    /*
    public void ReceiveOnLoaded(params object[] iSceneSharedObjects)
    {
        var score = iSceneSharedObjects[0] as ScoreContainer;
        var resultData = iSceneSharedObjects[2] as NoteTimeDiffRecorder_ExtendNoteReaction.NoteTimeDiff[];

        //記録が許可されていない場合は処理を終了
        if (!recordable) return;

        NoteTimeDiffs noteTimeDiffs = new NoteTimeDiffs(resultData);

        //パス名の作成
        string scoreName = score.name;
        string nme =
            $"{DateTime.Now.Month}月" +
            $"{DateTime.Now.Day}日 " +
            $"{DateTime.Now.Hour}時" +
            $"{DateTime.Now.Minute}分";
        string pathName = $"{Application.dataPath}/StreamingAssets/PlayLog/{scoreName}/{nme}.json";

        JsonUsage<NoteTimeDiffs>.WriteJson(pathName, noteTimeDiffs);
    }
    */

    public override void EndPlay()
    {
        var score = _scoreContainer;
        var resultData = new NoteTimeDiffs(_noteTimeDiffRecorder.noteTimeDiffs.ToArray());

        //記録が許可されていない場合は処理を終了
        if (!recordable) return;

        NoteTimeDiffs noteTimeDiffs = resultData;

        //パス名の作成
        string scoreName = score.name;
        string nme =
            $"{DateTime.Now.Month}月" +
            $"{DateTime.Now.Day}日 " +
            $"{DateTime.Now.Hour}時" +
            $"{DateTime.Now.Minute}分";
        string pathName = $"{Application.dataPath}/StreamingAssets/PlayLog/{scoreName}/{nme}.json";

        JsonUsage<NoteTimeDiffs>.WriteJson(pathName, noteTimeDiffs);
    }

    //使わない
    //=============================================================================

    public override void StartPlay()
    {

    }


    public override void SetHS(float HS)
    {

    }
}
