using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System.Threading;

namespace HolmonUtility
{
    /// <summary>
    /// �v�[������I�u�W�F�N�g�𐶐����A�P���̃A�j���[�V�������Đ����郂�W���[��
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
        /// �A�j���[�V�������Đ�����
        /// �I�u�W�F�N�g�ɉ��炩�̑��삪��������悤�A���������I�u�W�F�N�g��Ԃ�
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
                Debug.Log("�A�j���[�V�����R���|�[�l���g���A�^�b�`����Ă��܂���");
            }

            while (anim.isPlaying)
            {
                await Task.Delay(100); // 100�~���b�ҋ@����CPU�̕��ׂ��y��
            }

            _pool.ReturnObject(gam);
        }

        protected virtual void ControllGameObject(GameObject gam)
        {

        }
    }
}


