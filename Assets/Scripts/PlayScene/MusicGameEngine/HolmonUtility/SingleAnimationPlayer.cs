using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System.Threading;

namespace HolmonUtility
{
    /// <summary>
    /// プールからオブジェクトを生成し、単発のアニメーションを再生するモジュール
    /// </summary>
    [RequireComponent(typeof(ObjectPool))]
    public class SingleAnimationPlayer : MonoBehaviour
    {
        private ObjectPool _pool;

        private void Awake()
        {
            _pool = GetComponent<ObjectPool>();
        }

        /// <summary>
        /// アニメーションを再生する
        /// オブジェクトに何らかの操作が加えられるよう、生成したオブジェクトを返す
        /// </summary>
        public async void PlayAnimation()
        {
            var gam = _pool.GetObject(true);
            var anim = gam.GetComponent<Animation>();
            if(anim == null)
            {
                anim = gam.GetComponentInChildren<Animation>();
            }

            ControllGameObject(gam);

            try
            {
                anim.Play();
            }
            catch
            {
                Debug.Log("アニメーションコンポーネントがアタッチされていません");
            }

            while (anim.isPlaying)
            {
                await Task.Delay(100); // 100ミリ秒待機してCPUの負荷を軽減
            }

            _pool.ReturnObject(gam);
        }

        protected virtual void ControllGameObject(GameObject gam)
        {

        }
    }
}


