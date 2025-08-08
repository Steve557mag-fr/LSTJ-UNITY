using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ReadySystem : MonoBehaviour
{

    [SerializeField] UserMark[] marks;

    internal Action readyCompleted;

    int maxUser = 1;
    bool myMarkState = false;
    bool localState = false;

    private void Start()
    {
        GameSingleton.GetInstance<SocketManager>().onLobbyUpdate += UpdateSystem;
        ResetSystem();
    }

    public void ResetSystem(bool forceUI = true)
    {
        for (int i = 0; i < marks.Length; i++)
        {
            marks[i].markState = false;
            marks[i].markImage.gameObject.SetActive(false);
        }
    }

    public void UpdateSystem(JObject data)
    {
        bool allValided = true;
        List<JProperty> props = ((JObject)data["users"]).Properties().ToList();
        for(int i = 0; i < marks.Length; i++)
        {
            marks[i].markState = false;
            marks[i].markImage.gameObject.SetActive(marks[i].markState);

            if (i >= props.Count) continue;
            if (data["metadata"][$"{props[i].Name}_check"] == null) continue;

            bool haveMarked = data["metadata"][$"{props[i].Name}_check"].ToObject<bool>();
            marks[i].markState = haveMarked;
            marks[i].markImage.gameObject.SetActive(marks[i].markState);
            allValided = allValided && haveMarked;

        }

        if (props.Count < maxUser || !allValided) return;
        SystemCompleted();

    }

    public void ChangeReadyState()
    {
        localState = !localState;
        GameSingleton.GetInstance<SocketManager>().Ready(localState);
    }

    public void SystemCompleted()
    {
        print("validation completed!");
        readyCompleted?.Invoke();
        Destroy(gameObject);
    }

}


[System.Serializable]
public struct UserMark
{

    public Image markImage;
    public bool markState;

}
