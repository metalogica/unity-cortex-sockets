using UnityEngine;
using NativeWebSocket;


public class WebSocketConnection : MonoBehaviour {
  WebSocket websocket;
  [SerializeField] string clientId;
  [SerializeField] string clientSecret;
  string cortexToken;
  string sessionId;
  int debit = 0;

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

      if (debit >= 1)
      {
        HandleGetSessionToken(messageString);
      }
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
    if (clientId == "" || clientSecret == "")
    {
      Debug.LogWarning("Please provide both a client id and a client secret.");

      return;
    }

    if (websocket.State == WebSocketState.Open)
    {
      if (debit == 0)
      {
        string serializedMessage = buildGetSessiontokenEvent();
        Debug.Log("message sent: " + serializedMessage);
        await websocket.SendText(serializedMessage);

        debit += 1;
      }

      if (cortexToken != "" && sessionId == "")
      {
        Debug.Log("rock n roll");
      }
    }
  }

  private async void OnApplicationQuit()
  {
    await websocket.Close();
  }


  private void HandleGetUserResponse(string messageString)
  {
      CortexEvent cortexEvent = CortexEvent.CreateFromJSON(messageString);

      string id = cortexEvent.id;
      string jsonrpc = cortexEvent.jsonrpc;
      string username = cortexEvent.result[0].username;

      // cortexToken = cortexEvent.result[0].cortexToken;

      Debug.Log("Received OnMessage! " + messageString + "The `id` is: " + id + " . The cortex token is " + cortexToken);
  }

  private void HandleGetSessionToken(string messageString)
  {
    CortexEvent.GetSessionTokenResponse cortexEvent = JsonUtility.FromJson<CortexEvent.GetSessionTokenResponse>(messageString);
    Debug.Log("response");
    
    string cortexToken = cortexEvent.result.cortexToken;

    Debug.Log("The cortex token is " + cortexToken);

    this.cortexToken = cortexToken;
  }

  private string buildGetSessiontokenEvent()
  {
    CortexEvent.GetSessionTokenEvent getSession = new CortexEvent.GetSessionTokenEvent();
    getSession.id = 1;
    getSession.jsonrpc = "2.0";
    getSession.method = "authorize";
    CortexEvent.Params eventParams = new CortexEvent.Params();
    eventParams.clientId = clientId;
    eventParams.clientSecret = clientSecret;
    eventParams.debit = 1;
    getSession.@params = eventParams;
    string serializedMessage = getSession.SaveToString();
    
    return serializedMessage;
  }
}
