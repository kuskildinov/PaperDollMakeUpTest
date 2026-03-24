using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class SelectMakeUpButton : MonoBehaviour
{
    [SerializeField] private MakeupData _data;

    private Book _book;
    private Button _button;

    public MakeupData Data => _data;
  
    public void Initialize(Book book)
    {
        _book = book;
        _button = GetComponent<Button>();

        SubscribeToEvents();
    }
   
    #region >>> EVENTS

    private void SubscribeToEvents()
    {
        _button.onClick.AddListener(OnButtonClicked);
    }

    private void UnsubscribeToEvents()
    {
        _button.onClick.RemoveAllListeners();
    }

    private void OnButtonClicked()
    {
        _book.OnSelectMakeupButtonClicked(this);
    }
    #endregion

    private void OnDestroy()
    {
        UnsubscribeToEvents();
    }
}
