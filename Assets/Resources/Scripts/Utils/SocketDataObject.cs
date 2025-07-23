using Newtonsoft.Json.Linq;
using NUnit.Framework;
using UnityEngine;

[CreateAssetMenu(fileName = "SocketRequestObject", menuName = "Scriptable Objects/SocketRequestObject")]
public class SocketDataObject : ScriptableObject
{

    [SerializeField] string requestName;
    [SerializeField] JObject data = new JObject() { };

    void Execute()
    {
        GameSingleton.GetInstance<SocketManager>().SendData(new()
        {
            {"method_name", "send_data"},
            {"data", data}
        });
    }

}
