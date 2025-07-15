using UnityEngine;
using UnityEngine.EventSystems;

public class DragDrop : EventTrigger
{
    public delegate void Dropped();
    public Dropped onDropped;


    public override void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public override void OnDrop(PointerEventData eventData)
    {
        onDropped();
    }

}
