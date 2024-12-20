using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HolmonUtility;
using MusicGameEngine;

[RequireComponent(typeof(Collider))]
public class ScreenInput : InputBase
{
    [SerializeField] private Camera _refCamera;
    Collider _colider;

    void Start()
    {
        _colider = GetComponent<Collider>();
    }

    Vector2 onPos;
    bool isOn = false;
    void Update()
    {
        // �}�E�X�N���b�N�����o
        if (Input.GetMouseButtonDown(0))  // ���N���b�N (0 �͍��N���b�N)
        {
            if(isOn) return;

            onPos = Input.mousePosition;
            var ret = InputOnLogic(onPos);
            if (ret)
            {
                isOn = true;
            }
        }
        if(Input.GetMouseButtonUp(0))
        {
            if (!isOn) return;

            var ret = InputOffLogic(onPos);
            if (ret)
            {
                isOn = false;
            }
        }

        // �X�N���[���^�b�v�����o (�^�b�`����)
        if (Input.touchCount > 0)
        {
            foreach(var touch in Input.touches)
            {
                if (touch.phase == TouchPhase.Began)  // �^�b�v�̊J�n�����o
                {
                    if (isOn) return;

                    onPos = touch.position;
                    var ret = InputOnLogic(onPos);
                    if(ret)
                    {
                        isOn = true;
                    }
                }

                if(touch.phase == TouchPhase.Ended)
                {
                    if (!isOn) return;

                    var ret = InputOffLogic(onPos);
                    if(!ret)
                    {
                        isOn = false;
                    }
                }
            }
        }
    }

    private bool InputOnLogic(Vector2 pos)
    {
        var ret = false;

        // �}�E�X�ʒu����X�N���[�����W���擾���ARay�𔭎�
        Ray ray = _refCamera.ScreenPointToRay(pos);
        RaycastHit hit;

        // Ray���I�u�W�F�N�g�ɓ��������ꍇ
        if (Physics.Raycast(ray, out hit))
        {
            // ����̏��������s
            if (hit.collider == _colider)
            {
                Debug.Log("On");
                ExecuteInputOnCallback();
                ret = true;
            }
        }

        return ret;
    }

    private bool InputOffLogic(Vector2 pos)
    {
        var ret = false;

        // �}�E�X�ʒu����X�N���[�����W���擾���ARay�𔭎�
        Ray ray = _refCamera.ScreenPointToRay(pos);
        RaycastHit hit;

        // Ray���I�u�W�F�N�g�ɓ��������ꍇ
        if (Physics.Raycast(ray, out hit))
        {
            // ����̏��������s
            if (hit.collider == _colider)
            {
                Debug.Log("Off");
                ExecuteInputOffCallback();
                ret = true;
            }

        }
        return ret;
    }
}
