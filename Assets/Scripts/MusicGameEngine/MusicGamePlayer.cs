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
                    Debug.LogError(_NotesGroupManagerObject.name + "��NotesGroupManager���A�^�b�`����Ă��܂���");
                }
                foreach (var obj in _extendNoteReactionBaseObjects)
                {
                    try
                    {
                        var extend = obj.GetComponent<ExtendNoteReactionBase>();
                        _extendNoteReactions.Add(extend);
                    }catch
                    {
                        Debug.LogError(obj.name + "��ExtendNoteReactionBase���A�^�b�`����Ă��܂���");
                    }

                }
            }
        }

        [Header("���W�b�N")]
        [SerializeField] private GameObject _audioPlayableLogic;

        [SerializeField] private List<NotesGroup> _notesGroups;
        [SerializeField] private List<GameObject> _extendPlayReactionBaseObjects;

        [Header("�p�����[�^�[")]
        [SerializeField] protected double _judgeOffset = 0.0;
        [SerializeField] protected double _animationOffset = 0.0;
        [SerializeField] protected float _highSpeedIncrement = 0.5f; //�n�C�X�s�[�h�l�̑�����
        [SerializeField] protected float _highSpeedMax = 5.0f; //�n�C�X�s�[�h�̍ő�l
        [SerializeField] protected float _highSpeedMin = 1f; //�n�C�X�s�[�h�̍ŏ��l
        [SerializeField, ReadOnly] protected ScoreContainer _scoreContainer;
        public ScoreContainer scoreContainer => _scoreContainer;

        private IAudioPlayable _audioPlayable;

        //�v���p�e�B
        //==============================================================================================================

        public double HighSpeed => _notesGroups[0]._notesGroupManager.HS;
        public double JudgeOffset => _judgeOffset;
        public double AnimationOffset => _animationOffset;

        //���\�b�h
        //==============================================================================================================

        protected virtual void Awake()
        {
            try
            {
                _audioPlayable = _audioPlayableLogic.GetComponent<IAudioPlayable>();
            }
            catch
            {
                Debug.LogError(_audioPlayableLogic.name + "��IAudioPlayable���A�^�b�`����Ă��܂���");
                return;
            }

            foreach (var notesGroup in _notesGroups)
            {
                notesGroup.GetComponent();
            }
        }

        /// <summary>
        /// ���ʂ̍Đ����J�n����
        /// </summary>
        public void Play(ScoreContainer scoreContainer)
        {
            _scoreContainer = scoreContainer;
            _audioPlayable.Play(scoreContainer.Song);

            //await Task.Delay(Mathf.RoundToInt((float)AudioPlayingTime.PLAY_DELAY *1000)); //PLAY_DELAY���̑ҋ@���s��

            var score = scoreContainer.GetScore();

            foreach (var noteGroup in _notesGroups)
            {
                foreach(var extend in noteGroup._extendNoteReactions)
                {
                    extend.SetScoreContainer(scoreContainer);
                }

                //����Colimn�̃m�[�g���擾
                var s = score.Notes.Where(n => n.Column == noteGroup._columnNum).ToArray();
                ScoreData newData = new ScoreData(score.Speeds, s, score.Offset);

                noteGroup._notesGroupManager.SetExtendNoteReactionBases(noteGroup._extendNoteReactions.ToArray());
                noteGroup._notesGroupManager.SetJudgeOffset(_judgeOffset);
                noteGroup._notesGroupManager.SetDisplayOffset(_animationOffset);

                noteGroup._notesGroupManager.Play(newData);
            }

            foreach(var extendPlayReaction in _extendPlayReactionBaseObjects)
            {
                if (extendPlayReaction == null) Debug.LogError("�I�u�W�F�N�g���A�^�b�`����Ă��܂���");
                if (!extendPlayReaction.GetComponent<ExtendPlayReactionBase>()) Debug.LogError(extendPlayReaction.name + "��ExtendPlayReactionBase���A�^�b�`����Ă��܂���");

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
                            Debug.LogError(extendPlayReaction.name + "��ExtendPlayReactionBase���A�^�b�`����Ă��܂���");
                        }

                        component.EndPlay();
                    }
                }
                else
                {
                    Debug.LogError("�Ȃ̍Đ��󋵂ɗ�O�������Ă���\��������܂�");
                }
            });
        }

        /// <summary>
        /// �n�C�X�s�[�h��ύX����
        /// �ύX��̃n�C�X�s�[�h��Ԃ�l�Ƃ��ēn��
        /// </summary>
        /// <param name="up"></param>
        /// <returns></returns>
        public virtual float SetHS(bool up)
        {
            if (_notesGroups.Count == 0)
            {
                Debug.LogError("NotesGroupManager������A�^�b�`����Ă��܂���");
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
                    Debug.LogError(extendPlayReaction.name + "��ExtendPlayReactionBase���A�^�b�`����Ă��܂���");
                }

                component.SetHS(to);
            }

            Debug.Log("HS��" + to + "�ɕύX");

            return to;
        }

        /// <summary>
        /// ����I�t�Z�b�g��ݒ肷��
        /// �v���[���̕ύX�͕s�\
        /// </summary>
        /// <param name="offset"></param>
        public virtual double SetJudgeOffset(double offset)
        {
            _judgeOffset = offset;

            Debug.Log("JudgeOffset��" + offset + "�ɕύX���܂���");

            return _judgeOffset;
        }

        /// <summary>
        /// �A�j���[�V�����I�t�Z�b�g��ݒ肷��
        /// �v���[���̕ύX�͕s�\
        /// </summary>
        /// <param name="offset"></param>
        /// <returns></returns>
        public virtual double SetAnimationOffset(double offset)
        {
            _animationOffset = offset;

            Debug.Log("AnimationOffset��" + offset + "�ɕύX���܂���"); 

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

            EditorGUILayout.HelpBox("-----JudgeOffset----- \n �b�P�ʂł̃I�t�Z�b�g�����Z \n +�Ŕ������艺�A-�Ŕ��������ɔ���ʒu���ړ�", MessageType.Info);
            EditorGUILayout.HelpBox("-----AnimationOffset----- \n �b�P�ʂł̃I�t�Z�b�g�����Z \n +�ł�葁���A-�Ŕ������艺�Ƀm�[�c���ړ�", MessageType.Info);
        }
    }
#endif
}

