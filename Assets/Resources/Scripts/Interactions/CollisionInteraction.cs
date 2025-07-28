using UnityEngine;
using UnityEngine.Events;

public class CollisionInteraction : BaseInteraction
{

    [Header("Collision Interactions")]
    [SerializeField] protected UnityEvent<Collision2D> onCollisionEnter;
    [SerializeField] protected UnityEvent<Collision2D> onCollisionStay;
    [SerializeField] protected UnityEvent<Collision2D> onCollisionExit;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        onCollisionEnter?.Invoke(collision);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        onCollisionStay?.Invoke(collision);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        onCollisionExit?.Invoke(collision);
    }

}
