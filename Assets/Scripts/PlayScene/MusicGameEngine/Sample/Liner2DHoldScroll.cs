namespace MusicGameEngine
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using HolmonUtility;

    [RequireComponent(typeof(Transform))]
    public class Liner2DHoldScroll : ScrollNoteObjectBase
    {
        private const float MAX_HS = 5.0f;

        [SerializeField] protected GameObject _startNote;
        [SerializeField] protected GameObject _endNote;
        [SerializeField] protected GameObject _fillNote;
        [SerializeField] protected GameObject _mask;
        [Header("座標は全てローカル指定")]
        [SerializeField] private Vector2 _scrollStartPoint;
        [SerializeField] private Vector2 _judgeLinePoint;
        //ハイスピードとbpmが1の時、StartPointからjudgeLineまでの移動にかかる時間
        //TODO : Play()の実行タイミングにもかかわってくるので、どこかに分離したい

        //_scrillStartPointと_scrollEndPointの位置を変化させることで移動速度を制御
        private float _highSpeed = 1.0f;
        //純粋な移動速度を制御
        protected float _bpmRatio = 1.0f; //TODO : うまく機能してない
        protected double _scrollTime;
        protected double[] _correctInputTimes;

        //前のフレームのAudioPlayingTime
        protected double _prevPlayingTime;
        //スクロール開始からの総経過時間を取得
        protected double _scrollRatioNumerator = 0;
        //スクロールの進行度を取得
        protected double _scrollProgress { get 
            {
                if (_scrollRatioNumerator < 0) return 0;
                return _scrollRatioNumerator / _scrollTime;
            } }
        //オブジェクトの移動ベクトルを取得
        protected Vector2 _scrollVector { get { return _scrollStartPoint - _judgeLinePoint; } }

        protected bool _isLongNote { get 
            {
                if (_correctInputTimes == null) return false;

                return _correctInputTimes.Length > 1;
            } }

        protected double _scrollStartTime;
        //ハイスピードとbpmRatioを考慮した位置を取得
        protected Vector2 _notePosition
        {
            get
            {
                Vector2 scrollStartPoint = (Vector2)_judgeLinePoint + (_scrollVector * _highSpeed);
                Vector2 pos = Vector2Ext.OverLerp(scrollStartPoint, (Vector2)_judgeLinePoint, (float)_scrollProgress);

                return pos;
            }
        }
        protected Vector2 _endNoteDiff
        {
            get
            {
                if (!_isLongNote) return Vector2.zero;

                Vector2 scrollStartPoint = (Vector2)_judgeLinePoint + (_scrollVector * _highSpeed);
                //スクロールTとホールドTの比率を求める
                double r = (_correctInputTimes[1] - _correctInputTimes[0]) / _scrollTime;
                Vector2 scrollV = _scrollVector * _highSpeed;
                return scrollV * (float)r;
            }
        }

        protected bool _isPushing = false;
        protected float _pushingTime = 0;
        protected float _pushingProgress { get { return _pushingTime / (float)(_correctInputTimes[1] - _correctInputTimes[0]); } }

        private void Awake()
        {
            _endNote.SetActive(false);
            _fillNote.SetActive(false);
        }

        protected virtual void Update()
        {
            //if (Input.GetKeyDown(KeyCode.L)) Play();

            if (_scrollStartTime != 0 && _prevPlayingTime != 0)
            {
                _scrollRatioNumerator += _bpmRatio * (AudioPlayingTime.PlayingTime - _prevPlayingTime);
                this.transform.localPosition = _notePosition;
                if(_isLongNote)
                {
                    if(_isPushing) _pushingTime = (float)(AudioPlayingTime.PlayingTime - _correctInputTimes[0]);

                    _endNote.transform.localPosition = _endNoteDiff;
                    _fillNote.transform.localPosition = _endNoteDiff/2;

                    float hfx = _endNoteDiff.x == 0 ? 0 : _endNote.transform.localScale.x / 2;
                    float hfy = _endNoteDiff.y == 0 ? 0 : _endNote.transform.localScale.x / 2;
                    _mask.transform.localPosition = (_endNoteDiff / 2) + _pushingProgress * (_endNoteDiff);

                    float lsx = _endNoteDiff.x == 0 ? _fillNote.transform.localScale.x : _endNoteDiff.x;
                    float lsy = _endNoteDiff.y == 0 ? _fillNote.transform.localScale.y : _endNoteDiff.y;

                    if(_fillNote.transform.localScale != new Vector3(lsx, lsy, 1.0f))
                    {
                        _fillNote.transform.localScale = new Vector3(lsx, lsy, 1.0f);

                        _mask.transform.localScale = new Vector3(lsx + hfx, lsy + hfy, 1.0f);
                    }

                }

                double r = (_correctInputTimes[1] - _correctInputTimes[0]) / _scrollTime;
            }

            //押しはじめ、頭ノーツ削除
            if(_pushingProgress != 0)
            {
                _startNote.SetActive(false);
            }
            //押し終わり、尻ノーツ削除
            if (_pushingProgress == 1)
            {
                _endNote.SetActive(false);
            }

            _prevPlayingTime = AudioPlayingTime.PlayingTime;
        }

        public override void Play(double animationTime, double animationOffset, params double[] correctInputTimes)
        {
            _isPushing = false;
            _pushingTime = 0;
            _scrollTime = animationTime;
            _scrollRatioNumerator = animationOffset * _bpmRatio;
            this.gameObject.SetActive(true);
            _scrollStartTime = AudioPlayingTime.PlayingTime;
            _correctInputTimes = correctInputTimes;

            if(_isLongNote)
            {
                _endNote.SetActive(true);
                _fillNote.SetActive(true);
                float lsx = _endNoteDiff.x == 0 ? _fillNote.transform.localScale.x : _endNoteDiff.x;
                float lsy = _endNoteDiff.y == 0 ? _fillNote.transform.localScale.y : _endNoteDiff.y;
                _fillNote.transform.localScale = new Vector3(lsx, lsy, 1.0f);

                float hfx = _endNoteDiff.x == 0 ? 0 : _endNote.transform.localScale.x/2;
                float hfy = _endNoteDiff.y == 0 ? 0 : _endNote.transform.localScale.y / 2;
                _mask.transform.localScale = new Vector3(lsx + hfx, lsy + hfy, 1.0f);
            }
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
            _scrollStartTime = 0;

            this.transform.localPosition = _scrollStartPoint;

            _correctInputTimes = null;
            _endNote.SetActive(false);
            _fillNote.SetActive(false);

            //Debug.Log("Liner2DScrollは初期化されました");
        }

        public override void ReceiveJudgeCallback(int judge)
        {
            if(!_isPushing)
            {
                if(judge == 0)
                {
                    //ミスしたときのノートアニメーション
                }
                else
                {
                    _isPushing = true;
                }
            }
            if(_isPushing)
            {
                if(judge == 0)
                {
                    //ミスしたときのノートアニメーション

                    _isPushing = false;
                }
            }
        }
    }
}