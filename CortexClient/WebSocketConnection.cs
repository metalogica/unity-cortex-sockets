using UnityEngine;
using NativeWebSocket;

public class WebSocketConnection : MonoBehaviour {
  WebSocket websocket;

  async void Start()
  {
    websocket = new WebSocket("wss://localhost:6868");

    websocket.OnOpen += () => 
    {
      Debug.Log("Open!");
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
      var messageString = System.Text.Encoding.UTF8.GetString(bytes);
      // string json = JsonUtility.ToJson(messageString);
      // Debug.Log(json);

      Debug.Log("Received OnMessage! (" + bytes.Length + " bytes) " + messageString);
    };

    InvokeRepeating("SendWebSocketMessage", 0.0f, 1f);

    await websocket.Connect();
  }

  void Update()
  {
    websocket.DispatchMessageQueue();
  }

  async void SendWebSocketMessage()
  {
    if (websocket.State == WebSocketState.Open)
    {
      await websocket.SendText("{\"id\": \"1\", \"jsonrpc\": \"2.0\", \"method\": \"getUserLogin\"}");
    }
  }

  private async void OnApplicationQuit()
  {
    await websocket.Close();
  }
}
