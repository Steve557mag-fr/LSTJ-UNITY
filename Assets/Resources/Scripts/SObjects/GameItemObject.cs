using UnityEngine;

[CreateAssetMenu(fileName = "GameItemObject", menuName = "Scriptable Objects/GameItemObject")]
public class GameItemObject : ScriptableObject
{
    public Sprite sprite;
    public GameObject prefab;
    
    public bool canBeBurned = false;
    public bool canBePressed = false;
    public float burnValue = 1.0f;
    public GameItemObject[] canProduce;

    public string type;

}
