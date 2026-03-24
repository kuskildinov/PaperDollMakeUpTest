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
}
