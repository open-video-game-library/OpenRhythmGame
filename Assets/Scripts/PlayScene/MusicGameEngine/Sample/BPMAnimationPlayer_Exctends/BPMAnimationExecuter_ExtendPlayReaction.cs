using MusicGameEngine;
using HolmonUtility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MusicGameEngine
{
    public class BPMAnimationExecuter_ExtendPlayReaction : ExtendPlayReactionBase
    {
        [SerializeField]private List<GameObject> _IBPMAnimatinonEventObjects;
        private List<IBPMAnimationEvent> _IBPMAnimationEvents = new List<IBPMAnimationEvent>();

        //BPMが切り替わる時間, 変化後のBPM
        private IndexableQueue<(double, int)> _bpmData = new IndexableQueue<(double, int)>();

        private void Start()
        {
            foreach (var obj in _IBPMAnimatinonEventObjects)
            {
                try
                {
                    var extend = obj.GetComponent<IBPMAnimationEvent>();
                    _IBPMAnimationEvents.Add(extend);
                }
                catch
                {
                    Debug.LogError(obj.name + "にIBPMAnimationEventがアタッチされていません");
                }
            }
        }

        private void Update()
        {
            
        }

        public override void StartPlay()
        {
            InitBPMData();
        }

        //使用しない
        public override void SetHS(float HS)
        {

        }

        //BPMデータの初期化を行う
        private void InitBPMData()
        {
            var score = _scoreContainer.GetScore();
            var speeds = score.Speeds;
            foreach (var speed in speeds)
            {
                //var t = speed.beatT;
            }
        }

        public override void EndPlay()
        {

        }
    }
}

