using System.Collections;
using System.Collections.Generic;
using MusicGameEngine;
using UnityEngine;
using UniRx;

public class InputSwitcher : InputBase
{
    [SerializeField] private List<InputBase> _inputList;
    [SerializeField] private int _select = 0;
    
    private void Start()
    {
        for(int i = 0; i < _inputList.Count; i++)
        {
            int index = i;
            var input = _inputList[i];
            
            input.AssignInputOnCallback(() =>
            {
                if (index != _select) return;
                
                Debug.Log("On" + _select);
                this.ExecuteInputOnCallback();
            });
            input.AssignInputOffCallback(() =>
            {
                if (index != _select) return;
                Debug.Log("Off" + _select);
                this.ExecuteInputOffCallback();
            });
        }
    }

    private void Update()
    {
        if (PlaySceneMetaData.ReceiveOSC && _select != 1)
        {
            _select = 1;
        }
        
        if(!PlaySceneMetaData.ReceiveOSC && _select != 0)
        {
            _select = 0;
        }
    }
}
