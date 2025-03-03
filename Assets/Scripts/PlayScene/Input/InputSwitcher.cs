using System.Collections;
using System.Collections.Generic;
using MusicGameEngine;
using UnityEngine;
using UniRx;

public class InputSwitcher : InputBase
{
    [SerializeField] private List<InputBase> _inputList;
    [SerializeField] private eInputMode _inputMode = 0;
    
    private void Start()
    {
        for(int i = 0; i < _inputList.Count; i++)
        {
            int index = i;
            var input = _inputList[i];
            
            input.AssignInputOnCallback(() =>
            {
                if (index != (int)_inputMode) return;
                
                Debug.Log("On " + _inputMode);
                this.ExecuteInputOnCallback();
            });
            input.AssignInputOffCallback(() =>
            {
                if (index != (int)_inputMode) return;
                Debug.Log("Off " + _inputMode);
                this.ExecuteInputOffCallback();
            });
        }
    }

    private void Update()
    {
        if (PlaySceneMetaData.InputMode != _inputMode)
        {
            _inputMode = PlaySceneMetaData.InputMode;
        }
    }
}
