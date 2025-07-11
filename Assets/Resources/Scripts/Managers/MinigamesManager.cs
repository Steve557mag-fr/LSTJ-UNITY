using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MinigamesManager : GameSingleton
{

    public MinigameInfo[] minigamesInfo;
    protected Dictionary<string, MinigameInfo> minigames = new();

    protected string currentMGName;

    internal BaseMinigame GetCurrentMG()
    {
        return FindFirstObjectByType<BaseMinigame>();
    }

    internal void SetMG(string newMG)
    {
        if (!minigames.ContainsKey(newMG)) return;

        //make transition
        GameSingleton.GetInstance<UIManager>().MakeScreenTransition(async () =>
        {
            //unload old scene + load new scene
            await SceneManager.UnloadSceneAsync(newMG);
            currentMGName = newMG;
            await SceneManager.LoadSceneAsync(newMG);
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
    public string sceneMinigame;

}
