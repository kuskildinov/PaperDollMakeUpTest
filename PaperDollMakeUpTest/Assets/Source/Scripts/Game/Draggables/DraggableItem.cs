using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class DraggableItem : MonoBehaviour
{
    [SerializeField] private int _index;
    [SerializeField] private MakeupType _type;
    [SerializeField] private float _useYOffset;
    [SerializeField] private bool _needGetColor = true;

    private RectTransform _rectTransform;
    private Transform _startParent;
    private Vector2 _startPos;

    public MakeupType Type => _type;
    public int Index => _index;
    public Vector2 StartPosition => _startPos;
    public float UseYOffset => _useYOffset;
    public bool NeedGetColor => _needGetColor;

    public void Initialize()
    {
        _startParent = transform.parent;
        _startPos = transform.position;
        _rectTransform = GetComponent<RectTransform>();
    }

    #region >>> POSITION

    public void BackToStartPosition()
    {
        transform.position = _startPos;
        transform.SetParent(_startParent);
    }

    #endregion
    #region >>> TAKE DROP 
    
    public void TryTakeByHand(RectTransform hand)
    {
        _rectTransform.SetParent(hand);
        _rectTransform.SetSiblingIndex(1);
        _rectTransform.anchoredPosition = Vector2.zero;
    }

    #endregion
    #region >>> COLOR

    public virtual void SetColor(Color color)
    {

    }

    #endregion

    public virtual void Reset()
    {
        BackToStartPosition();       
    }
}
