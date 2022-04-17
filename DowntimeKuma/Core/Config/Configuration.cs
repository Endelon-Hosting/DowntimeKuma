using Newtonsoft.Json;

namespace DowntimeKuma.Core.Config
{
    public class Configuration
    {
        [JsonProperty("firststart")]
        public bool FirstStart { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }

        [JsonProperty("label")]
        public string Label { get; set; }
    }
}
