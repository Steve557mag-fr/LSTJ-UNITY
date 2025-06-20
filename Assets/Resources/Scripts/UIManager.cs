using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI authLabel;

    public void Start()
    {
        GameSingleton.GetInstance<DiscordManager>().authDone += AuthFinished;

    }

    void AuthFinished()
    {
        var data = GameSingleton.GetInstance<DiscordManager>().currentUserData;
        if (!data.HasValue) return;

        authLabel.text = $"Bonjour {data.Value.userName}!";

    }

}
