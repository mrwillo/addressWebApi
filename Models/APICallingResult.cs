using Newtonsoft.Json;

namespace AddressWebAPI.Models
{
  public class APICallingResult
  {
    [JsonProperty("status")]
    public bool Status { get; set; }

    [JsonProperty("id")]
    public int? Id { get; set; }

    [JsonProperty("ProcessMessage")]
    public string ProcessMessage { get; set; }

    [JsonProperty("data")]
    public object Data { get; set; }
  }
}