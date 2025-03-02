using MusicGameEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;


public class KeyBeam : MonoBehaviour
{
    //hge
    [Serializable]
    public class BeamSet
    {
        public Animator beamAnimator;
        [HideInInspector] public InputBase input;
        public GameObject imputObject;


        public void Init()
        {
            try
            {
                input = imputObject.GetComponent<InputBase>();
            }
            catch
            {
                Debug.LogError(imputObject.name + "にInputBaseがアタッチされていません");
            }
        }
    }

    [SerializeField] private BeamSet[] beams;

    private async void Start()
    {
        await Task.Delay(10);
        
        foreach(var beam in beams)
        {
            beam.Init();

            beam.input.AssignInputOnCallback(() =>
            {
                beam.beamAnimator.SetTrigger("On");
            });
            beam.input.AssignInputOffCallback(() =>
            {
                beam.beamAnimator.SetTrigger("Off");
            });
        }
    }
}
