using UnityEngine;

public class UIManager : MonoBehaviour
{

    public void Start()
    {
        GameSingleton.GetInstance<DiscordManager>().authDone += AuthFinished;

    }

    void AuthFinished()
    {
        var data = GameSingleton.GetInstance<DiscordManager>().currentUserData;
        if (!data.HasValue)
        {
            print($"hi, my name is {data.Value.userName} shady");
        }
    }

}
