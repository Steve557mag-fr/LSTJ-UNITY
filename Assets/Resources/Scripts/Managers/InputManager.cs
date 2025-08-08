using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;


public class InputManager : GameSingleton
{

    [Header("Generals")]
    [SerializeField] float shakeDetectionThreshold = 1;
    [SerializeField] LayerMask layerInteraction;
    [SerializeField] List<IGameInput> outcallStack = new();

    [Header("Debugging")]
    [SerializeField] bool usingDebugMode = false;
    [SerializeField] TMPro.TextMeshProUGUI textDebug;

    float shakeDetectionThresholdSqr;

    public Vector2 GetTapPosition()
    {
        return Touch.activeTouches.Count > 0 ? Touch.activeTouches[0].screenPosition : Mouse.current.position.value;
    }

    public Vector2 GetTapWorldPosition()
    {
        return Camera.main.ScreenToWorldPoint(GetTapPosition());
    }

    public Vector3 GetShakeVector()
    {
        Vector3 raw = Accelerometer.current == null ? Mouse.current.delta.value/10f : Accelerometer.current.acceleration.value;
        print(raw.sqrMagnitude);
        return raw.sqrMagnitude >= shakeDetectionThresholdSqr ? raw : Vector3.zero;
    }

    void DoInteraction(bool isDown = true)
    {
        if (!isDown)
        {
            print("interaction up");
            foreach (var f in outcallStack) { f.OnTapUp(); }
            outcallStack.Clear();
            return;
        }

        var pos = GetTapWorldPosition();
        var res = Physics2D.Raycast(pos, Vector3.zero, 25, layerInteraction);

        //print("interaction down");
        //print("c: "+res.collider);
        //print("p: "+res.point);

        if (res.collider == null) return;
        if (res.collider.GetComponent<IGameInput>() == null) return;

        res.collider.GetComponent<IGameInput>().OnTapDown();
        ForceToOutCallStack(res.collider.GetComponent<IGameInput>());

    }

    internal void ForceToOutCallStack(IGameInput i){
        outcallStack.Add(i);
    }
    internal void RemoveToOutCallStack(IGameInput i)
    {
        outcallStack.Remove(i);
    }



    void Start() {
        EnhancedTouchSupport.Enable();
        if(Accelerometer.current != null) InputSystem.EnableDevice(Accelerometer.current);
        shakeDetectionThresholdSqr = Mathf.Pow(shakeDetectionThreshold, 2);
    }
    void Update()
    {

        if (usingDebugMode)
        {
            textDebug.text = $"[GAME INPUT]\nfingersCount:{Touch.activeFingers.Count}\tCalloutStackCount:{outcallStack.Count}\nTouchesCount(old):{Input.touchCount}\ttouchCount:{Touch.activeTouches.Count}\naccel:{Accelerometer.current.acceleration.value}";
        }

        //PC support
        if (Mouse.current.leftButton.wasPressedThisFrame) DoInteraction();
        else if (Mouse.current.leftButton.wasReleasedThisFrame) DoInteraction(false);

        //Mobile support
        else if (Touch.activeTouches.Count > 0)
        {
            if (Touch.activeTouches[0].began) DoInteraction();
            else if (Touch.activeTouches[0].ended) DoInteraction(false);
        }

    }

}
