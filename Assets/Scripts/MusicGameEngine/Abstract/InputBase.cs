namespace MusicGameEngine
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Security.Cryptography;
    using UnityEngine;
    using HolmonUtility;
    
    public enum eInputType
    {
        On,
        Off
    }

    //���͂̊��N���X
    public abstract class InputBase : MonoBehaviour
    {
        protected Dictionary<string, Action> _inputOnCallBack = new Dictionary<string, Action>();
        protected Dictionary<string, Action> _inputOffCallBack = new Dictionary<string, Action>();

        public bool inputable { get; private set; } = true;

        /// <summary>
        /// ���͂�ON�ɂȂ������ɌĂ΂��R�[���o�b�N��ݒ肷��
        /// </summary>
        /// <param name="inputOnCallBack"></param>
        /// <returns></returns>
        public string AssignInputOnCallback(Action inputOnCallBack)
        {
            string key = RandomStringGenerator.Generate(16);
            this._inputOnCallBack.Add(key, inputOnCallBack);
            return key;
        }
        /// <summary>
        /// ���͂�OFF�ɂȂ������ɌĂ΂��R�[���o�b�N��ݒ肷��
        /// </summary>
        /// <param name="inputOffCallBack"></param>
        /// <returns></returns>
        public string AssignInputOffCallback(Action inputOffCallBack)
        {
            string key = RandomStringGenerator.Generate(16);
            this._inputOffCallBack.Add(key, inputOffCallBack);
            return key;
        }

        /// <summary>
        /// ���͂�ON�ɂȂ������ɌĂ΂��R�[���o�b�N����������
        /// </summary>
        /// <param name="key"></param>
        public void RemoveInputOnCallback(string key)
        {
            bool res = this._inputOnCallBack.Remove(key);
            if (!res) Debug.LogWarning("�R�[���o�b�N�ꗗ�ɓo�^����Ă��Ȃ��R�[���o�b�N���������悤�Ƃ��܂���");
        }
        /// <summary>
        /// ���͂�OFF�ɂȂ������ɌĂ΂��R�[���o�b�N����������
        /// </summary>
        /// <param name="key"></param>
        public void RemoveInputOffCallback(string key)
        {
            bool res = this._inputOffCallBack.Remove(key);
            if (!res) Debug.LogWarning("�R�[���o�b�N�ꗗ�ɓo�^����Ă��Ȃ��R�[���o�b�N���������悤�Ƃ��܂���");
        }

        /// <summary>
        /// ���̃C���v�b�g�����͉\���ǂ�����؂�ւ���
        /// </summary>
        /// <param name="inputable"></param>
        public void SetInputable(bool inputable)
        {
            this.inputable = inputable;
        }

        //�R�[���o�b�N���Ăяo��
        protected void ExecuteInputOnCallback()
        {
            if (!inputable) return;

            foreach (var value in _inputOnCallBack.Values)
            {
                value();
            }
        }
        protected void ExecuteInputOffCallback()
        {
            if (!inputable) return;

            foreach (var value in _inputOffCallBack.Values)
            {
                try
                {
                    value();
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                }
            }
        }

        private void OnDisable()
        {
            _inputOnCallBack.Clear();
            _inputOffCallBack.Clear();
            Debug.Log("InputBase�͏���������܂���");
        }
    }
}