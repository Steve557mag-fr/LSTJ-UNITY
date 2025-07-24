using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class DragDrop2D : MonoBehaviour, IGameInput
{
    [SerializeField] protected bool forceDraggingAtAwake = false;

    public event Action onDragEnd;
    protected Vector3 offset = new(0,0,-10);
    bool dragging;

    private void Start()
    {
        if (forceDraggingAtAwake) ForceDragging();
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

    internal void ForceDragging()
    {
        dragging=true;
        GameSingleton.GetInstance<InputManager>().ForceToOutCallStack(this);
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
