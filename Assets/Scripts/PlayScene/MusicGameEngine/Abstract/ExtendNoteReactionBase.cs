namespace MusicGameEngine
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public abstract class ExtendNoteReactionBase : MonoBehaviour
    {
        protected ScoreContainer _scoreContainer;

        public void SetScoreContainer(ScoreContainer container)
        {
            _scoreContainer = container;
        }

        public abstract void ExtendlInputOnProgress(ScoreData.Note note);

        public abstract void ExtendlInputOffProgress(ScoreData.Note note);

        public abstract void ExtendJudgeCallBack(int judge, ScoreData.Note note, float diff);
    }
}

