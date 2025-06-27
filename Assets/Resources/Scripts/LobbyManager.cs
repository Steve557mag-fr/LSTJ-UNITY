using UnityEngine;
using System.Linq;
using MikeSchweitzer.WebSocket;
using Unity.VisualScripting;

public class LobbyManager : GameSingleton
{
    public WebSocketConnection websocket;
    public string url = "http://51.75.121.124:3030";

    private void Awake()
    {
        websocket.MessageReceived += OnMessageReceived;
        websocket.StateChanged += OnStateChanged;
    }

    private void OnDestroy()
    {
        websocket.MessageReceived -= OnMessageReceived;
        websocket.StateChanged += OnStateChanged;
    }

    public void Connect()
    {
        websocket.Connect(url);
    }

    public void Disconnect()
    {
        websocket.Disconnect();
    }

    private void SendString(string message)
    {
        websocket.AddOutgoingMessage(message);
    }

    void OnLog(string message, LoggingSeverity severity = LoggingSeverity.Verbose)
    {
        Debug.Log($"[DISC/{severity}]: {message}");
    }

    private void OnMessageReceived(WebSocketConnection connection, WebSocketMessage message)
    {
        OnLog(message.String, LoggingSeverity.Message);

    }

    private void OnStateChanged(WebSocketConnection connection, WebSocketState oldState, WebSocketState newState)
    {
        OnLog($"OnStateChanged oldState = {oldState} | newState = {newState}", LoggingSeverity.State);
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