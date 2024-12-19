namespace MusicGameEngine
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using HolmonUtility;

    [RequireComponent(typeof(Transform))]
    public class Liner2DTapScroll : ScrollNoteObjectBase
    {
        private const float MAX_HS = 5.0f;

        [Header("���W�͑S�ă��[�J���w��")]
        [SerializeField] private Vector2 _scrollStartPoint;
        [SerializeField] private Vector2 _judgeLinePoint;
        //�n�C�X�s�[�h��bpm��1�̎��AStartPoint����judgeLine�܂ł̈ړ��ɂ����鎞��
        //TODO : Play()�̎��s�^�C�~���O�ɂ���������Ă���̂ŁA�ǂ����ɕ���������

        //_scrillStartPoint��_scrollEndPoint�̈ʒu��ω������邱�Ƃňړ����x�𐧌�
        private float _highSpeed = 1.0f;
        //�����Ȉړ����x�𐧌�
        private float _bpmRatio = 1.0f; //TODO : ���܂��@�\���ĂȂ�
        private double _scrollTime;

        //�O�̃t���[����AudioPlayingTime
        private double _prevPlayingTime;
        //�X�N���[���J�n����̑��o�ߎ��Ԃ��擾
        private double _scrollRatioNumerator = 0;
        //�X�N���[���̐i�s�x���擾
        private double _scrollProgress { get 
            {
                if (_scrollRatioNumerator < 0) return 0;
                return _scrollRatioNumerator / _scrollTime;
            } }
        //�I�u�W�F�N�g�̈ړ��x�N�g�����擾
        private Vector2 _scrollVector { get { return _scrollStartPoint - _judgeLinePoint; } }

        private double _scrillStartTime;
        //�n�C�X�s�[�h��bpmRatio���l�������ʒu���擾
        private Vector2 _notePosition
        {
            get
            {
                //TODO : BPMratio�͂����ɂ����ėǂ��̂��H
                Vector2 scrollStartPoint = (Vector2)_judgeLinePoint + (_scrollVector * _highSpeed);

                //
                //Debug.Log(_scrollProgress);
                Vector2 pos = Vector2Ext.OverLerp(scrollStartPoint, (Vector2)_judgeLinePoint, (float)_scrollProgress);

                return pos;
            }
        }

        private void Update()
        {
            //if (Input.GetKeyDown(KeyCode.L)) Play();

            if (_scrillStartTime != 0 && _prevPlayingTime != 0)
            {
                _scrollRatioNumerator += _bpmRatio * (AudioPlayingTime.PlayingTime - _prevPlayingTime);
                this.transform.localPosition = _notePosition;
            }
            
            _prevPlayingTime = AudioPlayingTime.PlayingTime;
        }

        public override void Play(double animationTime, double animationOffset, params double[] correctInputTimes)
        {
            _scrollTime = animationTime;
            _scrollRatioNumerator = animationOffset * _bpmRatio;
            this.gameObject.SetActive(true);
            _scrillStartTime = AudioPlayingTime.PlayingTime;
            //Debug.Log("�m�[�g�A�j���[�V�����̍Đ����J�n");
        }

        public override void SetBpmRatio(float ratio)
        {
            if(ratio < 0)
            {
                Debug.LogError("�ݒ肳�ꂽBPM��͖����Ȓl�ł�");
                return;
            }

            _bpmRatio = ratio;
        }

        public override void SetHS(float highSpeed)
        {
            if(highSpeed < 1 || highSpeed > MAX_HS)
            {
                Debug.Log("�ݒ肳�ꂽ�n�C�X�s�[�h�͖����Ȓl�ł�");
                return;
            }

            _highSpeed = highSpeed;
        }

        protected override void Init()
        {
            _prevPlayingTime = 0;
            _scrollRatioNumerator = 0;
            _scrillStartTime = 0;

            this.transform.localPosition = _scrollStartPoint;

            //Debug.Log("Liner2DScroll�͏���������܂���");
        }

        public override void ReceiveJudgeCallback(int judge)
        {
            //throw new System.NotImplementedException();
        }
    }
}