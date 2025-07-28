using Newtonsoft.Json.Linq;
using System;
using UnityEngine;

public class GameSpace : MonoBehaviour
{
    internal delegate void SpaceEnded();
    internal SpaceEnded onEndTriggered;

    public void Begin(JObject data)
    {
        OnBegin(data);
    }
    public void End()
    {
        OnEnd();
        onEndTriggered();
    }

    protected virtual void OnBegin(JObject data) { }
    protected virtual void OnEnd() { }
    protected virtual void OnDataReceived(JObject data) { }

    internal void ReceiveData(JObject receivedData)
    {
        OnDataReceived(receivedData);
    }
}
