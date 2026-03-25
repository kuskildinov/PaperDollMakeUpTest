using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private ApplyPanel _applyPanel;

    public void Initialize()
    {
        _applyPanel.Initialize(this);
    }

    public void OnMakeStateChanged(bool value)
    {
        _applyPanel.SetInteractable(value);
    }
}
