using UnityEngine;

public class PressMachine : MonoBehaviour
{

    [Header("Generals")]
    [SerializeField] Slider2D pressController;
    [SerializeField] Transform pressTransform;
    [SerializeField] Vector3 beginPressPos, endPressPos;

    public void PressEnter(Collider2D col)
    {
        if (pressController.value <= 0.1f) return;

        var res = Resources.Load<GameItemObject>($"Prefabs/SObjects/GameItems/{col.name.Replace("(Clone)", "")}");
        if (res == null || !res.canBePressed || res.canProduce.Length <= 0) return;

        Debug.Log("*MINECRAFT ANVIL NOISE*");
        GameObject g = Instantiate(res.canProduce[0].prefab);
        g.transform.position = endPressPos;
        Destroy(col.gameObject);
    }

    private void Update()
    {
        pressTransform.transform.position = Vector3.Lerp(beginPressPos, endPressPos, pressController.value);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(beginPressPos, endPressPos);
    }

}
