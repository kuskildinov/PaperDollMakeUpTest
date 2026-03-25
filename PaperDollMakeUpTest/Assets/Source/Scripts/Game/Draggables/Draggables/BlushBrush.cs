using UnityEngine;
using UnityEngine.UI;

public class BlushBrush : DraggableItem
{
    [Header("Brush Settings")]
    [Header("Color")]
    [SerializeField] private Image _colorHead;
    [SerializeField] private Color _startColor;

    #region >>> COLOR

    public override void SetColor(Color color)
    {
        _colorHead.color = color;
    }

    private void ResetColor()
    {
        SetColor(_startColor);
    }

    #endregion

    public override void Reset()
    {
        base.Reset(); 

        ResetColor();
    }
}
