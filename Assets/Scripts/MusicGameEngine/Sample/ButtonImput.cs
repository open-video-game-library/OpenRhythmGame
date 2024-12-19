namespace MusicGameEngine
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using HolmonUtility;

    public class ButtonImput : InputBase
    {
        [SerializeField] private KeyCode _inputKey;

        private void Update()
        {
            if (Input.GetKeyDown(_inputKey))
            {
                //Debug.Log("On");
                ExecuteInputOnCallback();
            }
            else if (Input.GetKeyUp(_inputKey))
            {
                ExecuteInputOffCallback();
            }
        }
    }
}


