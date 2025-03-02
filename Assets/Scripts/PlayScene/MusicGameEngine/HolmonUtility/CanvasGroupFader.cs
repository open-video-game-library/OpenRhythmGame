using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HolmonUtility
{
    using UnityEngine;

    [RequireComponent(typeof(CanvasGroup))]
    public class CanvasGroupFader : MonoBehaviour
    {
        // アニメーションにかかる時間
        [SerializeField] private float _duration = 1.0f;

        // alphaの変化方法を指定するアニメーションカーブ
        [SerializeField] private AnimationCurve _alphaCurve;

        // 内部で管理するCanvasGroup
        private CanvasGroup _canvasGroup;

        // アニメーションの経過時間を追跡する変数
        private float _elapsedTime = 0f;

        // アニメーションが実行中かどうかを管理するフラグ
        private bool _isPlaying = false;

        void Awake()
        {
            // CanvasGroupのコンポーネントを取得
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        // アニメーションを開始するメソッド
        public void Play()
        {
            // 経過時間をリセット
            _elapsedTime = 0f;
            _isPlaying = true;
        }

        void Update()
        {
            if (_isPlaying)
            {
                // 経過時間を更新
                _elapsedTime += Time.deltaTime;

                // 0から1までの値を計算 (経過時間 / アニメーション時間)
                float t = Mathf.Clamp01(_elapsedTime / _duration);

                // アニメーションカーブに従ってalpha値を設定
                _canvasGroup.alpha = _alphaCurve.Evaluate(t);

                // アニメーションが終了したらisPlayingをfalseにする
                if (t >= 1f)
                {
                    _isPlaying = false;
                }
            }
        }
    }

}