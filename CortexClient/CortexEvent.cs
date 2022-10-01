namespace CortexEvent {
  public class Result
  {
    public class CortexToken 
    {
      [JsonProperty("result")]
      public string result { get; }
    }

    public class Status
    {
      [JsonProperty("status")]
      public string status { get; }
      // activated, opened
    }

    public class SessionId
    {
      [JsonProperty("id")]
      public string sessionId { get; }
    }

    public class MentalCommand 
    {
      [JsonProperty("com")]
    }
  }
}
