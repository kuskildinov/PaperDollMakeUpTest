using UnityEngine;

public class BookPage : MonoBehaviour
{
    [SerializeField] private MakeupType _type;

    public MakeupType PageType => _type;
}
