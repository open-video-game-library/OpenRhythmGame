using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FramerateController : MonoBehaviour
{
    public enum VSyncMode
    {
        DontSync = 0, //VSyncをしない
        EveryVBlank = 1, //VSyncを毎回行う
        EverySecondVBlank = 2 //VSyncを2回に1回行う
    }

    [SerializeField] private VSyncMode _vSyncMode = VSyncMode.DontSync;
    [SerializeField] private int _targetFrameRate = 60;

    [SerializeField] private Text _frameRateTex;
    [SerializeField] private bool _visibleFrameRate = false;

    private void Start()
    {
        SetVSync(_vSyncMode);
        SetTargetFrameRate(_targetFrameRate);

        SetVisibleFrameRate(_visibleFrameRate);
    }

    private void Update()
    {
        if(_visibleFrameRate)
        {
            _frameRateTex.text = $"{1.0f / Time.deltaTime} FPS";
        }
    }

    public void SetVSync(VSyncMode mode)
    {
        switch (_vSyncMode)
        {
            case VSyncMode.DontSync:
                QualitySettings.vSyncCount = 0;
                break;
            case VSyncMode.EveryVBlank:
                QualitySettings.vSyncCount = 1;
                break;
            case VSyncMode.EverySecondVBlank:
                QualitySettings.vSyncCount = 2;
                break;
        }

        _vSyncMode = mode;
    }

    public void SetTargetFrameRate(int frameRate)
    {
        if(_vSyncMode != VSyncMode.DontSync)
        {
            Debug.LogWarning("VSyncが有効なため、フレームレートの設定は無視されます");
        }

        Application.targetFrameRate = frameRate;

        _targetFrameRate = frameRate;
    }

    public void SetVisibleFrameRate(bool enable)
    {
        _frameRateTex.gameObject.SetActive(enable);
        _visibleFrameRate = enable;
    }
}
