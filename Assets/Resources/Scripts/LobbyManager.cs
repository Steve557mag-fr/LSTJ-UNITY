using UnityEngine;
using System.Linq;
using Newtonsoft.Json.Linq;
using NativeWebSocket;
using Unity.VisualScripting;

public class LobbyManager : GameSingleton
{
    public WebSocket websocket;
    
    async void Start()
    {
        websocket = new WebSocket("ws://localhost:3030");

        websocket.OnOpen += () =>
        {
            Debug.Log("Connection open!");
        };

        websocket.OnError += (e) =>
        {
            Debug.Log("Error! " + e);
        };

        websocket.OnClose += (e) =>
        {
            OnLog($"Connection closed! => {e}", LoggingSeverity.Error);
        };

        websocket.OnMessage += (bytes) =>
        {
            Debug.Log("OnMessage!");
            Debug.Log(bytes);

            // getting the message as a string
            // var message = System.Text.Encoding.UTF8.GetString(bytes);
            // Debug.Log("OnMessage! " + message);
        };

        // waiting for messages
        await websocket.Connect();

        JObject jobj = new JObject();
        jobj["sdsd"] = new JObject() { { "age", 13 }, {"pute", new JObject() } };
    }

    private void Update()
    {
#if !UNITY_WEBGL || UNITY_EDITOR
        websocket.DispatchMessageQueue();
#endif
    }

    async void SendWebSocketMessage()
    {
        if (websocket.State == WebSocketState.Open)
        {
            // Sending bytes
            await websocket.Send(new byte[] { 10, 20, 30 });

            // Sending plain text
            //await websocket.SendText("plain text message");
        }
    }

    private async void OnApplicationQuit()
    {
        await websocket.Close();
    }

    void OnLog(string message, LoggingSeverity severity = LoggingSeverity.Verbose)
    {
        Debug.Log($"[DISC/{severity}]: {message}");
    }

}
public enum LoggingSeverity
{
    Verbose = 1,
    Info = 2,
    Warning = 3,
    Error = 4,
    None = 5,
    Message = 6,
    State = 7,
}