namespace MusicGameEngine
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public abstract class ExtendPlayReactionBase : MonoBehaviour
    {
        protected ScoreContainer _scoreContainer;

        public void SetScoreContainer(ScoreContainer container)
        {
            _scoreContainer = container;
        }

        public abstract void StartPlay();

        public abstract void EndPlay();

        public abstract void SetHS(float HS);
    }
}

