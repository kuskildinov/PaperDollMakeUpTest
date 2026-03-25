using UnityEngine;

public class Face : MonoBehaviour
{
    public RectTransform Eyes;
    public RectTransform Nose;
    public RectTransform Lips;

    public void TryApply(Hand hand)
    {        
        hand.ApplyEffect(this, () =>
        {

        }       
        );
    }
}
