using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MusicGameEngine;

public interface IScratchInput
{
    public bool inputable { get; }
    public int scrDirection { get; }

    public float UpSensitivty(bool up);

    public void SetInputable(bool inputable);
}
