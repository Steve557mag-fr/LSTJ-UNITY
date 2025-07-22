using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "SpawnerHelperObject", menuName = "Scriptable Objects/SpawnerHelperObject")]
public class SpawnerHelperObject : ScriptableObject
{
    [SerializeField] GameObject prefab;

    public void Trigger()
    {
        Instantiate(prefab);
    }

    public void Trigger(bool spawnOnMouse=false)
    {
        if (spawnOnMouse) Trigger(Mouse.current.position.value);
        else Trigger();
    }

    public void Trigger(Vector3 pos)
    {
        Instantiate(prefab, pos, Quaternion.identity);
    }

}
