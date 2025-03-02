using UnityEngine;
using UnityEngine.UI;
using HolmonUtility;
using System.Collections.Generic;


#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(Image))]
public class RingVisualizer : MonoBehaviour
{
    [SerializeField, Range(0.0f, 1.0f)]
    public float innerRadius = 0.5f;

    [SerializeField, Range(0.0f, 1.0f)]
    public float outerRadius = 0.5f;

    protected Image _image;
    protected Material mat;

    protected virtual void Start()
    {
        _image = GetComponent<Image>();
        mat = _image.material;
    }

    protected virtual void Update()
    {
        //値を範囲に収める
        innerRadius = Mathf.Clamp01(innerRadius);
        outerRadius = Mathf.Clamp01(outerRadius);

        mat.SetFloat("_InnerRadius", innerRadius);
        mat.SetFloat("_OuterRadius", outerRadius);
        mat.SetColor("_Color", _image.color);
    }
}