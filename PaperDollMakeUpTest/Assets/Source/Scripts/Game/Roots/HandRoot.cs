using UnityEngine;

public class HandRoot : CompositeRoot
{
    [SerializeField] private Hand _hand;
    [SerializeField] private Canvas _canvas;
    [Header("other")]
    [SerializeField] private CharacterRoot _characterRoot;
    [SerializeField] private SelectMakeupRoot _selectMakeupRoot;

    public override void Compose()
    {
        _hand.Initialize(this, _canvas);
    }

    public void OnMakeUpSelected(SelectMakeUpButton button)
    {
        DraggableItem draggableItem = _selectMakeupRoot.GetDraggableItemByType(button);

        _hand.OnMakeUpSelected(button, draggableItem);
    }

    public Vector2 GetChestPosition()
    {
        return _characterRoot.GetChestPosition();
    }
}
