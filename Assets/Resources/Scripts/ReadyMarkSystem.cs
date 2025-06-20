using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ReadyMarkSystem : MonoBehaviour
{

    [SerializeField] UserMark[] marks;

    int maxUser = 4;
    bool myMarkState = false;

    private void Start()
    {
        ResetSystem(false);
    }

    public void SetMaxUser(int _maxUser = 4) { maxUser = _maxUser; }

    public void ResetSystem(bool forceUI = true)
    {
        //maxUser = _maxUser;
        marks = new UserMark[maxUser];
    }

    public void UpdateMarks(JObject data)
    {
        int currentMark = 0;
        foreach (var mark in marks) {
            
        }

        // check for the global-ready activation
        if (currentMark == maxUser) WhenReady();

    }

    void SendMyMark(bool state = true)
    {
        myMarkState = state;
        //GameSingleton.GetInstance<DiscordManager>().MarkUserInLobby(myMarkState);
    }

    void WhenReady()
    {
        // do stuff..
        print("[RMARK]: all is marked!");
        ResetSystem(false);
    }

}


[System.Serializable]
public struct UserMark
{

    public Image markImage;
    public bool markState;
    public ulong userId;

}
