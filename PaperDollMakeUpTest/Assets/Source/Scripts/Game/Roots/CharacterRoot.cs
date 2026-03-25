using UnityEngine;

public class CharacterRoot : CompositeRoot
{
    [SerializeField] private Character _character;
    [SerializeField] private Face _face;
    [SerializeField] private PlayerUI _playerUI;
    [Header("other")]
    [SerializeField] private SelectMakeupRoot _makeupRoot;

    public override void Compose()
    {
        _character.Initialize();
        _face.Initialize(this);
        _playerUI.Initialize();

        OnMakeStateChanged(false);
    }

    public void TrySetMakeupData()
    {
        MakeupData data = _makeupRoot.CurrentSelectedData;
        if(data == null)
        {
            Debug.LogError("Error: Makeup Data is null");
            return;
        }

        _character.SetMakeupData(data);
        OnMakeStateChanged(true);
        _makeupRoot.ResetMakeUpData();
    }

    public Vector2 GetChestPosition()
    {
        return _character.ChestPosition;
    }

    public Vector2 GetFacePosition()
    {
        return _face.RectTransform.position;
    }

    public void ResetCharacterMakeup()
    {
        _character.Reset();
        OnMakeStateChanged(false);
    }

    private void OnMakeStateChanged(bool value)
    {
        _playerUI.OnMakeStateChanged(value);
    }
}
