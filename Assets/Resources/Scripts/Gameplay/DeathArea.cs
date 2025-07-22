using UnityEngine;
using UnityEngine.Events;

public class DeathArea : MonoBehaviour
{
    [SerializeField] UnityEvent<GameObject> onDeath;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        onDeath.Invoke(collision.gameObject);
        Destroy(collision.gameObject);
    }

}
