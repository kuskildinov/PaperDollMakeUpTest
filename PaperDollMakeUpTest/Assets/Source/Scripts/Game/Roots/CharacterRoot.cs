using UnityEngine;

public class CharacterRoot : CompositeRoot
{
    [SerializeField] private Character _character;

    public override void Compose()
    {
        _character.Initialize();
    }

    public void SetMakeupData(MakeupData data)
    {
        _character.SetMakeupData(data);
    }

    public Vector2 GetChestPosition()
    {
        return _character.ChestPosition;
    }
}
