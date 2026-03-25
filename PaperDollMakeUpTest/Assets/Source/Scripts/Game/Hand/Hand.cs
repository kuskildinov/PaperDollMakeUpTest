using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System;

[RequireComponent(typeof(RectTransform))]
public class Hand : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("Hand Y Positions")]   
    [SerializeField] private float _hiddenY = -500;
    [SerializeField] private float _chestY;
    [Header("Animations Settings")]
    [SerializeField] private float _handMoveSpeed = 0.5f;
    [SerializeField] private float _handShakeSpeed = 0.3f;
    [SerializeField] private float _widthOfShake = 20f;
    [SerializeField] private float _widthOfStroke = 5f;

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

    #region >>> TWEENS
    public void OnMakeUpSelected(SelectMakeUpButton button, DraggableItem item)
    {
        if (_currentState != HandState.Idle) return;

        _currentItem = item;
        _currentState = HandState.TakingTool;

        _startPos = transform.position;

        Sequence seq = DOTween.Sequence();
        seq.Append(_rectTransform.DOMove(_currentItem.transform.position, _handMoveSpeed));                                                                         // Летим к предмету
        seq.AppendCallback(() =>                                                                                                                                    // Хватаем предмет
        {
            _currentItem.TryTakeByHand(_rectTransform);  
        });
        if(_currentItem.NeedGetColor)                                                                                                                             // Летим к месту подготовки набора цвета, если нужно
            seq.Append(_rectTransform.DOMove(new Vector2(button.transform.position.x, button.transform.position.y - _currentItem.UseYOffset), _handMoveSpeed));      
        seq.AppendCallback(() =>                                                                                                                                 // Анимация набора цвета
        {
            seq.Pause();
            PlayPrepareAnimationByType(button, () =>
            {
                Color color = button.Data.Color;
                item.SetColor(color);
                seq.Play();
            });            
        });

        Vector2 preparePosition = GetPreparePosition(_currentItem);                                                         
        seq.Append(_rectTransform.DOMove(preparePosition, _handMoveSpeed));                                                                                        // Летим к месту передачи управления игроку
        seq.OnComplete(() =>
        {
            _currentState = HandState.HoldingTool;
        });
    }
    
    public void ApplyEffect(Face face, Action onComplete)
    {
        _currentState = HandState.Applying;

        Sequence seq = DOTween.Sequence();

        Vector2 setMakeupPosition = GetMakeupPosition(face, _currentItem);
        seq.Append(_rectTransform.DOMove(setMakeupPosition, _handMoveSpeed));
        seq.AppendCallback(() =>                                                                                                          // Анимация нанесения макияжа
        {
            seq.Pause();
           
            PlaySetMakeupAnimationByType(() =>
            {
                seq.Play();
            });
        });

        Vector2 itemStartPosition = _currentItem.StartPosition;
        seq.Append(_rectTransform.DOMove(itemStartPosition, _handMoveSpeed));                                                            // Летим у стартовому слоту 
        seq.OnComplete(() =>                                                                                                             // Сбрасываем и скрываем руку
        {
            ReturnTool();
            onComplete?.Invoke();
        });
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
    #endregion
    #region >>> ANIMATIONS

    private void PlayPrepareAnimationByType(SelectMakeUpButton button, Action OnComplete)
    {
        MakeupType type = button.Data.Type;

        if(type == MakeupType.Blush || type == MakeupType.EyeShadow)
        {            
            PlayShakeAnimation(button.transform.position, _widthOfShake, () =>
            {
                OnComplete?.Invoke();
            });
        }
        else
        {
            OnComplete?.Invoke();
        }
    }

    private void PlaySetMakeupAnimationByType(Action OnComplete)
    {
        if (_currentItem.Type == MakeupType.Cream)
        {
            PlayTopDownAnimation(_currentItem.transform.position, _widthOfStroke, () =>
            {
                OnComplete?.Invoke();
            });
        }
        else
        {
            PlayShakeAnimation(_currentItem.transform.position, _widthOfShake, () =>
            {
                OnComplete?.Invoke();
            });
        }
        
    }

    private void PlayShakeAnimation(Vector2 startPos, float brushWidth, Action onComplete)
    {
        Sequence brushSeq = DOTween.Sequence();
        brushSeq.Append(_rectTransform.DOMoveX(startPos.x - brushWidth, _handShakeSpeed).SetEase(Ease.InOutSine))                                            // Движения влево-вправо 3 раза
                .Append(_rectTransform.DOMoveX(startPos.x + brushWidth, _handShakeSpeed).SetEase(Ease.InOutSine))
                .Append(_rectTransform.DOMoveX(startPos.x - brushWidth, _handShakeSpeed).SetEase(Ease.InOutSine))
                .Append(_rectTransform.DOMoveX(startPos.x + brushWidth, _handShakeSpeed).SetEase(Ease.InOutSine))
                .Append(_rectTransform.DOMoveX(startPos.x, _handMoveSpeed).SetEase(Ease.OutBack));

        brushSeq.OnComplete(() =>
        {
            onComplete?.Invoke();
        });                                                                                                  // При необходимости задаем цвет кисти
        brushSeq.Play();
    }

    private void PlayTopDownAnimation(Vector2 startPos, float width, Action onComplete)
    {
        Sequence Seq = DOTween.Sequence();
        Seq.Append(_rectTransform.DOMoveY(startPos.y - width, _handShakeSpeed).SetEase(Ease.InOutSine))                                            // Движения вниз-вверх 3 раза
               .Append(_rectTransform.DOMoveY(startPos.y + width, _handShakeSpeed).SetEase(Ease.InOutSine))
               .Append(_rectTransform.DOMoveY(startPos.y, _handMoveSpeed).SetEase(Ease.OutBack));
        Seq.OnComplete(() =>
        {
            onComplete?.Invoke();
        });                                                                                                  // При необходимости задаем цвет кисти
        Seq.Play();
    }

    #endregion
    #region >>> DRAG EVENTS
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
    #endregion  

    private Vector2 GetPreparePosition(DraggableItem item)
    {
        Vector2 preparePosition = new Vector2();
        if (item.Type == MakeupType.Cream)
        {
            Vector2 facePos = _root.GetFacePosition();
            preparePosition = (facePos + (Vector2)item.GetComponent<RectTransform>().position) / 2f;
        }
        else
        {
            preparePosition = _root.GetChestPosition();
        }
        return preparePosition;
    }

    private Vector2 GetMakeupPosition(Face face, DraggableItem item)
    {
        Vector2 makeupPosition = new Vector2();
        if (item.Type == MakeupType.Blush)
            makeupPosition = face.Nose.position;
        else if (item.Type == MakeupType.EyeShadow)
            makeupPosition = face.Eyes.position;
        else if (item.Type == MakeupType.Lipstick)
            makeupPosition = face.Lips.position;
        else if (item.Type == MakeupType.Cream)
            makeupPosition = face.Nose.position;
        else
            makeupPosition = face.Nose.position;

        makeupPosition = new Vector2(makeupPosition.x, makeupPosition.y - _currentItem.UseYOffset);
        return makeupPosition;
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
