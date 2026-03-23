using UnityEngine;

public class SelectPagesPanel : MonoBehaviour
{
    [SerializeField] private SelectPageButton[] _pagesButton;

    private Book _book;
    private SelectPageButton _currentSelectedButton;

    public void Initialize(Book book)
    {
        _book = book;

        InitializeSelectPageButtons();

        OpenPageByType(MakeupType.Blush);
    }

    private void InitializeSelectPageButtons()
    {
        foreach (SelectPageButton button in _pagesButton)
        {
            button.Initialize(this);
        }
    }

    public void OnSelectPageButtonClicked(SelectPageButton button)
    {
        if(_currentSelectedButton != null)
        {
            ResetSelectionFromPageButton(_currentSelectedButton);
        }

        _currentSelectedButton = button;

        MakeupType selectedPageType = _currentSelectedButton.PageType;
        OpenPageByType(selectedPageType);       
    }

    private void OpenPageByType(MakeupType type)
    {
        SetSelectionToPageButtonByType(type);
        _book.OpenPageByType(type);
    }

    private void SetSelectionToPageButtonByType(MakeupType type)
    {
        foreach (SelectPageButton button in _pagesButton)
        {
            if(button.PageType == type)
            {
                _currentSelectedButton = button;
                _currentSelectedButton.SetSelection(true);
                break;
            }
        }
    }

    private void ResetSelectionFromPageButton(SelectPageButton button)
    {
        button.SetSelection(false);
    }
}
