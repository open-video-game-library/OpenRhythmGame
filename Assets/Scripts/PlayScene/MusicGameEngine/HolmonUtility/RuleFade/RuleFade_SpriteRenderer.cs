using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class RuleFade_SpriteRenderer : RuleFade
{
    private SpriteRenderer _spriteRenderer;

    protected override void Start()
    {
        base.Start();

        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected override void Update()
    {
        base.Update();

        foreach (var mat in _syncronizablePalamatorMaterial)
        {
        }
    }
}
