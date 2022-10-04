using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class CortexEvent {
  public string id;
  public string jsonrpc;
  public List<Result> result;

  // public result response;
  [System.Serializable]
  public class Result {
    public string lastLoginTime;
    public string username;
    public string cortexToken;
    
  }
  public static CortexEvent CreateFromJSON(string jsonString)
  {
    return JsonUtility.FromJson<CortexEvent>(jsonString);
  }

  [System.Serializable]
  public class GetSessionTokenEvent{
    public int id;
    public string jsonrpc;
    public string method;
    public Params @params;

    public string SaveToString()
    {
      return JsonUtility.ToJson(this);
    }
  }

  [System.Serializable]
  public class Params
  {
    public string clientId;
    public string clientSecret;
    public int debit;
  }

  [System.Serializable]
  public class GetSessionTokenResponse
  {
    public int id;
    public string jsonrpc;
    public Result result;

    [System.Serializable]
    public class Result 
    {
      public string cortexToken;
    }
  }

  [System.Serializable]
  public class CreateSessionEvent
  {
    public int id;
    public string jsonrpc;
    public string method;
    public Params @params;

    [System.Serializable]
    public class Params
    {
      public string cortexToken;
      public string headset;
      public string status;
    }
  }

  [System.Serializable]
  public class SessionCreatedEvent
  {
    public int id;
    public string jsonrpc;
    public Result result;

    [System.Serializable]
    public class Result
    {
      public string id;
      public string status;
      public string appId;
      public Headset headset;
      public string license;
      public string owner;
      public List<PerformanceMetric> performanceMetrics;
      public List<object> recordIds;
      public bool recording;
      public string started;
      public string stopped;
      public List<object> streams;
    }

    [System.Serializable]
    public class Settings 
    {
      public int eegRate;
      public int eegRes;
      public int memsRate;
      public int memsRes;
      public string mode;
    }

    [System.Serializable]
    public class Headset 
    {
      public string connectedBy;
      public List<string> dfuTypes;
      public string dongle;
      public string firmware;
      public string id;
      public bool isDfuMode;
      public bool isVirtual;
      public List<string> motionSensors;
      public List<string> sensors;
      public Settings settings;
      public string status;
      public string virtualHeadsetId;
    }
    public class PerformanceMetric
    {
      public string apiName;
      public string displayName;
      public string name;
      public string shortDispalyName;
    }
  }

  [System.Serializable]
  public class QuerySessionRequest
  {
    public int id;
    public string jsonrpc;
    public string method;
    public Params @params;

    [System.Serializable]
    public class Params 
    {
      public string cortexToken;
    }
  }
}

// Started again.... START HERE
public class RequestMethods
{
  public string Authorize = "authorize";
}

[System.Serializable]
public class Request
{
  public int id = 1;
  public string jsonrpc = "2.0";
  public string method;

  public string SaveToString()
  {
    return JsonUtility.ToJson(this);
  }

  [System.Serializable]
  public class ActivateSession : Request {
    public Params @params;
    
    public ActivateSession(string method, Params param)
    {
      this.method = method;
      this.@params = param;
    }

    [System.Serializable]
    public class Params 
    {
      public string cortexToken;
      public string sessionId;
      public Params(string cortexToken, string sessionId)
      {
        this.cortexToken = cortexToken;
        this.sessionId = sessionId;
      }
    }
  }

  public class AuthorizeSession : Request {

  }

  public class CreateSession : Request {}

  public class StreamMentalCommand : Request {}

  public class CloseSession : Request {}
}
