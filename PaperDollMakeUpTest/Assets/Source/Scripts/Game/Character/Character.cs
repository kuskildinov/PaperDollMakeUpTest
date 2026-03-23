using UnityEngine;
using UnityEngine.UI;

public class Character : MonoBehaviour
{
    [SerializeField] private Image _eyeShadowImage;
    [SerializeField] private Image _blushImage;
    [SerializeField] private Image _lipsImage;

    public void Initialize()
    {

    }

    #region >>> SET MAKEUP

    public void SetMakeup(MakeupData data)
    {         
        switch(data.Type)
        {
            case MakeupType.Blush:
                {
                    _blushImage.sprite = data.Image;
                    break;
                }
            case MakeupType.EyeShadow:
                {
                    _eyeShadowImage.sprite = data.Image;
                    break;
                }
            case MakeupType.Lipstick:
                {
                    _lipsImage.sprite = data.Image;
                    break;
                }
            default:
                {
                    Debug.LogError("Error: Incorrect Makeup Type");
                    break;
                }
        }
    
    }

    #endregion
}
