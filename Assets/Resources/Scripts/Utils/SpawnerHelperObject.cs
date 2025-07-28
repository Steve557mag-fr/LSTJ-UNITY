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

    public void TriggerDragDrop(bool spawnOnMouse = false)
    {
        GameObject g = Instantiate(prefab, spawnOnMouse ? GameSingleton.GetInstance<InputManager>().GetTapWorldPosition() : Vector2.zero, Quaternion.identity);
        g.name = nameBuilder.Replace("*", prefab.name);
        g.GetComponent<DragDrop2D>().ForceDragging();

    }

    public void Trigger(Vector3 pos)
    {
        GameObject g = Instantiate(prefab, pos, Quaternion.identity);
        g.name = nameBuilder.Replace("*", prefab.name);

    }

}
