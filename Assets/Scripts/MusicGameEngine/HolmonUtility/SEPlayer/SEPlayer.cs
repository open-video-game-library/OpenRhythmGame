using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace HolmonUtility
{
    public static class GlobalSEPlayer
    {
        public static void PlaySE(string key, int channnel = 0)
        {
            //�q�G�����L�[����SEPlayer��T��
            SEPlayer sePlayer = GameObject.FindObjectOfType<SEPlayer>();

            if (sePlayer == null)
            {
                Debug.LogError("SEPlayer��������܂���ł���");
                return;
            }

            sePlayer.PlaySE(key, channnel);
        }
    }

    public class SEPlayer : MonoBehaviour
    {
        [Serializable]
        public class SEs
        {
            public string key;
            public AudioClip clip;
        }

        [SerializeField] private List<SEs> _serializedSeList;
        [SerializeField] private int _channelLimit = 3;
        [SerializeField] private GameObject _channelPrefab;

        private List<AudioSource> _channnels = new List<AudioSource>();
        Dictionary<string, AudioClip> _seList { get
            {
                //SEs�N���X�̃��X�g��Dictionary�ɕϊ�����
                Dictionary<string, AudioClip> seList = new Dictionary<string, AudioClip>();
                foreach (var se in _serializedSeList)
                {
                    seList.Add(se.key, se.clip);
                }
                return seList;
            } }

        private void Start()
        {
            CheckList();
            GenerrateChannels();
        }

        //�����L�[��SE������ꍇ�̓G���[���o��
        private void CheckList()
        {
            HashSet<string> hashSet = new HashSet<string>();
            foreach (var se in _seList)
            {
                if (hashSet.Contains(se.Key))
                {
                    Debug.LogError("������SE�����݂��܂� : " + se.Key);
                }
                else
                {
                    hashSet.Add(se.Key);
                }
            }
        }
        //�`�����l���̐������s��
        private void GenerrateChannels()
        {
            //_channelLimit����_channelPrefab���q�I�u�W�F�N�g�Ƃ��Đ�������
            for (int i = 0; i < _channelLimit; i++)
            {
                GameObject channel = Instantiate(_channelPrefab, this.transform);
                _channnels.Add(channel.GetComponent<AudioSource>());
            }
        }

        //SE���Đ�����
        public void PlaySE(string key, int channnel = 0)
        {
            //channnel��0�����̏ꍇ�̓G���[���o��
            if (channnel < 0)
            {
                Debug.LogError("�`�����l���ԍ����s���ł� : " + channnel);
                return;
            }
            //channnel��_channnels�̗v�f���ȏ�̏ꍇ�̓G���[���o��
            if (channnel >= _channnels.Count)
            {
                Debug.LogError("�`�����l���ԍ����s���ł� : " + channnel);
                return;
            }

            //key��_seList�ɑ��݂��Ȃ��ꍇ�̓G���[���o��
            if (!_seList.ContainsKey(key))
            {
                Debug.LogError("�w�肳�ꂽ�L�[��SE���X�g�ɑ��݂��܂��� : " + key);
                return;
            }

            //�w��̃`�����l����SE���Đ�����
            _channnels[channnel].PlayOneShot(_seList[key]);
        }
    }
}



