using Newtonsoft.Json.Linq;
using UnityEngine;

public class TechArt : GameSpace
{
    
    int clientJobID = 0;

    protected override void OnBegin(JObject data)
    {
        clientJobID = GameSingleton.GetInstance<SocketManager>().GetClientIndexInLobby();

    }

    public void SendItem()
    {

    }


}
