using UnityEngine;
using UnityEngine.UI;

public class Character : MonoBehaviour
{
    [Header("Images")]
    [SerializeField] private Image _eyeShadowImage;
    [SerializeField] private Image _blushImage;
    [SerializeField] private Image _lipsImage;
    [Header("Visual Data")]
    [SerializeField] private MakeupVisualData _eyeShadowVisualData;
    [SerializeField] private MakeupVisualData _blushVisualData;
    [SerializeField] private MakeupVisualData _lipsVisualData;
    [Header("other")]
    [SerializeField] private RectTransform _face;
    [SerializeField] private RectTransform _chest;

    public Vector2 FacePosition => _face.position;
    public Vector2 ChestPosition => _chest.position;

    public void Initialize()
    {

    }

    #region >>> SET MAKEUP

    public void SetMakeupData(MakeupData data)
    {
        MakeupVisualData selectedVisualData = GetVisualDataByType(data.Type);
        if (selectedVisualData == null)
        {
            Debug.LogError("Visual Data is null");
            return;
        }

        int makeupIndex = data.Index;
        Sprite makeSprite = selectedVisualData.Sprites[makeupIndex];

        SetMakeImage(data.Type, makeSprite);
    }

    private void SetMakeImage(MakeupType makeType, Sprite sprite)
    {
        switch (makeType)
        {
            case MakeupType.Blush:
                {
                    _blushImage.sprite = sprite;
                    _blushImage.gameObject.SetActive(true);
                    break;
                }
            case MakeupType.EyeShadow:
                {
                    _eyeShadowImage.sprite = sprite;
                    _eyeShadowImage.gameObject.SetActive(true);
                    break;
                }
            case MakeupType.Lipstick:
                {
                    _lipsImage.sprite = sprite;
                    _lipsImage.gameObject.SetActive(true);
                    break;
                }
            default:
                {
                    Debug.LogError("Error: Incorrect Makeup Type");
                    break;
                }
        }
    }

    private MakeupVisualData GetVisualDataByType(MakeupType type)
    {
        switch (type)
        {
            case MakeupType.Blush:
                {
                    return _blushVisualData;
                }
            case MakeupType.EyeShadow:
                {
                    return _eyeShadowVisualData;
                }
            case MakeupType.Lipstick:
                {
                    return _lipsVisualData;
                }
            default:
                {
                    Debug.LogError("Error: Incorrect Makeup Type");
                    return null;
                }
        }
    }

    #endregion
}
