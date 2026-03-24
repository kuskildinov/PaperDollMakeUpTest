using UnityEngine;

public class Face : MonoBehaviour
{    
    public void TryApply(Hand hand)
    {
        Debug.Log("Apply to Face");

        hand.ApplyEffect(this, () =>
        {

        }       
        );
    }
}
