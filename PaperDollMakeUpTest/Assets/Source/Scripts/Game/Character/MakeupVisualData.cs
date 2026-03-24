using UnityEngine;

[CreateAssetMenu(fileName = "New Makeup Visual", menuName = "Makeup Visual")]
public class MakeupVisualData : ScriptableObject
{
    [SerializeField] private MakeupType _type;
    [SerializeField] private Sprite[] _sprites;

    public MakeupType Type => _type;
    public Sprite[] Sprites => _sprites;
}
