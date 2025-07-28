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
        GameSingleton.GetInstance<SocketManager>().onJoinedLobby += (JObject jo) =>
        {
            GameManager.GetCurrentSpace().Begin(new());
        };

        GameSingleton.GetInstance<SocketManager>().Connect($"bob_{Random.Range(100, 999)}");

    }

}
