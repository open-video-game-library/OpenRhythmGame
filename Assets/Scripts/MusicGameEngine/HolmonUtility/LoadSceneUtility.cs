
using System.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace HolmonUtility
{
    /*
     * -----�v���C�V�[��-----
     * [0] ���ʃf�[�^
     * -----���U���g�V�[��-----
     * [0] ���ʃf�[�^(ScoreContainer)
     * [1] �X�R�A�f�[�^
    */

    /*
    //���[�h���ꂽ���ɌĂяo���\�ȃN���X�Ɏ�������C���^�[�t�F�[�X
    public interface IOnLoadedReaction
    {
        public void ReceiveOnLoaded(params object[] iSceneSharedObjects);
    }

    public class LoadSceneUtility : MonoBehaviour
    {
        [SerializeField] private List<GameObject> _onLoadedReactionObjects;
        private List<IOnLoadedReaction> _onLoadedReactions = new List<IOnLoadedReaction>();

        private void Awake()
        {
            foreach (var obj in _onLoadedReactionObjects)
            {
                var comp = obj.GetComponent<IOnLoadedReaction>();
                _onLoadedReactions.Add(comp);
            };
        }

        /// <summary>
        /// �����̃V�[���Ƀ��[�h���ꂽ���ɌĂяo�����
        /// </summary>
        /// <param name="sceneSharedObjects">�ʂ̃V�[������n���ꂽ�f�[�^���i�[����object�^�̃��X�g</param>
        public void OnLoaded(params object[] sceneSharedObjects)
        {
            foreach(var act in _onLoadedReactions) act.ReceiveOnLoaded(sceneSharedObjects);
        }

        /// <summary>
        /// �ʂ̃V�[�������[�h����
        /// </summary>
        /// <param name="sceneName">���[�h����V�[����</param>
        /// <param name="sceneSharedObjects">�ʂ̃V�[���ɓn�������f�[�^���i�[����object�^�̃��X�g</param>
        public void LoadScene(string sceneName, params object[] sceneSharedObjects)
        {
            SceneManager.sceneLoaded += OnSceneLoaded;

            void OnSceneLoaded(Scene scene, LoadSceneMode _mode)
            {
                SceneManager.sceneLoaded -= OnSceneLoaded;

                var target = GameObject.Find("LoadSceneUtility").GetComponent<LoadSceneUtility>();

                target.OnLoaded(sceneSharedObjects);
            }

            // �V�[���؂�ւ�
            //SceneManager.LoadScene(sceneName);
            Initiate.Fade(sceneName, Color.black, 2f);
        }
    }
*/
}
