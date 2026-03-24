using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System;

[RequireComponent(typeof(RectTransform))]
public class Hand : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("Hand Y Positions")]
    [SerializeField] private float _selectColorY = 50f;
    [SerializeField] private float hiddenY = -500;
    [SerializeField] private float _chestY;

    private HandRoot _root;
    private RectTransform _rectTransform;
    private Canvas _canvas;
    private Vector2 _startPos;
    private Transform _startParent;
    private HandState _currentState = HandState.Idle;
    private DraggableItem _currentItem;

    public void Initialize(HandRoot root, Canvas canvas)
    {
        _rectTransform = GetComponent<RectTransform>();
        _root = root;
        _canvas = canvas;
        _startPos = _rectTransform.position;
    }
    
    public void OnMakeUpSelected(SelectMakeUpButton button, DraggableItem item)
    {
        if (_currentState != HandState.Idle) return;

        _currentItem = item;
        _currentState = HandState.TakingTool;

        _startPos = transform.position;

        Sequence seq = DOTween.Sequence();

        seq.Append(_rectTransform.DOMove(item.transform.position, 1f));                                                                   // Летим к предметы
        seq.AppendCallback(() =>                                                                                                          // Хватает предмет
        {
            item.transform.SetParent(_rectTransform);
            item.transform.SetSiblingIndex(1);
            item.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;          
        });
        seq.Append(_rectTransform.DOMove(new Vector2(button.transform.position.x, button.transform.position.y - _selectColorY), 1f));     // Летим к месту подготовки набора цвета
        seq.AppendCallback(() =>                                                                                                          // Анимация набора цвета
        {
            seq.Pause();
            Vector3 buttonPos = button.transform.position;
            float brushWidth = 10f; // ширина движения кисточки

            // Создаём вложенную последовательность для протирания
            Sequence brushSeq = DOTween.Sequence();

            // Движения влево-вправо 3 раза
            brushSeq.Append(_rectTransform.DOMoveX(buttonPos.x - brushWidth, 0.3f).SetEase(Ease.InOutSine))
                    .Append(_rectTransform.DOMoveX(buttonPos.x + brushWidth, 0.3f).SetEase(Ease.InOutSine))
                    .Append(_rectTransform.DOMoveX(buttonPos.x - brushWidth, 0.3f).SetEase(Ease.InOutSine))
                    .Append(_rectTransform.DOMoveX(buttonPos.x + brushWidth, 0.3f).SetEase(Ease.InOutSine))
                    .Append(_rectTransform.DOMoveX(buttonPos.x, 0.3f).SetEase(Ease.OutBack));

            brushSeq.OnComplete(() =>
            {
                Color color = button.Data.Color;
                item.SetColor(color);
                seq.Play();              
            });
            brushSeq.Play();
        });

        Vector2 chestPosition = new Vector2(_chestY, _chestY);
        seq.Append(_rectTransform.DOMove(chestPosition, 1f));
        seq.OnComplete(() =>
        {
            _currentState = HandState.HoldingTool;
        });
    }
    
    public void ApplyEffect(Face face, Action onComplete)
    {
        _currentState = HandState.Applying;

        Sequence seq = DOTween.Sequence();

        seq.Append(_rectTransform.DOMove(new Vector2(face.transform.position.x, face.transform.position.y - _selectColorY), 1f));
        seq.AppendCallback(() =>                                                                                                          // Анимация набора цвета
        {
            seq.Pause();

            float brushWidth = 10f;         
            Sequence brushSeq = DOTween.Sequence();

            // Движения влево-вправо 3 раза
            brushSeq.Append(_rectTransform.DOMoveX(_rectTransform.position.x - brushWidth, 0.3f).SetEase(Ease.InOutSine))
                    .Append(_rectTransform.DOMoveX(_rectTransform.position.x + brushWidth, 0.3f).SetEase(Ease.InOutSine))
                    .Append(_rectTransform.DOMoveX(_rectTransform.position.x - brushWidth, 0.3f).SetEase(Ease.InOutSine))
                    .Append(_rectTransform.DOMoveX(_rectTransform.position.x + brushWidth, 0.3f).SetEase(Ease.InOutSine))
                    .Append(_rectTransform.DOMoveX(_rectTransform.position.x, 0.3f).SetEase(Ease.OutBack));

            brushSeq.Play();
            brushSeq.OnComplete(() =>
            {
                seq.Play();
            });
           
        });
        Vector2 itemStartPosition = _currentItem.StartPosition;
        seq.Append(_rectTransform.DOMove(itemStartPosition, 1f));
        seq.OnComplete(() =>
        {
            ReturnTool();
            onComplete?.Invoke();
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

        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        // Ищем среди них Face, игнорируя руку
        foreach (var hit in results)
        {
            Face face = hit.gameObject.GetComponent<Face>();
            if (face != null && hit.gameObject != this.gameObject)
            {
                Debug.Log($"Face found: {hit.gameObject.name}");
                face.TryApply(this);               
                return;
            }
        }

        // не попали → ничего не делаем
        _currentState = HandState.HoldingTool;
    }

    private void ReturnTool()
    {
        Sequence seq = DOTween.Sequence();       
        seq.AppendCallback(() =>
        {
            _currentItem.Reset();
        });

        seq.Append(_rectTransform.DOMove(_startPos, 1f));

        seq.OnComplete(() =>
        {
            _currentState = HandState.Idle;
            _currentItem = null;
        });
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
