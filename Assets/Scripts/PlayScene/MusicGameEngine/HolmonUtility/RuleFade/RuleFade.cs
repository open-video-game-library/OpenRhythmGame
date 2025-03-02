using UnityEngine;
using UnityEngine.UI;
using HolmonUtility;
using System.Collections.Generic;


#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(Image))]
public class RuleFade : MonoBehaviour
{
    [SerializeField, ReadOnly]
    public float sliderValue = 0.5f;

    [SerializeField]
    protected List<Material> _syncronizablePalamatorMaterial = new List<Material>();

    protected Image _image;

    public bool fading { get; private set; } = false;

    protected virtual void Start()
    {
        fading = false;
        _image = GetComponent<Image>();
    }

    protected virtual void Update()
    {
        //値を範囲に収める
        sliderValue = Mathf.Clamp01(sliderValue);

        foreach(var mat in _syncronizablePalamatorMaterial)
        {
            mat.SetFloat("_Alpha", sliderValue);
            mat.SetColor("_Color", _image.color);
        }
    }
}