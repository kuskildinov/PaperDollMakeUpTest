using UnityEngine;

public class DraggableItem : MonoBehaviour
{
    private MakeupData _data;

    public MakeupData Data { get => _data; set => _data = value; }
}
