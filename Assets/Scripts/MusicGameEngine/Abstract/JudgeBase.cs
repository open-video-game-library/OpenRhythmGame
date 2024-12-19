namespace MusicGameEngine
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using UnityEditor;
    using UnityEngine;
    using HolmonUtility;
    using System.Linq;
    using UnityEngine.UIElements;

    //������Mono�ł��悭�ˁH
    //����̊��N���X
    public abstract�@class JudgeBase : MonoBehaviour
    {
        protected double[] _correctInputTimes { get; private set; } = new double[] { };

        protected Dictionary<string, Action<int, float, bool>> _judgeCallBack = new Dictionary<string, Action<int, float, bool>>(); //���茋�ʂ�ʒm����
        protected Dictionary<string, Action> _completeCallBack = new Dictionary<string, Action>(); //�m�[�g���������I�������Ƃ�ʒm����

        // ���y�̍Đ����ԂƐ��������͎��ԂƂ̍������擾����
        protected double[] _timingDiscrepancies
        {
            get
            {
                double[] discrepancies = new double[_correctInputTimes.Length];
                for (int i = 0; i < _correctInputTimes.Length; i++)
                {
                    discrepancies[i] = AudioPlayingTime.PlayingTime - _correctInputTimes[i];
                }
                return discrepancies;
            }
        }

        protected float[] _additionalInfo;
        protected bool _endJudgeProgress = false;
        public bool endJudgeProgress => _endJudgeProgress;

        protected abstract void Init();

        /// <summary>
        /// �����Ăق����^�C�~���O��ݒ肷��
        /// </summary>
        /// <param name="correctInputTime"></param>
        public void SetCorrectInputTimes(params double[] correctInputTimes)
        {
            _endJudgeProgress = false;
            _correctInputTimes = correctInputTimes;

            Init();
        }

        /// <summary>
        /// note�ɃZ�b�g���ꂽAdditional�����擾����
        /// </summary>
        /// <param name="additionalInfo"></param>
        public void SetAdditionalInfo(float[] additionalInfo)
        {
            _additionalInfo = additionalInfo;
        }

        protected virtual void Update()
        {
            if(_correctInputTimes.Length == 0)
            {
                Debug.LogError("CorrectInputTime�̐ݒ肪�������Ă��܂���");
                return;
            }

            if(!_endJudgeProgress) ThroughJudge();

            //���̃I�u�W�F�N�g���j������Ȃ��܂ܕ��u����Ă����ꍇ�́A�����Ŕj������?
            if (_timingDiscrepancies.Length != 0 && 10 < _timingDiscrepancies.Max())
            {
                Debug.LogWarning("�j�����ꂸ���u����Ă���\���̂��锻��I�u�W�F�N�g�����݂��܂�");
            }

#if UNITY_EDITOR
            if (!EditorApplication.isPlaying) return;
#endif
        }

        /// <summary>
        /// ������󂯎��R�[���o�b�N��ݒ肷��
        /// </summary>
        /// <param name="judgeCallBack"></param>
        /// <returns></returns>
        public string AssignJudgeResultCallback(Action<int, float, bool> judgeCallBack)
        {
            string key = RandomStringGenerator.Generate(16);
            this._judgeCallBack.Add(key, judgeCallBack);
            return key;
        }
        /// <summary>
        /// ������󂯎��R�[���o�b�N����������
        /// </summary>
        /// <param name="key"></param>
        public void RemoveJudgeResultCallback(string key)
        {
            bool res = this._judgeCallBack.Remove(key);
            if (!res) Debug.LogWarning("�R�[���o�b�N�ꗗ�ɓo�^����Ă��Ȃ��R�[���o�b�N���������悤�Ƃ��܂���");
        }
        /// �m�[�g�I�u�W�F�N�g���������I�������Ƃ��󂯎��R�[���o�b�N��ݒ肷��
        /// </summary>
        /// <param name="completeCallBack"></param>
        /// <returns></returns>
        public string AssignNotesObjectCompleteCallBack(Action completeCallBack)
        {
            string key = RandomStringGenerator.Generate(16);
            this._completeCallBack.Add(key, completeCallBack);
            return key;
        }
        /// <summary>
        /// �m�[�g�I�u�W�F�N�g���������I�������Ƃ��󂯎��R�[���o�b�N����������
        /// </summary>
        /// <param name="key"></param>
        public void RemoveNotesObjectCompleteCallBack(string key)
        {
            bool res = this._completeCallBack.Remove(key);
            if (!res) Debug.LogWarning("�R�[���o�b�N�ꗗ�ɓo�^����Ă��Ȃ��R�[���o�b�N���������悤�Ƃ��܂���");
        }

        /// <summary>
        /// �C�ӂ̃^�C�~���O�Ŕ��菈�����s��
        /// </summary>
        public abstract void Judge(eInputType type);

        /// <summary>
        /// ���菈���̈ꎞ��~���s��
        /// </summary>
        /// <param name="pause"></param>
        public abstract void Pause(bool pause);

        //�R�[���o�b�N���Ăяo��
        protected void ExecuteJudgeCallback(int judge, float diff, bool end)
        {
            if (_endJudgeProgress)
            {
                Debug.LogWarning("���̃m�[�g�I�u�W�F�N�g�̔��菈���͊��ɏI����Ă��܂�");
                return;
            }

            if (end) //����Ȃ炢���邩�E�E�H
            {
                if (_endJudgeProgress)
                {
                    Debug.LogError("�ق�");
                }

                //�lA�ύX
                _endJudgeProgress = true;
            }

            foreach (var action in _judgeCallBack.Values)
            {
                //�A�N�V����A
                action(judge, diff, end);
            }

            /*
            if(end)
            {
                if(_endJudgeProgress)
                {
                    Debug.LogError("�ق�");
                }

                //�lA�ύX
                _endJudgeProgress = true;
            }
            */
        }
        private void ExecuteNotesObjectCompleteCallback()
        {
            foreach (var action in _completeCallBack.Values)
            {
                action();
            }
        }

        protected void ProgressComplete()
        {
            if(!_endJudgeProgress)
            {
                Debug.LogWarning("�m�[�g�̔��菈�����I����Ă��Ȃ��̂ɂ�����炸�C�m�[�c�𖳌������悤�Ƃ��Ă��܂�");
            }

            ExecuteNotesObjectCompleteCallback();
        }

        //�X���[���菈�����s��
        protected abstract void ThroughJudge();

        private async void OnDisable()
        {
            _judgeCallBack.Clear();
            _correctInputTimes = new double[] { };

            await Task.Delay(2);

            _completeCallBack.Clear();
        }

    }
}


