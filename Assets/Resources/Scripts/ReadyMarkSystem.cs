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
        GameSingleton.GetInstance<SocketManager>().onLobbyUpdate += UpdateSystem;
        ResetSystem();
    }

    public void ResetSystem(bool forceUI = true)
    {
        marks = new UserMark[maxUser];

    }

    public void UpdateSystem(JObject data)
    {
        Debug.Log(data.ToString());
    }

}


[System.Serializable]
public struct UserMark
{

    public Image markImage;
    public bool markState;
    public ulong userId;

}
