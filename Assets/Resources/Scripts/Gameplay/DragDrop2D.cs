using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent (typeof(Rigidbody2D))]
public class DragDrop2D : MonoBehaviour, IGameInput
{
    [SerializeField] float forceTarget = 0.5f;
    [SerializeField] bool forceDraggingAtAwake = false;

    public event Action onDragEnd;
    bool dragging;
    Rigidbody2D rgbody;
    Vector3 offset = new(0,0,-10);

    private void Awake()
    {
        rgbody = GetComponent<Rigidbody2D>();
        dragging = forceDraggingAtAwake;
    }

    private void Update()
    {
        if(dragging) rgbody.transform.position = Camera.main.ScreenToWorldPoint(Mouse.current.position.value)-offset;
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
        onDragEnd();
    }
}
