using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class DragDrop2D : MonoBehaviour, IGameInput
{
    [SerializeField] protected bool forceDraggingAtAwake = false;

    public event Action onDragEnd;
    protected Vector3 offset = new(0,0,-10);
    bool dragging;

    private void Awake()
    {
        dragging = forceDraggingAtAwake;
    }

    private void Update()
    {
        if(dragging) UpdatePosition();
    }

    protected virtual void OnDragEnd() { }
    protected virtual void UpdatePosition()
    {
        transform.position = Camera.main.ScreenToWorldPoint(Mouse.current.position.value) - offset;
    }

    public void ForceDragging()
    {
        dragging=true;
    }

    public void OnTapDown()
    {
        dragging = true;
    }

    public void OnTapUp()
    {
        dragging = false;
        OnDragEnd();
        onDragEnd?.Invoke();
    }


}
