using System;
using UnityEngine;

[Serializable]
public class MakeupData
{
    [SerializeField] private int _index;
    [SerializeField] private MakeupType _type;
    [SerializeField] private Color _color;

    public int Index => _index;
    public MakeupType Type => _type;
    public Color Color => _color;
}

public enum MakeupType
{
    Blush,
    EyeShadow,
    Lipstick,
    Powder,
    Cream,
}
