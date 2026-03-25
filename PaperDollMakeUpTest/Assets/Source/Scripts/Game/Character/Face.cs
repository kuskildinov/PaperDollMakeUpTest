using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class Face : MonoBehaviour
{
    private CharacterRoot _root;
    private RectTransform _rectTransform;

    public RectTransform Eyes;
    public RectTransform Nose;
    public RectTransform Lips;
    public RectTransform RectTransform => _rectTransform;

    public void Initialize(CharacterRoot root)
    {
        _root = root;
        _rectTransform = GetComponent<RectTransform>();
    }

    public void TryApply(Hand hand)
    {        
        hand.ApplyEffect(this, () =>
        {
            _root.TrySetMakeupData();
        });
    }
}
