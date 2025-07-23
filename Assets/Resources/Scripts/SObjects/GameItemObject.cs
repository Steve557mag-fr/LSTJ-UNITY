using UnityEngine;

[CreateAssetMenu(fileName = "GameItemObject", menuName = "Scriptable Objects/GameItemObject")]
public class GameItemObject : ScriptableObject
{
    [SerializeField] internal Sprite sprite;
    [SerializeField] internal GameObject prefab;

    [SerializeField] internal GameItemObject[] canProduce;

}
