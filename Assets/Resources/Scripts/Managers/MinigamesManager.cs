using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MinigamesManager : GameSingleton
{

    public MinigameInfo[] minigamesInfo;
    protected Dictionary<string, MinigameInfo> minigames = new();
    protected string currentMGName;
    bool isBusy = false;

    internal bool IsBusy {
        get { return isBusy; }
    }

    internal BaseMinigame GetCurrentMG()
    {
        return FindFirstObjectByType<BaseMinigame>();
    }

    internal void SetMGFromCode(string code)
    {
        Debug.Log(JObject.FromObject(minigames).ToString());
        foreach(var minigame in minigames)
        {
            Debug.Log($"minigame is :{minigame.Value.minigameCode.Length}=={code.Length}");
            if (minigame.Value.minigameCode.Equals(code))
            {
                SetMGFromName(minigame.Key);
                return;
            }
            else Debug.Log("ta race");
        }
    }

    internal void SetMGFromName(string newMG)
    {
        Debug.Log(newMG);
        if (!minigames.ContainsKey(newMG)) return;

        //make transition
        isBusy = true;
        GameSingleton.GetInstance<UIManager>().MakeScreenTransition(
        duringTransition: async () =>
        {
            //unload old scene + load new scene
            if(currentMGName != null) await SceneManager.UnloadSceneAsync(minigames[currentMGName].sceneMinigame);
            currentMGName = newMG;
            await SceneManager.LoadSceneAsync(minigames[currentMGName].sceneMinigame);
        },
        finished: () => { 
            isBusy = false;
        });

    }

    private void Awake()
    {
        for(int i = 0; i < minigamesInfo.Length; i++)
        {
            minigames.Add(minigamesInfo[i].minigameName, minigamesInfo[i]);
        }
    }
    
}



[Serializable]
public struct MinigameInfo
{
    public string minigameName;
    public string minigameCode;
    public string sceneMinigame;

}
