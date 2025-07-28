using Newtonsoft.Json.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class TechArt : GameSpace
{
    [SerializeField] JobSpace[] jobSpaces;
    
    [Header("Debug")]
    [SerializeField] bool inDebugging;
    [SerializeField] int forcedJobID = 1;
    [SerializeField] GameObject testingGI;

    int clientJobID = 0;


    protected override void OnBegin(JObject data)
    {
        clientJobID = inDebugging ? forcedJobID : GameSingleton.GetInstance<SocketManager>().GetClientIndexInLobby();
        for(int i = 0; i < jobSpaces.Length; i++)
        {
            jobSpaces[i].container.SetActive(i == clientJobID);
        }
    }

    public void SendItem(GameObject go)
    {
        SendItem(go, clientJobID + 1);
    }

    internal void SendItem(GameObject go, int toID = 1)
    {
        string path = $"Prefabs/SObjects/GameItems/{go.name.Replace("(Clone)","")}";
        GameItemObject item = Resources.Load<GameItemObject>(path);
        
        print($"go: " + go.name);
        print("path: " + path);
        print("item>: " + item);

        if (item != null) GameSingleton.GetInstance<SocketManager>().SendData(new()
        {
            {"method_name", "send_game_item_to_user_index"},
            {"data", new JObject(){ {"game_item_name", item.name}, {"job_id", toID}}}
        });
    }

    protected override void OnDataReceived(JObject data)
    {
        string method_name = data["method_name"].ToString();
        JObject param = data["data"].ToObject<JObject>();

        switch (method_name)
        {
            case "send_game_item_to_user_index":

                if (clientJobID != param["job_id"].ToObject<int>()) break;
                
                string path = $"Prefabs/SObjects/GameItems/{param["game_item_name"]}";
                GameItemObject item = Resources.Load<GameItemObject>(path);
                if (item == null) break;

                SpawnItem(item.prefab);
                break;

            default:
                break;
        }

        Debug.Log("new data : "+ param.ToString());

    }


    protected void SpawnItem(GameObject go)
    {
        GameObject g = Instantiate(go, jobSpaces[clientJobID].container.transform);
        g.transform.position = jobSpaces[clientJobID].spawnerPoint == null ? jobSpaces[clientJobID].spawnerPositionDefault : jobSpaces[clientJobID].spawnerPoint.position;

        print("hehhe");

        if (g.GetComponent<DragDrop2D>())
        {
            g.GetComponent<DragDrop2D>().forceDraggingAtAwake = false;
        }
    }

    public void SendCocoter(GameObject go)
    {
        if (!go.GetComponent<Cocoter>()) return;
        GameSingleton.GetInstance<SocketManager>().SendData(new()
        {
            {"method_name", "send_cocoter"},
            {"data", new JObject(){ 
                {"job_id", clientJobID + 1},
                {"values", go.GetComponent<Cocoter>().GetValues() }
            }}
        });
    }

    private void Update()
    {
        if (!inDebugging) return;
        if (Keyboard.current[Key.D].wasPressedThisFrame) {
            SendItem(testingGI, clientJobID); // self-send
        }
    }

}

[System.Serializable]
internal struct JobSpace
{
    [SerializeField] internal GameObject container;
    [SerializeField] internal Vector3 spawnerPositionDefault;
    [SerializeField] internal Transform spawnerPoint;
}
