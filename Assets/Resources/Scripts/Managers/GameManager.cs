using Newtonsoft.Json.Linq;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : GameSingleton
{
    public NodePath[] path;
    int currentObjective;

    private void Start()
    {
        GameSingleton.GetInstance<SocketManager>().onLobbyUpdate += TryToLaunch;
    
    }

    internal void TryToLaunch(JObject data)
    {
        if(!GameSingleton.GetInstance<SocketManager>().LobbyIsReady()) return;
        currentObjective = 0;
        Goto("Vault");
    
    }

    internal void Goto(string sceneName = "Lobby")
    {
        GameSingleton.GetInstance<UIManager>().MakeScreenTransition(
        duringTransition: async () => {
            await SceneManager.LoadSceneAsync(sceneName);

        });
    }

}


[Serializable]
public struct NodePath
{

}
