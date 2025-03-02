using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace HolmonUtility
{
    /*
     * -----プレイシーン-----
     * [0] 譜面データ
     * -----リザルトシーン-----
     * [0] 譜面データ(ScoreContainer)
     * [1] スコアデータ
    */

    //ロードされた時に呼び出し可能なクラスに実装するインターフェース
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
        /// 何かのシーンにロードされた時に呼び出される
        /// </summary>
        /// <param name="sceneSharedObjects">別のシーンから渡されたデータを格納するobject型のリスト</param>
        public void OnLoaded(params object[] sceneSharedObjects)
        {
            foreach(var act in _onLoadedReactions) act.ReceiveOnLoaded(sceneSharedObjects);
        }

        /// <summary>
        /// 別のシーンをロードする
        /// </summary>
        /// <param name="sceneName">ロードするシーン名</param>
        /// <param name="sceneSharedObjects">別のシーンに渡したいデータを格納するobject型のリスト</param>
        public void LoadScene(string sceneName, params object[] sceneSharedObjects)
        {
            SceneManager.sceneLoaded += OnSceneLoaded;

            void OnSceneLoaded(Scene scene, LoadSceneMode _mode)
            {
                SceneManager.sceneLoaded -= OnSceneLoaded;

                var target = GameObject.Find("LoadSceneUtility").GetComponent<LoadSceneUtility>();

                target.OnLoaded(sceneSharedObjects);
            }

            // シーン切り替え
            //SceneManager.LoadScene(sceneName);
            Initiate.Fade(sceneName, Color.black, 2f);
        }
    }
}

