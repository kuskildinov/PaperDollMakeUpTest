using UnityEngine;
using UnityEngine.UI;

public class ApplyPanel : MonoBehaviour
{
    [SerializeField] private Button _applyButton;
    [Header("Images")]
    [SerializeField] private Sprite _activeImage;
    [SerializeField] private Sprite _nonActiveImage;

    private PlayerUI _playerUI;

    public void Initialize(PlayerUI playerUi)
    {
        _playerUI = playerUi;
    }

    public void SetInteractable(bool value)
    {
        if (value)
        {
            _applyButton.interactable = true;
            _applyButton.image.sprite = _activeImage;
        }
        else
        {
            _applyButton.interactable = false;
            _applyButton.image.sprite = _nonActiveImage;
        }
    }
}
