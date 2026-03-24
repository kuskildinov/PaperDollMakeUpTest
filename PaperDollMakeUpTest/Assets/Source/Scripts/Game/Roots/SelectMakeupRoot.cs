using UnityEngine;

public class SelectMakeupRoot : CompositeRoot
{
    [SerializeField] private Book _book;
    [Header("other")]
    [SerializeField] private CharacterRoot _characterRoot;

    private MakeupData _currentSelectedData;

    public override void Compose()
    {
        _book.Initialize(this);
    }

    public void SetMakeUpData(MakeupData newData)
    {
        _currentSelectedData = newData;
        _characterRoot.SetMakeupData(newData);
        Debug.Log($"{newData.Type} type selected with index: {newData.Index}");
    }
}
