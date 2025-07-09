using Newtonsoft.Json.Linq;
using UnityEngine;


public class BaseMinigame : MonoBehaviour
{
    internal delegate void OnMinigameStateChanged(int state = 0, JObject data = null);
    internal OnMinigameStateChanged minigameStateChanged;


    public void MGBegin()
    {
        Debug.Log($"[MG]: begin");
        OnReset();
        OnStart();
        minigameStateChanged(state: 1);
    }

    public void MGClose(bool isWinning = true)
    {
        Debug.Log($"[MG]: closed with win={isWinning}");
        OnEnd(isWinning);
        minigameStateChanged(state: 2, data: new(){"is_winning", isWinning});
    }

    public void ReceiveSocketPacket(JObject packet)
    {
        OnReceivedData(packet);
    }


    protected virtual void OnReceivedData(JObject data) { }
    protected virtual void OnReset() {}
    protected virtual void OnStart() {}
    protected virtual void OnEnd(bool isWinning = true) {}
    
}
