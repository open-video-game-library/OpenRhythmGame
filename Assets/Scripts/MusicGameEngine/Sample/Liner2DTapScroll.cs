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

        [Header("座標は全てローカル指定")]
        [SerializeField] private Vector2 _scrollStartPoint;
        [SerializeField] private Vector2 _judgeLinePoint;
        //ハイスピードとbpmが1の時、StartPointからjudgeLineまでの移動にかかる時間
        //TODO : Play()の実行タイミングにもかかわってくるので、どこかに分離したい

        //_scrillStartPointと_scrollEndPointの位置を変化させることで移動速度を制御
        private float _highSpeed = 1.0f;
        //純粋な移動速度を制御
        private float _bpmRatio = 1.0f; //TODO : うまく機能してない
        private double _scrollTime;

        //前のフレームのAudioPlayingTime
        private double _prevPlayingTime;
        //スクロール開始からの総経過時間を取得
        private double _scrollRatioNumerator = 0;
        //スクロールの進行度を取得
        private double _scrollProgress { get 
            {
                if (_scrollRatioNumerator < 0) return 0;
                return _scrollRatioNumerator / _scrollTime;
            } }
        //オブジェクトの移動ベクトルを取得
        private Vector2 _scrollVector { get { return _scrollStartPoint - _judgeLinePoint; } }

        private double _scrillStartTime;
        //ハイスピードとbpmRatioを考慮した位置を取得
        private Vector2 _notePosition
        {
            get
            {
                //TODO : BPMratioはここにかけて良いのか？
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
            //Debug.Log("ノートアニメーションの再生を開始");
        }

        public override void SetBpmRatio(float ratio)
        {
            if(ratio < 0)
            {
                Debug.LogError("設定されたBPM比は無効な値です");
                return;
            }

            _bpmRatio = ratio;
        }

        public override void SetHS(float highSpeed)
        {
            if(highSpeed < 1 || highSpeed > MAX_HS)
            {
                Debug.Log("設定されたハイスピードは無効な値です");
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

            //Debug.Log("Liner2DScrollは初期化されました");
        }

        public override void ReceiveJudgeCallback(int judge)
        {
            //throw new System.NotImplementedException();
        }
    }
}