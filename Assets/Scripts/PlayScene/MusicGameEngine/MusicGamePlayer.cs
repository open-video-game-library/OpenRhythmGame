namespace MusicGameEngine
{
    using System.Collections.Generic;
    using UnityEngine;
    using HolmonUtility;
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using UnityEditor;
    using TMPro;

    public class MusicGamePlayer : MonoBehaviour
    {
        [Serializable]
        public class NotesGroup
        {
            public GameObject _NotesGroupManagerObject;
            [NonSerialized] public NotesGroupManager _notesGroupManager;
            public int _columnNum;
            public List<GameObject> _extendNoteReactionBaseObjects;
            [NonSerialized] public List<ExtendNoteReactionBase> _extendNoteReactions = new List<ExtendNoteReactionBase>();

            public void GetComponent()
            {
                try
                {
                    _notesGroupManager = _NotesGroupManagerObject.GetComponent<NotesGroupManager>();
                }
                catch
                {
                    Debug.LogError(_NotesGroupManagerObject.name + "にNotesGroupManagerがアタッチされていません");
                }
                foreach (var obj in _extendNoteReactionBaseObjects)
                {
                    try
                    {
                        var extend = obj.GetComponent<ExtendNoteReactionBase>();
                        _extendNoteReactions.Add(extend);
                    }catch
                    {
                        Debug.LogError(obj.name + "にExtendNoteReactionBaseがアタッチされていません");
                    }

                }
            }
        }

        [Header("ロジック")]
        [SerializeField] private GameObject _audioPlayableLogic;

        [SerializeField] private List<NotesGroup> _notesGroups;
        [SerializeField] private List<GameObject> _extendPlayReactionBaseObjects;

        [Header("パラメーター")]
        [SerializeField] protected double _judgeOffset = 0.0;
        [SerializeField] protected double _animationOffset = 0.0;
        [SerializeField] protected float _highSpeedIncrement = 0.5f; //ハイスピード値の増加量
        [SerializeField] protected float _highSpeedMax = 5.0f; //ハイスピードの最大値
        [SerializeField] protected float _highSpeedMin = 1f; //ハイスピードの最小値
        [SerializeField, ReadOnly] protected ScoreContainer _scoreContainer;
        public ScoreContainer scoreContainer => _scoreContainer;

        private IAudioPlayable _audioPlayable;

        //プロパティ
        //==============================================================================================================

        public double HighSpeed => _notesGroups[0]._notesGroupManager.HS;
        public double JudgeOffset => _judgeOffset;
        public double AnimationOffset => _animationOffset;

        //メソッド
        //==============================================================================================================

        protected virtual void Awake()
        {
            try
            {
                _audioPlayable = _audioPlayableLogic.GetComponent<IAudioPlayable>();
            }
            catch
            {
                Debug.LogError(_audioPlayableLogic.name + "にIAudioPlayableがアタッチされていません");
                return;
            }

            foreach (var notesGroup in _notesGroups)
            {
                notesGroup.GetComponent();
            }
        }

        /// <summary>
        /// 譜面の再生を開始する
        /// </summary>
        public void Play(ScoreContainer scoreContainer)
        {
            _scoreContainer = scoreContainer;
            _audioPlayable.Play(scoreContainer.Song);

            //await Task.Delay(Mathf.RoundToInt((float)AudioPlayingTime.PLAY_DELAY *1000)); //PLAY_DELAY分の待機を行う

            var score = scoreContainer.GetScore();

            foreach (var noteGroup in _notesGroups)
            {
                foreach(var extend in noteGroup._extendNoteReactions)
                {
                    extend.SetScoreContainer(scoreContainer);
                }

                //同じColimnのノートを取得
                var s = score.Notes.Where(n => n.Column == noteGroup._columnNum).ToArray();
                ScoreData newData = new ScoreData(score.Speeds, s, score.Offset);

                noteGroup._notesGroupManager.SetExtendNoteReactionBases(noteGroup._extendNoteReactions.ToArray());
                noteGroup._notesGroupManager.SetJudgeOffset(_judgeOffset);
                noteGroup._notesGroupManager.SetDisplayOffset(_animationOffset);

                noteGroup._notesGroupManager.Play(newData);
            }

            foreach(var extendPlayReaction in _extendPlayReactionBaseObjects)
            {
                if (extendPlayReaction == null) Debug.LogError("オブジェクトがアタッチされていません");
                if (!extendPlayReaction.GetComponent<ExtendPlayReactionBase>()) Debug.LogError(extendPlayReaction.name + "にExtendPlayReactionBaseがアタッチされていません");

                ExtendPlayReactionBase component = extendPlayReaction.GetComponent<ExtendPlayReactionBase>();

                component.SetScoreContainer(scoreContainer);
                component.StartPlay();
            }

            StartObserveIsPlaying(changed =>
            {
                if (!changed)
                {
                    foreach (var extendPlayReaction in _extendPlayReactionBaseObjects)
                    {
                        ExtendPlayReactionBase component = null;

                        try
                        {
                            component = extendPlayReaction.GetComponent<ExtendPlayReactionBase>();

                            if (component == null) throw new System.Exception();
                        }
                        catch
                        {
                            Debug.LogError(extendPlayReaction.name + "にExtendPlayReactionBaseがアタッチされていません");
                        }

                        component.EndPlay();
                    }
                }
                else
                {
                    Debug.LogError("曲の再生状況に例外が生じている可能性があります");
                }
            });
        }

        /// <summary>
        /// ハイスピードを変更する
        /// 変更後のハイスピードを返り値として渡す
        /// </summary>
        /// <param name="up"></param>
        /// <returns></returns>
        public virtual float SetHS(bool up)
        {
            if (_notesGroups.Count == 0)
            {
                Debug.LogError("NotesGroupManagerが一つもアタッチされていません");
                return 1;
            }

            float nowHS = _notesGroups[0]._notesGroupManager.HS;

            float to = 0;

            if (up) to = Mathf.Clamp(nowHS + _highSpeedIncrement, _highSpeedMin, _highSpeedMax);
            else to = Mathf.Clamp(nowHS - _highSpeedIncrement, _highSpeedMin, _highSpeedMax);

            foreach (var noteGroup in _notesGroups) noteGroup._notesGroupManager.SetHS(to);
            foreach (var extendPlayReaction in _extendPlayReactionBaseObjects)
            {
                ExtendPlayReactionBase component = null;

                try
                {
                    component = extendPlayReaction.GetComponent<ExtendPlayReactionBase>();

                    if (component == null) throw new System.Exception();
                }
                catch
                {
                    Debug.LogError(extendPlayReaction.name + "にExtendPlayReactionBaseがアタッチされていません");
                }

                component.SetHS(to);
            }

            Debug.Log("HSを" + to + "に変更");

            return to;
        }

        /// <summary>
        /// 判定オフセットを設定する
        /// プレー中の変更は不可能
        /// </summary>
        /// <param name="offset"></param>
        public virtual double SetJudgeOffset(double offset)
        {
            _judgeOffset = offset;

            Debug.Log("JudgeOffsetを" + offset + "に変更しました");

            return _judgeOffset;
        }

        /// <summary>
        /// アニメーションオフセットを設定する
        /// プレー中の変更は不可能
        /// </summary>
        /// <param name="offset"></param>
        /// <returns></returns>
        public virtual double SetAnimationOffset(double offset)
        {
            _animationOffset = offset;

            Debug.Log("AnimationOffsetを" + offset + "に変更しました"); 

            return _animationOffset;
        }

        private async void StartObserveIsPlaying(Action<bool> act)
        {
            bool last = _audioPlayable.isPlaying;

            while(true)
            {
                if (last != _audioPlayable.isPlaying)
                {
                    act.Invoke(_audioPlayable.isPlaying);
                    break;
                }

                await Task.Delay(1);
            }
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(MusicGamePlayer))]
    public class MusicGamePlayerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var targetScript = target as MusicGamePlayer;

            EditorGUILayout.HelpBox("-----JudgeOffset----- \n 秒単位でのオフセットを加算 \n +で判定線より下、-で判定線より上に判定位置を移動", MessageType.Info);
            EditorGUILayout.HelpBox("-----AnimationOffset----- \n 秒単位でのオフセットを加算 \n +でより早く、-で判定線より下にノーツを移動", MessageType.Info);
        }
    }
#endif
}

