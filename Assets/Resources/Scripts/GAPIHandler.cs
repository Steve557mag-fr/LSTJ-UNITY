using UnityEngine;
using RestSharp;
using Newtonsoft.Json.Linq;

public class GAPIHandler : GameSingleton
{
    const string API_URL = "http://localhost:3000";
    RestClient restClient;

    private void Awake()
    {
        MakeInstance<GAPIHandler>();
        restClient = new(API_URL);
        
    }

    private void Start()
    {

        //Hello("jack");

    }

    void Hello(string userName)
    {
        // create the body
        var bodyJSON = new JObject();
        bodyJSON["user_name"] = userName;

        // create the request
        var request = new RestRequest($"hello", Method.POST);
        request.AddParameter("text/plain", bodyJSON.ToString(), ParameterType.RequestBody);

        // execute the req. & store the response
        var response = restClient.Execute(request);

        // handle response
        print(JObject.FromObject(response));
        Debug.Log($"response : {response.Content}");

        //if (response.IsSuccessful) Debug.Log($"response : {response.Content}");
        //else Debug.Log("ERR!");

    }



}
