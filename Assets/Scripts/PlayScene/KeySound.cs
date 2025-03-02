using HolmonUtility;
using MusicGameEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeySound : MonoBehaviour
{
    public GameObject[] imputObjects;
    private InputBase[] inputs;

    private void Start()
    {
        inputs = new InputBase[imputObjects.Length];
        for (int i = 0; i < imputObjects.Length; i++)
        {
            try
            {
                inputs[i] = imputObjects[i].GetComponent<InputBase>();
            }
            catch
            {
                Debug.LogError(imputObjects[i].name + "にInputBaseがアタッチされていません");
            }

            inputs[i].AssignInputOnCallback(() =>
            {
                GlobalSEPlayer.PlaySE("KeySound");
            });
        }
    }
}