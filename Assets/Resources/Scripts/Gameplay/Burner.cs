using UnityEngine;

public class Burner : MonoBehaviour
{

    [Header("Generals")]
    [SerializeField][Range(0.1f,2.0f)] float burnerRate = 1f;
    [SerializeField] float burnerMaxValue = 10f;
    [SerializeField] Transform spawnTransform;

    float currentBurnerValue = 0f;

    GameObject currentBurnerGO;
    GameItemObject currentBurnerObject;



    private void Update()
    {
        //wait..
        if(currentBurnerObject == null || currentBurnerObject.canProduce.Length <= 0 || !currentBurnerObject.canBeBurned)
        {
            currentBurnerValue = burnerMaxValue;
            return;
        }

        //burning..
        currentBurnerValue -= burnerRate * Time.deltaTime;
        if (currentBurnerValue > 0f) {
            print("burning....");
            return;
        }

        //done!
        if(currentBurnerObject.canProduce.Length > 0)
        {
            GameObject go = Instantiate(currentBurnerObject.canProduce[0].prefab);
            go.transform.position = spawnTransform.transform.position;
        }
        Destroy(currentBurnerGO);
        currentBurnerObject = null;

    }

    public void BurnerEnter(Collision2D col)
    {
        print("ah?");
        string path = $"Prefabs/SObjects/GameItems/{col.collider.name.Replace("(Clone)","")}";
        var gameItem = Resources.Load<GameItemObject>(path);
        if (gameItem == null) return;

        currentBurnerObject = gameItem;
        currentBurnerValue = gameItem.burnValue;
        currentBurnerGO = col.gameObject;

    }

    public void BurnerExit(Collision2D col)
    {
        currentBurnerObject = null;
        currentBurnerGO = null;
    }

}
