using UnityEngine;

public class SelectMakeupRoot : CompositeRoot
{
    [SerializeField] private Book _book;
    [Header("other")]
    [SerializeField] private CharacterRoot _characterRoot;
    [SerializeField] private HandRoot _handRoot;

    private MakeupData _currentSelectedData;

    public override void Compose()
    {
        _book.Initialize(this);
    }

    public void SetMakeUpData(SelectMakeUpButton button)
    {
        _currentSelectedData = button.Data;

        _handRoot.OnMakeUpSelected(button);
    }

    public DraggableItem GetDraggableItemByType(MakeupType type)
    {
        return _book.GetDraggableItemByType(type);
    }

    public void ResetMakeUpData()
    {
        _currentSelectedData = null;
    }
}
