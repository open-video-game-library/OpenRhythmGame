using MusicGameEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�X�N���[��������I�u�W�F�N�g�ɃA�^�b�`������N���X
public abstract class ScrollNoteObjectBase : MonoBehaviour
{
    private Action _destroyCallback = () => Debug.Log("�I�u�W�F�N�g���폜����܂���");

    protected float[] _additionalInfo;

    /// <summary>
    /// �I�u�W�F�N�g���폜���ꂽ���ɌĂ΂��R�[���o�b�N��ݒ肷��
    /// </summary>
    /// <param name="destroyCallback"></param>
    public void SetDestroyCallback(Action destroyCallback)
    {
        _destroyCallback = destroyCallback;
    }

    /// <summary>
    /// note�ɃZ�b�g���ꂽAdditional�����擾����
    /// </summary>
    /// <param name="additionalInfo"></param>
    public void SetAdditionalInfo(float[] additionalInfo)
    {
        _additionalInfo = additionalInfo;
    }

    /// <summary>
    /// �m�[�g�̃A�j���[�V�������Đ�����
    /// </summary>
    public abstract void Play(double animationTime, double animationOffset, params double[] correctInputTimes);

    /// <summary>
    /// �n�C�X�s�[�h��ݒ肷��
    /// </summary>
    /// <param name="highSpeed"></param>
    public abstract void SetHS(float highSpeed);

    /// <summary>
    /// BPM�ω��䗦��ݒ肷��
    /// </summary>
    /// <param name="ratio"></param>
    public abstract void SetBpmRatio(float ratio);

    /// <summary>
    /// ����̃R�[���o�b�N���󂯎��
    /// </summary>
    /// <param name="judge"></param>
    public abstract void ReceiveJudgeCallback(int judge);

    private void OnDestroy()
    {
        _destroyCallback();
    }

    protected abstract void Init();

    private void OnDisable()
    {
        Init();
        _destroyCallback = () => { };
    }
}
