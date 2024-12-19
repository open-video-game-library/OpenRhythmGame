using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MusicGameEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// PointCounter_ExtendNoteReaction�N���X�̃R�[���o�b�N�Ƃ��ēo�^�����N���X
/// ����W�v�̉������s��
/// </summary>
public class JudgeTotalizer : MonoBehaviour
{
    //�ϐ�
    //================================================================

    [Header("�V���A���C�Y")]
    [SerializeField] TMP_Text _perfectTex;
    [SerializeField] TMP_Text _goodTex;
    [SerializeField] TMP_Text _badTex;
    [SerializeField] TMP_Text _missTex;

    int _perfect;
    int _good ;
    int _bad;
    int _miss;

    //���\�b�h
    //================================================================

    private void Start()
    {
        //_perfect���̕ϐ��̏��������s��
        _perfect = 0;
        _good = 0;
        _bad = 0;
        _miss = 0;
    }

    public void ReceiveNewPointData(PointData pointData)
    {
        //����W�v���s��
        //����pointData�̒l���قȂ�ꍇ�́C_perfect���̕ϐ����X�V����
        if (_perfect != pointData.perfect)
        {
            _perfect = pointData.perfect;
            _perfectTex.text = _perfect.ToString();
        }
        if (_good != pointData.good)
        {
            _good = pointData.good;
            _goodTex.text = _good.ToString();
        }
        if (_bad != pointData.bad)
        {
            _bad = pointData.bad;
            _badTex.text = _bad.ToString();
        }
        if (_miss != pointData.miss)
        {
            _miss = pointData.miss;
            _missTex.text = _miss.ToString();
        }

    }
}
