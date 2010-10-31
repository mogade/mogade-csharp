using Newtonsoft.Json;

namespace Mogade.Leaderboard
{
   public class Score
   {
      [JsonProperty("username")]
      public string UserName { get; set; }
      [JsonProperty("points")]
      public int Points { get; set; }
      [JsonProperty("data")]
      public string Data { get; set; }
   }
}