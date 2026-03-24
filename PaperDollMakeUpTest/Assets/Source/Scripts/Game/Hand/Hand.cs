using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;

[RequireComponent(typeof(RectTransform))]
public class Hand : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("Hand Y Positions")]
    [SerializeField] private float hiddenY = -500;
    [SerializeField] private float chestY = 150;

    private RectTransform _rectTransform;
    private Canvas _canvas;
    private Vector2 _startPos;
    private Transform _startParent;
    private HandState _currentState = HandState.Idle;
    private DraggableItem _currentItem;

    public void Initialize(Canvas canvas)
    {
        _rectTransform = GetComponent<RectTransform>();
        _canvas = canvas;
    }

    private void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
        _canvas = FindFirstObjectByType<Canvas>();
    }

    public void TakeTool(DraggableItem item, SelectMakeUpButton selectMakeButton)
    {
        if (_currentState != HandState.Idle) return;

        _currentItem = item;
        _currentState = HandState.TakingTool;

        _startPos = item.GetComponent<RectTransform>().anchoredPosition;
        _startParent = item.transform.parent;

        Sequence seq = DOTween.Sequence();

        // 1. рука появляется
        //seq.Append(_rectTransform.DOAnchorPosY(chestY, 1f).SetEase(Ease.OutBack));

        // 2. летит к предмету
        seq.Append(_rectTransform.DOMove(item.transform.position, 1f));

       
        // 3. захват
        seq.AppendCallback(() =>
        {
            item.transform.SetParent(_rectTransform);
            item.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        });

        // 2. летит к кнопке
        seq.Append(_rectTransform.DOMove(selectMakeButton.transform.position, 1f));

        seq.AppendCallback(() =>
        {
            // Запоминаем позицию кнопки
            Vector3 buttonPos = selectMakeButton.transform.position;
            float brushWidth = 50f; // ширина движения кисточки

            // Создаём вложенную последовательность для протирания
            Sequence brushSeq = DOTween.Sequence();

            // Движения влево-вправо 3 раза
            brushSeq.Append(_rectTransform.DOMoveX(buttonPos.x - brushWidth, 0.5f).SetEase(Ease.InOutSine))
                    .Append(_rectTransform.DOMoveX(buttonPos.x + brushWidth, 0.5f).SetEase(Ease.InOutSine))
                    .Append(_rectTransform.DOMoveX(buttonPos.x - brushWidth, 0.5f).SetEase(Ease.InOutSine))
                    .Append(_rectTransform.DOMoveX(buttonPos.x + brushWidth, 0.5f).SetEase(Ease.InOutSine))
                    .Append(_rectTransform.DOMoveX(buttonPos.x, 0.5f).SetEase(Ease.OutBack));

            brushSeq.Play();
        });
        // 4. возвращается в центр (грудь)
        //seq.Append(_rectTransform.DOAnchorPosY(chestY, 1f));

        seq.OnComplete(() =>
        {
            _currentState = HandState.HoldingTool;
        });
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (_currentState != HandState.HoldingTool) return;

        _currentState = HandState.Dragging;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (_currentState != HandState.Dragging) return;

        _rectTransform.anchoredPosition += eventData.delta / _canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (_currentState != HandState.Dragging) return;

        // Проверяем куда отпустили
        if (eventData.pointerEnter != null)
        {
            //FaceZone zone = eventData.pointerEnter.GetComponent<FaceZone>();

            //if (zone != null)
            //{
            //    zone.TryApply(this);
            //    return;
            //}
        }

        // не попали → ничего не делаем
        _currentState = HandState.HoldingTool;
    }
}

public enum HandState
{
    Idle,
    TakingTool,
    HoldingTool,
    Dragging,
    Applying
}
