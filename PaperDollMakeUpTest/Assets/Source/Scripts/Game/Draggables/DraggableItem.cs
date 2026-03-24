using UnityEngine;
using UnityEngine.UI;

public class DraggableItem : MonoBehaviour
{
    [SerializeField] private MakeupType _type;
    [Header("Color")]
    [SerializeField] private Image _colorHead;
    [SerializeField] private Color _startColor;

    private MakeupData _data;
    private Transform _startParent;
    private Vector2 _startPos;

    public MakeupType Type => _type;
    public MakeupData Data { get => _data; set => _data = value; }
    public Vector2 StartPosition => _startPos;

    public void Initialize()
    {
        _startParent = transform.parent;
        _startPos = transform.position;
    }

    #region >>> POSITION

    public void BackToStartPosition()
    {
        transform.position = _startPos;
        transform.SetParent(_startParent);
    }

    #endregion
    #region >>> COLOR

    public void SetColor(Color color)
    {
        _colorHead.color = color;
    }

    private void ResetColor()
    {
        SetColor(_startColor);
    }

    #endregion
    #region >>> INTERACTABLE

    public void SetInteractable(bool value)
    {
        _colorHead.raycastTarget = value;
    }

    #endregion

    public void Reset()
    {
        BackToStartPosition();
        ResetColor();
    }
}
