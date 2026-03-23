using UnityEngine;

public class Book : MonoBehaviour
{
    [SerializeField] private SelectPagesPanel _selectPagesPanel;
    [SerializeField] private BookPage[] _pages;

    private BookPage _currentOpenPage;

    public void Initialize()
    {
        _selectPagesPanel.Initialize(this);

        CloseAllPages();
        OpenPage(_pages[0]);
    }

    private void Start()
    {
        _selectPagesPanel.Initialize(this);

        CloseAllPages();
        OpenPage(_pages[0]);
    }

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
}
