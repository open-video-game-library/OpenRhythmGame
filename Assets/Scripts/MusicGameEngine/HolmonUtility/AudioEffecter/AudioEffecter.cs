namespace HolmonUtility.AudioEffecter
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Audio;
    using UnityEngine.EventSystems;
    using UnityEngine.UI;

    public abstract class AudioEffector : MonoBehaviour
    {
        [Serializable]
        public class EffectButton
        {
            public EventTrigger button;
            public float divBeatDuration; //1�������������邩
        }

        [SerializeField] protected float _bpm;
        [SerializeField] protected float _musicOffset;
        [SerializeField] private AudioMixer _audioMixer;
        [SerializeField] private Slider _lowcutSlider;
        [SerializeField] private Slider _highcutSlider;
        [SerializeField] private EventTrigger _flangeButton;
        [SerializeField] private List<EffectButton> _loopButtons;
        [SerializeField] private List<EffectButton> _cutButtons;

        protected const float CUT_MIN = 10;
        protected const float CUT_MAX = 22000;

        private const float FLANGE_DRY_ON = 0.7f;
        private const float FLANGE_DRY_OFF = 1.0f;
        private const float FLANGE_WET_ON = 0.9f;
        private const float FLANGE_WET_OFF = 0;

        //�f�t�H���g�̐ݒ�l���v���p�e�B�Ō������
        public float DefaultLowcutValue { get; private set; }
        public float DefaultHighcutValue { get; private set; }

        public float BPM => _bpm;
        //TODO : ����oneBeatTime����BPM�ω��ɑΉ��o���Ȃ�
        //       float nowDivBeat = (int)Mathf.Floor(nowTime / oneDivBeatTime);���o�O��
        public float oneBeatTime { get { return 60f / _bpm; } }


        protected virtual void Start()
        {
            if (_lowcutSlider)
            {
                _lowcutSlider.onValueChanged.AddListener(SetLowcutValue);
                Debug.Log(_lowcutSlider.gameObject.name + "�̑���ɂ���ă��[�J�b�g�𐧌�\");
            }
            if (_highcutSlider)
            {
                _highcutSlider.onValueChanged.AddListener(SetHighcutValue);
                Debug.Log(_highcutSlider.gameObject.name + "�̑���ɂ���ăn�C�J�b�g�𐧌�\");
            }

            _flangeButton.triggers.Add(
                EventTriggerGenerator.Generate
                    (
                        EventTriggerType.PointerDown,
                        () => OnFlange()
                    )
                );
            _flangeButton.triggers.Add(
                EventTriggerGenerator.Generate
                    (
                        EventTriggerType.PointerUp,
                        () => OffFlange()
                    )
                );

            for (int i = 0; i < _loopButtons.Count; i++)
            {
                float div = _loopButtons[i].divBeatDuration;
                _loopButtons[i].button.triggers.Add(
                    EventTriggerGenerator.Generate
                    (
                        EventTriggerType.PointerDown,
                        () => OnLoop(div)
                    )
                );
                _loopButtons[i].button.triggers.Add(
                    EventTriggerGenerator.Generate
                    (
                        EventTriggerType.PointerUp,
                        () => OffLoop()
                    )
                );
                Debug.Log(_loopButtons[i].button.gameObject.name + "�̑���ɂ����" + div + "���̃��[�v�𐧌�\");
            }
            for (int i = 0; i < _cutButtons.Count; i++)
            {
                float div = _cutButtons[i].divBeatDuration;
                _cutButtons[i].button.triggers.Add(
                    EventTriggerGenerator.Generate
                    (
                        EventTriggerType.PointerDown,
                        () => OnCut(div)
                    )
                );
                _cutButtons[i].button.triggers.Add(
                    EventTriggerGenerator.Generate
                    (
                        EventTriggerType.PointerUp,
                        () => OffCut()
                    )
                );
                Debug.Log(_cutButtons[i].button.gameObject.name + "�̑���ɂ����" + div + "���̃J�b�g�𐧌�\");
            }


            //----- InitUI -----
            float nowLowcut = 0;
            if(_audioMixer.GetFloat("Lowpass", out nowLowcut))
            {
                _lowcutSlider.value = Mathf.InverseLerp(CUT_MIN, CUT_MAX, nowLowcut);
            }
            float nowHighcut = 0;
            if (_audioMixer.GetFloat("Highpass", out nowHighcut))
            {
                _highcutSlider.value = Mathf.InverseLerp(CUT_MIN, CUT_MAX, nowHighcut);
            }

            //----- Set Default Value -----
            float defLowcut = 0;
            if (_audioMixer.GetFloat("Lowpass", out defLowcut))
            {
                DefaultLowcutValue = Mathf.InverseLerp(10 ,22000, defLowcut);
            }

            float defHighcut = 0;
            if (_audioMixer.GetFloat("Highpass", out defHighcut))
            {
                DefaultHighcutValue = Mathf.InverseLerp(10, 22000, defHighcut);
            }
        }

        public void SetLowcutValue(float value)
        {
            float v = Mathf.Lerp(CUT_MIN, CUT_MAX, value);
            _audioMixer.SetFloat("Lowpass", v);
        }

        public void SetHighcutValue(float value)
        {
            float v = Mathf.Lerp(CUT_MIN, CUT_MAX, value);
            _audioMixer.SetFloat("Highpass", v);
        }

        public abstract void OnLoop(float divBeatDuration); //���q, ����
        public abstract void OnLoop(float divBeatDuration, float loopStartClipTime); //���̃X�^�[�g�^�C���́A�I�[�f�B�I�N���b�v�̈ʒu���w�肷��
        public abstract void OffLoop();

        public abstract void OnCut(float divBeatDuration);
        public abstract void OnCut(float divBeatDuration, float cutStartClipTime);
        public abstract void OffCut();

        public void OnFlange()
        {
            _audioMixer.SetFloat("Flange_dry", FLANGE_DRY_ON);
            _audioMixer.SetFloat("Flange_wet", FLANGE_WET_ON);
        }
        public void OffFlange()
        {
            _audioMixer.SetFloat("Flange_dry", FLANGE_DRY_OFF);
            _audioMixer.SetFloat("Flange_wet", FLANGE_WET_OFF);
        }

        public void SetBPM(float bpm)
        {
            _bpm = bpm;
        }
        public void SetMusicOffset(float musicOffset)
        {
            _musicOffset = musicOffset;
        }
    }

}