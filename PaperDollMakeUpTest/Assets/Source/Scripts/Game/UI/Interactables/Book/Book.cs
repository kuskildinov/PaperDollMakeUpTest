using UnityEngine;

public class Book : MonoBehaviour
{
    [SerializeField] private SelectPagesPanel _selectPagesPanel;
    [SerializeField] private BookPage[] _pages;

    private SelectMakeupRoot _root;
    private BookPage _currentOpenPage;
    private SelectMakeUpButton[] _selectMakeupButtons;
    private DraggableItem[] _draggableItems;

    public void Initialize(SelectMakeupRoot root)
    {
        _root = root;
        _selectPagesPanel.Initialize(this);
        InitializeSelectMakeupButtons();
        InitializeDraggableItems();

        CloseAllPages();
        OpenPage(_pages[0]);
    }

    #region >>> PAGES

    public void OpenPageByType(MakeupType type)
    {
        if (_currentOpenPage != null)
            ClosePage(_currentOpenPage);
        foreach (BookPage page in _pages)
        {
            if(page.PageType == type)
            {
                _currentOpenPage = page;
                OpenPage(_currentOpenPage);
                return;
            }
        }
    }

    private void OpenPage(BookPage page)
    {
        page.gameObject.SetActive(true);
    }

    private void ClosePage(BookPage page)
    {
        page.gameObject.SetActive(false);
    }

    private void CloseAllPages()
    {
        foreach (BookPage page in _pages)
        {
            ClosePage(page);
        }
    }

    #endregion
    #region >>> SELECT MAKEUP

    public void OnSelectMakeupButtonClicked(SelectMakeUpButton button)
    {
        _root.SetMakeUpData(button);
    }

    private void InitializeSelectMakeupButtons()
    {
        _selectMakeupButtons = FindObjectsOfType<SelectMakeUpButton>(true);

        if (_selectMakeupButtons == null)
        {
            Debug.LogError("Cant find Select Makeup Buttons");
            return;
        }

        foreach (SelectMakeUpButton button in _selectMakeupButtons)
        {
            button.Initialize(this);
        }
    }

    #endregion
    #region >>> DRAGGABLE ITEMS

    public DraggableItem GetDraggableItemByType(MakeupType type)
    {
        foreach (DraggableItem item in _draggableItems)
        {
            if (item.Type == type)
                return item;
        }

        return null;
    }

    private void InitializeDraggableItems()
    {
        _draggableItems = FindObjectsOfType<DraggableItem>(true);

        if (_selectMakeupButtons == null)
        {
            Debug.LogError("Cant find Draggable Items");
            return;
        }

        foreach (DraggableItem item in _draggableItems)
        {
            item.Initialize();
        }
    }

    #endregion
}
