using UnityEngine;

[RequireComponent (typeof(SpriteRenderer))]
public class ProgressBar2D : MonoBehaviour
{

    public float value = 0;
    Material currentMaterial;

    void Start()
    {
        var c = GetComponent<SpriteRenderer>();
        currentMaterial = new Material(c.material);
        c.material = currentMaterial;
        SetValue();
    }

    public void SetValue(float val = 0)
    {
        if (currentMaterial == null) return;
        Debug.Log($"[{name}] val: "+ val);
        value = val;
        currentMaterial.SetFloat("_value", value);
    }



}
