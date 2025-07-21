using UnityEngine;

[RequireComponent (typeof(Rigidbody2D))]
public class DragDrop2D : MonoBehaviour
{
    [SerializeField] float forceTarget = 0.5f;

    Rigidbody2D rg;
    private void Awake()
    {
        rg = GetComponent<Rigidbody2D>();
    }

    private void OnMouseDrag()
    {

        rg.position = Input.mousePosition;
        print("bruh?");
    }

}
