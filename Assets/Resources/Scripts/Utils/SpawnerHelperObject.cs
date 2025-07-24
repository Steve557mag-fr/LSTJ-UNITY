using UnityEngine;

[CreateAssetMenu(fileName = "SpawnerHelperObject", menuName = "Scriptable Objects/SpawnerHelperObject")]
public class SpawnerHelperObject : ScriptableObject
{
    [SerializeField] GameObject prefab;
    [SerializeField] string nameBuilder = "*";

    public void Trigger()
    {
        GameObject g = Instantiate(prefab);
        g.name = nameBuilder.Replace("*", prefab.name);

    }

    public void Trigger(bool spawnOnMouse=false)
    {
        if (spawnOnMouse) Trigger(GameSingleton.GetInstance<InputManager>().GetTapWorldPosition());
        else Trigger();
    }

    public void Trigger(Vector3 pos)
    {
        Instantiate(prefab, pos, Quaternion.identity);
    }

}
