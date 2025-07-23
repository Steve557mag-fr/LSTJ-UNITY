using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PhysicalDragDrop2D : DragDrop2D
{

    [SerializeField] protected float forceTarget = 0.5f;
    Rigidbody2D rgbody2D;

    void Awake()
    {
        rgbody2D = GetComponent<Rigidbody2D>();
    }

    protected override void UpdatePosition()
    {
        var pos = GameSingleton.GetInstance<InputManager>().GetTapWorldPosition();
        rgbody2D.linearVelocity = ((pos-rgbody2D.position) * forceTarget);
    }

}
