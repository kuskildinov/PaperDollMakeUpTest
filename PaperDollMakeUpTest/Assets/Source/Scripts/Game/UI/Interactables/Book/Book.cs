using UnityEngine;

public class Book : MonoBehaviour
{
    [SerializeField] private SelectPagesPanel _selectPagesPanel;
    [SerializeField] private BookPage[] _pages;

    private SelectMakeupRoot _root;
    private BookPage _currentOpenPage;

    public void Initialize(SelectMakeupRoot root)
    {
        _root = root;
        _selectPagesPanel.Initialize(this);
        InitializeSelectMakeupButtons();

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

    public void OnSelectMakeupButtonClicked(MakeupData data)
    {
        _root.SetMakeUpData(data);
    }

    private void InitializeSelectMakeupButtons()
    {
        SelectMakeUpButton[] selectMakeupButtons = FindObjectsOfType<SelectMakeUpButton>(true);

        if (selectMakeupButtons == null)
        {
            Debug.LogError("Cant find Select Makeup Buttons");
            return;
        }

        foreach (SelectMakeUpButton button in selectMakeupButtons)
        {
            button.Initialize(this);
        }
    }

    #endregion
}
