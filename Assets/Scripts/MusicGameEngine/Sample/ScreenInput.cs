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
        // マウスクリックを検出
        if (Input.GetMouseButtonDown(0))  // 左クリック (0 は左クリック)
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

        // スクリーンタップを検出 (タッチ入力)
        if (Input.touchCount > 0)
        {
            foreach(var touch in Input.touches)
            {
                if (touch.phase == TouchPhase.Began)  // タップの開始を検出
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

        // マウス位置からスクリーン座標を取得し、Rayを発射
        Ray ray = _refCamera.ScreenPointToRay(pos);
        RaycastHit hit;

        // Rayがオブジェクトに当たった場合
        if (Physics.Raycast(ray, out hit))
        {
            // 特定の処理を実行
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

        // マウス位置からスクリーン座標を取得し、Rayを発射
        Ray ray = _refCamera.ScreenPointToRay(pos);
        RaycastHit hit;

        // Rayがオブジェクトに当たった場合
        if (Physics.Raycast(ray, out hit))
        {
            // 特定の処理を実行
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
