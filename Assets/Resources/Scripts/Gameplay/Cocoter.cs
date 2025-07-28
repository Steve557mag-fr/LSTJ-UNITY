using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.InputSystem;

public class Cocoter : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Transform rotator;
    [SerializeField] GameObject containerBackground;
    [SerializeField] GameObject containerParams;
    [SerializeField] GameObject containerShakeMode;
    [SerializeField] ProgressBar2D[] progresses;
    [SerializeField] ProgressBar2D shakeProgress;
    [SerializeField] BoxCollider2D boxDrag;
    [SerializeField] PhysicalDragDrop2D dragDrop;

    [Header("Maxs")]
    [SerializeField] float shakeMaxValue    = 10f;
    [SerializeField] float shakeRateValue   = 1f;
    [SerializeField] int maxRGBValue        = 3;
    [SerializeField] int maxWaterValue      = 3;
    [SerializeField] int maxSpeedValue      = 3;

    List<GameItemObject> gameItems = new();
    bool inShakeMode = false;
    bool shaked = false;

    float shakeValue = 0;


    public JObject GetValues()
    {
        return new(){
            {"red_value", progresses[0].value },
            {"green_value", progresses[1].value },
            {"blue_value", progresses[2].value },
            {"water_value", progresses[3].value },
            {"speed_value", progresses[4].value },
        };
    }

    public void UpdateProgresses()
    {
        containerParams.SetActive(true);
        containerShakeMode.SetActive(false);
        progresses[0].SetValue(GetGameItemsWithTag("RED").Count /   (float)maxRGBValue);
        progresses[1].SetValue(GetGameItemsWithTag("GREEN").Count / (float)maxRGBValue);
        progresses[2].SetValue(GetGameItemsWithTag("BLUE").Count /  (float)maxRGBValue);

        progresses[3].SetValue(GetGameItemsWithTag("WATER").Count / (float)maxWaterValue);
        progresses[4].SetValue(GetGameItemsWithTag("SPEED").Count / (float)maxSpeedValue);

    }

    public List<GameItemObject> GetGameItemsWithTag(string tag)
    {
        List<GameItemObject> res = new List<GameItemObject>();
        foreach (var item in gameItems)
        {
            if(item.type.Contains(tag)) res.Add(item);    
        }
        return res;
    }

    public void RemoveOneGameItemOfTag(string tag)
    {
        for(int i = 0; i < gameItems.Count; i++)
        {
            if (gameItems[i].type.Contains(tag))
            {
                gameItems.RemoveAt(i);
                return; // just once
            }
        }

    }

    public void ConcocterEnter(GameObject go)
    {
        string path = $"Prefabs/SObjects/GameItems/{go.name.Replace("(Clone)", "")}";
        GameItemObject item = Resources.Load<GameItemObject>(path);
        if (item == null) return;

        print($"item.type:{item.type}\nitem.name:{item.name}\nitem:{item}");

        if (item.type.Contains("_SUB")) RemoveOneGameItemOfTag(item.type.Replace("_SUB", ""));
        else gameItems.Add(item);

        UpdateProgresses();

    }

    public void CloseCocoter()
    {
        containerParams.SetActive(false);
        containerShakeMode.SetActive(true);
        shakeValue = shakeMaxValue;

        rotator.LeanRotate(Vector3.forward * 0, 1).setOnComplete(() =>
        {
            inShakeMode = true;
        });

    }

    private void Update()
    {
        if (inShakeMode)
        {
            Vector3 shakeVector = GameSingleton.GetInstance<InputManager>().GetShakeVector();
            var t = Mathf.Min(shakeVector.sqrMagnitude, 1.0f) * shakeRateValue * Time.deltaTime;
            //Debug.Log($"t: {t} \nv: {shakeVector.sqrMagnitude}");
            shakeValue -= t;
            shakeProgress.SetValue(shakeMaxValue - shakeValue);

            if(shakeValue <= 0)
            {
                inShakeMode = false;
                shaked = true;
                shakeValue = shakeMaxValue;
                ShakingCompleted();
            }

        }
    }

    void ShakingCompleted()
    {
        print("done!");
        containerBackground.SetActive(false);
        containerShakeMode.SetActive(false);
        containerParams.SetActive(false);

        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;

        boxDrag.enabled = true;
        dragDrop.enabled = true;
        
    }


}
