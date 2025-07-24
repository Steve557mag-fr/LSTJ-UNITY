using Newtonsoft.Json.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class TechArt : GameSpace
{
    [SerializeField] GameObject[] jobSpaces;


    int clientJobID = 0;


    protected override void OnBegin(JObject data)
    {
        clientJobID = GameSingleton.GetInstance<SocketManager>().GetClientIndexInLobby();
        for(int i = 0; i < jobSpaces.Length; i++)
        {
            jobSpaces[i].SetActive(i == clientJobID);
        }
    }

    public void SendItem(GameObject go)
    {
        print($"go: " + go.name);

        GameItemObject item = Resources.Load<GameItemObject>($"Prefabs/SObjects/GameItems/${go.name}");
        if (item) GameSingleton.GetInstance<SocketManager>().SendData(new()
        {
            {"method_name", "send_game_item_to_user_index"},
            {"data", new JObject(){ {"game_item_name", item.name} } }
        });
    }


}
