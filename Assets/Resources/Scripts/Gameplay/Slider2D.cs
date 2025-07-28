using UnityEngine;

[RequireComponent (typeof(BoxCollider2D))]
public class Slider2D : MonoBehaviour, IGameInput
{

    [Header("Generals")]
    [SerializeField] [Range(0.0f, 1.0f)] float currentValue = 0f;
    [SerializeField] Vector2 beginPos, endPos;
    public bool canBeDrag = true;
    bool isBusy;


    public float value
    {
        get { return currentValue; }
    }


    public void OnTapDown()
    {
        if (!canBeDrag) return;
        isBusy = true;
    }

    public void OnTapUp()
    {
        isBusy = false;
    }


    private void Update()
    {

        if (!isBusy) return;

        Vector2 worldTapPos = GameSingleton.GetInstance<InputManager>().GetTapWorldPosition();

        // source: https://discussions.unity.com/t/projection-of-a-point-on-a-line/43801
        currentValue = MathUtils.InverseLerp(beginPos, endPos, worldTapPos);
        UpdateSliderPos();

    }

    void UpdateSliderPos()
    {
        transform.position = Vector3.Lerp(beginPos, endPos, currentValue);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(beginPos, 0.25f);
        
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(endPos, 0.25f);

        Gizmos.color= Color.blue;
        Gizmos.DrawLine(beginPos, endPos);

    }

}
