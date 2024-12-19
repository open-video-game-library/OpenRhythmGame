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

        //�L�^��������Ă��Ȃ��ꍇ�͏������I��
        if (!recordable) return;

        NoteTimeDiffs noteTimeDiffs = new NoteTimeDiffs(resultData);

        //�p�X���̍쐬
        string scoreName = score.name;
        string nme =
            $"{DateTime.Now.Month}��" +
            $"{DateTime.Now.Day}�� " +
            $"{DateTime.Now.Hour}��" +
            $"{DateTime.Now.Minute}��";
        string pathName = $"{Application.dataPath}/StreamingAssets/PlayLog/{scoreName}/{nme}.json";

        JsonUsage<NoteTimeDiffs>.WriteJson(pathName, noteTimeDiffs);
    }
    */

    public override void EndPlay()
    {
        var score = _scoreContainer;
        var resultData = new NoteTimeDiffs(_noteTimeDiffRecorder.noteTimeDiffs.ToArray());

        //�L�^��������Ă��Ȃ��ꍇ�͏������I��
        if (!recordable) return;

        NoteTimeDiffs noteTimeDiffs = resultData;

        //�p�X���̍쐬
        string scoreName = score.name;
        string nme =
            $"{DateTime.Now.Month}��" +
            $"{DateTime.Now.Day}�� " +
            $"{DateTime.Now.Hour}��" +
            $"{DateTime.Now.Minute}��";
        string pathName = $"{Application.dataPath}/StreamingAssets/PlayLog/{scoreName}/{nme}.json";

        JsonUsage<NoteTimeDiffs>.WriteJson(pathName, noteTimeDiffs);
    }

    //�g��Ȃ�
    //=============================================================================

    public override void StartPlay()
    {

    }


    public override void SetHS(float HS)
    {

    }
}
