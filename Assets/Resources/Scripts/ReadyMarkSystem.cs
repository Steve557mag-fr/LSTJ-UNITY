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
        marks = new UserMark[maxUser];
        UpdateMarks(new());
    }

    public void UpdateMarks(JArray data)
    {
        int currentMark = 0;
        for (int i = 0; i < marks.Length; i++) {
            if (i < data.Count){

                marks[i].markState = data[i]["mark_state"].ToObject<bool>();
                marks[i].markState = data[i]["member_id"].ToObject<bool>();

                continue;
            }
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
