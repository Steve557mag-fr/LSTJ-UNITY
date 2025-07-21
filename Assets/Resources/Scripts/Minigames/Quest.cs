using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using UnityEditor;
using UnityEngine;

public class Quest : BaseMinigame
{
    SocketManager socketManager;

    private void Start()
    {
        // ONLY FOR QUEST SCENE TESTING 
        MGBegin();
        //-----
            
    }

    protected override void OnStart()
    {
        socketManager = GameSingleton.GetInstance<SocketManager>();
        socketManager.onGameData += onGameData;

    }

    void onGameData(JObject data)
    {
        if (data != null)
        {
            string wordList = JsonConvert.SerializeObject(data);
            Debug.Log(wordList);

            return;
        }
    }

    protected override void OnReset()
    {
        
    }

    protected override void OnEnd(bool isWinning = true)
    {
        
    }

}
