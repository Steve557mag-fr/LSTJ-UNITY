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
        for(int i = 0; i < minigames.Keys.Count; i++)
        {
            var k = minigames.Keys.ToArray()[0];
            if (minigames[k].minigameCode == code)
            {
                SetMGFromName(k);
                return;
            }
        }
    }

    internal void SetMGFromName(string newMG)
    {
        if (!minigames.ContainsKey(newMG)) return;

        //make transition
        isBusy = true;
        GameSingleton.GetInstance<UIManager>().MakeScreenTransition(
        duringTransition: async () =>
        {
            //unload old scene + load new scene
            await SceneManager.UnloadSceneAsync(newMG);
            currentMGName = newMG;
            await SceneManager.LoadSceneAsync(newMG);
        },
        finished: () => { 
            isBusy = true;
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
