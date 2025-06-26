using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class UIManager : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI authLabel, lobbyPlayersCount;
    [SerializeField] GameObject joinLobbyButton, authButton;
    [SerializeField] GameObject lobbyContainer, authContainer;
    [SerializeField] UserSlot[] userSlots;



    public void Start()
    {
        GameSingleton.GetInstance<DiscordManager>().authDone += AuthFinished;
        GameSingleton.GetInstance<DiscordManager>().lobbyJoined += LobbyJoined;

    }

    private void LobbyJoined(LobbyData lobby)
    {
        DisplayLobby();
        UpdateLobby(lobby);
    }

    void DisplayLobby()
    {
        authContainer.SetActive(false);
        lobbyContainer.SetActive(true);
        //UpdateLobby();
    }

    void UpdateLobby(LobbyData lobby)
    {
        lobbyPlayersCount.text = $"{lobby.id} - ({lobby.users.Length}/{DiscordManager.maxLobbySize})";

        for(int i = 0; i < DiscordManager.maxLobbySize; i++)
        {
            var userExist = i < lobby.users.Length;
            userSlots[i].container.SetActive(userExist);
            if (!userExist) continue;

            userSlots[i].userNameText.text = lobby.users[i].userName;
        }
    }

    void AuthFinished()
    {
        var data = GameSingleton.GetInstance<DiscordManager>().currentUserData;
        if (!data.HasValue) return;

        authLabel.text = $"Bonjour {data.Value.userName}!";
        joinLobbyButton.SetActive(true);
        authButton.SetActive(false);

    }

}


[System.Serializable]
public struct UserSlot
{
    public GameObject container;
    public TextMeshProUGUI userNameText;
    public Image userProfilePic;
}
