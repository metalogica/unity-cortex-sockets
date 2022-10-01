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
    public string cortexToken;
    public string headset;
    public string status;
  }
}

// public static class JsonHelper
// {
//     public static T[] FromJson<T>(string json)
//     {
//         Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
//         return wrapper.Items;
//     }

//     public static string fixJson(string value)
//     {
//       value = "{\"Items\" :" + value + "}";
//       return value;
//     }

//     public static string ToJson<T>(T[] array)
//     {
//         Wrapper<T> wrapper = new Wrapper<T>();
//         wrapper.Items = array;
//         return JsonUtility.ToJson(wrapper);
//     }

//     public static string ToJson<T>(T[] array, bool prettyPrint)
//     {
//         Wrapper<T> wrapper = new Wrapper<T>();
//         wrapper.Items = array;
//         return JsonUtility.ToJson(wrapper, prettyPrint);
//     }

//     [System.Serializable]
//     private class Wrapper<T>
//     {
//         public T[] Items;
//     }
// }
