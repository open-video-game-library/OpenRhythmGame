using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HolmonUtility
{
    using UnityEngine;

    [RequireComponent(typeof(CanvasGroup))]
    public class CanvasGroupFader : MonoBehaviour
    {
        // �A�j���[�V�����ɂ����鎞��
        [SerializeField] private float _duration = 1.0f;

        // alpha�̕ω����@���w�肷��A�j���[�V�����J�[�u
        [SerializeField] private AnimationCurve _alphaCurve;

        // �����ŊǗ�����CanvasGroup
        private CanvasGroup _canvasGroup;

        // �A�j���[�V�����̌o�ߎ��Ԃ�ǐՂ���ϐ�
        private float _elapsedTime = 0f;

        // �A�j���[�V���������s�����ǂ������Ǘ�����t���O
        private bool _isPlaying = false;

        void Awake()
        {
            // CanvasGroup�̃R���|�[�l���g���擾
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        // �A�j���[�V�������J�n���郁�\�b�h
        public void Play()
        {
            // �o�ߎ��Ԃ����Z�b�g
            _elapsedTime = 0f;
            _isPlaying = true;
        }

        void Update()
        {
            if (_isPlaying)
            {
                // �o�ߎ��Ԃ��X�V
                _elapsedTime += Time.deltaTime;

                // 0����1�܂ł̒l���v�Z (�o�ߎ��� / �A�j���[�V��������)
                float t = Mathf.Clamp01(_elapsedTime / _duration);

                // �A�j���[�V�����J�[�u�ɏ]����alpha�l��ݒ�
                _canvasGroup.alpha = _alphaCurve.Evaluate(t);

                // �A�j���[�V�������I��������isPlaying��false�ɂ���
                if (t >= 1f)
                {
                    _isPlaying = false;
                }
            }
        }
    }

}