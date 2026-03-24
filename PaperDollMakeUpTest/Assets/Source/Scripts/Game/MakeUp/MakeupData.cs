using System;
using UnityEngine;

[Serializable]
public class MakeupData
{
    [SerializeField] private int _index;
    [SerializeField] private MakeupType _type;

    public int Index => _index;
    public MakeupType Type => _type;
}

public enum MakeupType
{
    Blush,
    EyeShadow,
    Lipstick,
    Powder,
}
