using Newtonsoft.Json;

namespace Company.Function
{
    public class Counter
    {
        [JsonProperty("id")]
        public string id { get; set; } = "1"; // Set the counter ID
        
        [JsonProperty("count")]
        public int Count { get; set; }
    }
}
