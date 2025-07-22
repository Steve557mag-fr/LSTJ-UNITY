using UnityEngine;


public class GameSingleton : MonoBehaviour
{
    public void MakeInstance<T>() where T : Object
    {
        if (FindObjectsByType<T>(FindObjectsInactive.Exclude, FindObjectsSortMode.InstanceID).Length > 1) Destroy(gameObject);
        else DontDestroyOnLoad(gameObject);
    }

    public static T GetInstance<T>() where T : Object {
        return FindAnyObjectByType<T>(FindObjectsInactive.Exclude);
    }

}
