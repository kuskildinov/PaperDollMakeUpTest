using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image),(typeof(Button)))]
public class SelectPageButton : MonoBehaviour
{
    [SerializeField] private MakeupType _pageType;
    [Header("Visual")]
    [SerializeField] private Sprite _selectedSprite;
    [SerializeField] private Sprite _notSelectedSprite;

    private bool _isSelected;
    private Image _image;
    private Button _button;
    private SelectPagesPanel _selectPagePanel;

    public MakeupType PageType => _pageType;

    public void Initialize(SelectPagesPanel selectPagePanel)
    {
        _selectPagePanel = selectPagePanel;

        _image = GetComponent<Image>();
        _button = GetComponent<Button>();

        SubscribeToEvents();
    }

    public void SetSelection(bool value)
    {
        _isSelected = value;

        if (value)
        {
            _image.sprite = _selectedSprite;
            _image.transform.localScale = new Vector3(1.1f,1.1f,1.1f);
        }
           
        else
        {
            _image.sprite = _notSelectedSprite;
            _image.transform.localScale = Vector3.one;
        }
            
    }

    #region >>> EVENTS

    private void SubscribeToEvents()
    {
        if (_button != null)
            _button.onClick.AddListener(OnButtonClicked);
    }

    private void UnsubscribeToEvents()
    {
        if (_button != null)
            _button.onClick.RemoveAllListeners();
    }

    private void OnButtonClicked()
    {
        if (_isSelected)
            return;

        _selectPagePanel.OnSelectPageButtonClicked(this);
    }

    #endregion

    private void OnDestroy()
    {
        UnsubscribeToEvents();
    }
}
