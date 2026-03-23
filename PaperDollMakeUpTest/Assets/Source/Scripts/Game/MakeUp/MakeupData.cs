using UnityEngine;

public class MakeupData : MonoBehaviour
{
    [SerializeField] private MakeupType _type;
    [SerializeField] private Sprite _image;

    public MakeupType Type => _type;
    public Sprite Image => _image;
}

public enum MakeupType
{
    Blush,
    EyeShadow,
    Lipstick,
    Powder,
}
