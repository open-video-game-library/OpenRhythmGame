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

        //BPM���؂�ւ�鎞��, �ω����BPM
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
                    Debug.LogError(obj.name + "��IBPMAnimationEvent���A�^�b�`����Ă��܂���");
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

        //�g�p���Ȃ�
        public override void SetHS(float HS)
        {

        }

        //BPM�f�[�^�̏��������s��
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

