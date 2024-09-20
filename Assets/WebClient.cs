using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Meta.Net.NativeWebSocket;
using System.Text;
using Newtonsoft.Json;
using OVRSimpleJSON;


public class WebClient : MonoBehaviour
{
    WebSocket websocket;
    public bool NewValue = false;
    public RootObject ObjInfo = null;
    public string IPAdd;

    // Start is called before the first frame update
    async void Start()
    {
        string serverAdd = "ws://" + IPAdd + ":8080";
        websocket = new WebSocket(serverAdd);

        websocket.OnOpen += () =>
        {
            Debug.Log("Connection open!");
        };

        websocket.OnError += (e) =>
        {
            Debug.Log("Error! " + e);
        };

        websocket.OnClose += (e) =>
        {
            Debug.Log("Connection closed!");
        };

        websocket.OnMessage += (bytes) =>
        {
            string message = Encoding.UTF8.GetString(bytes);
            Debug.Log("Received: " + message);
            ObjInfo = JsonConvert.DeserializeObject<RootObject>(message);
            NewValue = true;
        };
        await websocket.Connect();
    }

    void Update()
    {
#if !UNITY_WEBGL || UNITY_EDITOR
        websocket.DispatchMessageQueue();
#endif
    }


    private async void OnApplicationQuit()
    {
        await websocket.Close();
    }

}


