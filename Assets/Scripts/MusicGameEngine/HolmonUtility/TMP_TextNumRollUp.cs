using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace HolmonUtility
{
    public class TMP_TextNumRollUp : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;
        [SerializeField] private float _rollUpTime = 1.0f;
        [SerializeField] private int _decimalPlace = 0;
        [SerializeField] private bool _useComma = false;
        [SerializeField] private bool _useUnit = false;
        [SerializeField] private string _unit;

        private float _currentValue = 0;

        private void Start()
        {
            string numRaw = _text.text;
            if (_useComma)
            {
                numRaw = numRaw.Replace(",", "");
            }
            if (_useUnit)
            {
                numRaw = numRaw.Replace(_unit, "");
            }

            float num = 0;
            if (float.TryParse(numRaw, out num))
            {
                _currentValue = num;
            }
            else if(numRaw == "") _currentValue = 0;
            else
            {
                Debug.LogError("テキストの初期値が数字ではありません");
            }
        }

        public void Play(float targetValue)
        {
            StartCoroutine(Coroutine(targetValue));
        }

        public IEnumerator Coroutine(float targetValue)
        {
            double startT = Time.time;
            float startValue = _currentValue;

            while (true)
            {
                float progress = (float)((Time.time - startT) / _rollUpTime);

                if (progress > 1f) break;

                _currentValue = (int)Mathf.Lerp(startValue, targetValue, progress);

                _text.text = _useComma ? _currentValue.ToString("N" + _decimalPlace) : _currentValue.ToString("F" + _decimalPlace);
                _text.text += _useUnit ? _unit : "";

                yield return null;
            }

            _currentValue = (int)Mathf.Lerp(startValue, targetValue, 1);
            _text.text = _useComma ? _currentValue.ToString("N" + _decimalPlace) : _currentValue.ToString("F" + _decimalPlace);
            _text.text += _useUnit ? _unit : "";

            yield break;
        }

    }
}

