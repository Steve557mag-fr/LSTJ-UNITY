using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;


public class InputManager : GameSingleton
{
    [SerializeField] LayerMask layerInteraction;
    [SerializeField] List<IGameInput> outcallStack = new();

    public Vector2 GetTapPosition()
    {
        return Touch.fingers.Count > 0 ? Touch.fingers[0].screenPosition : Mouse.current.position.value;
    }

    public Vector2 GetTapWorldPosition()
    {
        return Camera.main.ScreenToWorldPoint(GetTapPosition());
    }

    void DoInteraction(bool isDown = true)
    {
        if (!isDown)
        {
            foreach (var f in outcallStack) { f.OnTapUp(); }
            outcallStack.Clear();
            return;
        }

        var pos = GetTapWorldPosition();
        var res = Physics2D.Raycast(pos, Vector3.zero, 25, layerInteraction);

        if (res.collider == null) return;
        if (res.collider.GetComponent<IGameInput>() == null) return;

        res.collider.GetComponent<IGameInput>().OnTapDown();
        ForceToOutCallStack(res.collider.GetComponent<IGameInput>());

    }

    internal void ForceToOutCallStack(IGameInput i){
        outcallStack.Add(i);
    }

    void Start() {}
    void Update()
    {
        //PC support
        if (Mouse.current.leftButton.wasPressedThisFrame) DoInteraction();
        else if (Mouse.current.leftButton.wasReleasedThisFrame) DoInteraction(false);

        //Mobile support
        else if (Touch.fingers.Count > 0)
        {
            if (Touch.fingers[0].currentTouch.began) DoInteraction();
            else if (Touch.fingers[0].currentTouch.ended) DoInteraction(false);
        }

    }

}
