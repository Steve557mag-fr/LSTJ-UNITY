using UnityEngine;

public class ObjectInteraction : BaseInteraction, IGameInput
{
    public bool once = false;
    bool locked = false;

    public void OnTapDown()
    {
        print("zapdlzdlp");
        if(locked) return;
        if (once) locked = true;
        Interact();
    }

    public void OnTapUp() {}

    
}
