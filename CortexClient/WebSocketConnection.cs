using UnityEngine;
using NativeWebSocket;

public enum State
{
  NoSession,
  SessionAuthorized,
  SessionActivated,
  SessionStreaming
}

public class WebSocketConnection : MonoBehaviour {
  WebSocket websocket;
  [SerializeField] string clientId = "";
  [SerializeField] string clientSecret = "";
  [SerializeField] string headset = "";
  [SerializeField] string cortexServerUrl = "wss://localhost:6868";
  string cortexToken;
  string sessionId;
  int debit;
  bool receivedCortexToken;
  State state = State.NoSession;

  async void Start()
  {
    websocket = new WebSocket(this.cortexServerUrl);

    websocket.OnOpen += () => 
    {
      if (state == State.NoSession)
      {
        if (HasCorrectConfig())
        {
          Debug.Log("read to rock n roll!");
        }
        else 
        {
          Debug.LogError("Missing credentials: Please ensure you have the correct configuration.");
        }
      }
    };

    websocket.OnError += (error) =>
    {
      Debug.Log(
        "Connection Error! Please double check your URL and ensure the cortex API is running"
        + "URL: " + this.cortexServerUrl
        + "Error: " + error
      );
    };

    websocket.OnClose += (e) =>
    {
      Debug.Log("Connection closed!");
    };

    websocket.OnMessage += (bytes) =>
    {
      Debug.Log("CONIFG: Debit is: "+this.debit+"cortex token is: "+this.cortexToken+"sessionId is: "+this.sessionId);
      Debug.Log("Message received");
      string message;
      var messageString = System.Text.Encoding.UTF8.GetString(bytes);

      if (debit >= 1 && sessionId == "")
      {
        message = HandleGetSessionToken(messageString);
        Debug.Log("Message received details: " + message);
      }

      if (receivedCortexToken)
      {
        message = HandleSessionCreatedEvent(messageString);
        Debug.Log("Message received details: " + message);
      }
    };

    // InvokeRepeating("SendWebSocketMessage", 0.0f, 2.5f);

    await websocket.Connect();
  }

  void Update()
  {
    websocket.DispatchMessageQueue();
  }

  bool HasCorrectConfig()
  {
    bool clientId = this.clientId.Length > 0;
    bool clientSecret = this.clientSecret.Length > 0;
    bool headset = this.headset.Length > 0;

    return clientId && clientSecret && headset;
  }

  async void SendWebSocketMessage()
  {
    Debug.Log("CONFIG: Debit is: "+this.debit+"cortex token is: "+this.cortexToken+"sessionId is: "+this.sessionId);
    if (clientId == "" || clientSecret == "")
    {
      Debug.LogWarning("Please provide both a client id and a client secret.");

      return;
    }

    if (websocket.State == WebSocketState.Open)
    {
      string serializedMessage = buildGetSessiontokenEvent();

      if (this.debit == 0)
      {
        serializedMessage = buildGetSessiontokenEvent();
        this.debit = 1;
      }

      if (receivedCortexToken)
      {
        serializedMessage = buildCreateSessionRequest();
        await websocket.SendText(serializedMessage);

        string querySession = BuildQuerySessionRequest();
        await websocket.SendText(querySession);
      }
      
      Debug.Log("message constructed: " + serializedMessage);
      await websocket.SendText(serializedMessage);
      Debug.Log("message sent!");
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

      // Debug.Log("Received OnMessage! " + messageString + "The `id` is: " + id + " . The cortex token is " + cortexToken);
  }

  private string HandleGetSessionToken(string messageString)
  {
    CortexEvent.GetSessionTokenResponse cortexEvent = JsonUtility.FromJson<CortexEvent.GetSessionTokenResponse>(messageString);
    
    string cortexToken = cortexEvent.result.cortexToken;

    if (this.cortexToken == "")
    {
      this.cortexToken = cortexToken;
      this.receivedCortexToken = true;
    }

    return cortexToken;
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

  private string buildCreateSessionRequest()
  {
    CortexEvent.CreateSessionEvent cortexEvent = new CortexEvent.CreateSessionEvent();

    cortexEvent.id = 1;
    cortexEvent.jsonrpc = "2.0";
    cortexEvent.method = "createSession";

    CortexEvent.CreateSessionEvent.Params eventParams = new CortexEvent.CreateSessionEvent.Params();
    eventParams.cortexToken = this.cortexToken;
    eventParams.headset = this.headset;
    eventParams.status = "active";
    cortexEvent.@params = eventParams;

    string serializedMessage = JsonUtility.ToJson(cortexEvent);

    return serializedMessage;
  }

  private string HandleSessionCreatedEvent(string messageString)
  {
    CortexEvent.SessionCreatedEvent cortexEvent = JsonUtility.FromJson<CortexEvent.SessionCreatedEvent>(messageString);
    
    string sessionId = cortexEvent.result.id;
    string status = cortexEvent.result.status;
    Debug.Log("DEBUG"+cortexEvent.result.status);

    if (this.sessionId == "")
    {
      this.sessionId = sessionId;
    }

    return $"Session Id is {sessionId}";
  }

  private string BuildQuerySessionRequest()
  {
    CortexEvent.QuerySessionRequest query = new CortexEvent.QuerySessionRequest();
  
    query.id = 1;
    query.jsonrpc = "2.0";
    query.method = "authorize";

    CortexEvent.QuerySessionRequest.Params eventParams = new CortexEvent.QuerySessionRequest.Params();
    eventParams.cortexToken = this.cortexToken;
    query.@params = eventParams;

    string serializedMessage = JsonUtility.ToJson(query);

    return serializedMessage;
  }
}
