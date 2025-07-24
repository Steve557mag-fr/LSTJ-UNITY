using Newtonsoft.Json.Linq;
using UnityEngine;

public class WebSocketConnector : MonoBehaviour
{
    void Start()
    {
        if(FindAnyObjectByType<SocketManager>() == null)
        {
            gameObject.AddComponent<SocketManager>();
        }

        GameSingleton.GetInstance<SocketManager>().onAuthentificated += (JObject jo) =>
        {
            GameSingleton.GetInstance<SocketManager>().JoinOrCreateLobby();
        };
        GameSingleton.GetInstance<SocketManager>().Connect($"bob_{Random.Range(1000, 999)}");
    }

}
