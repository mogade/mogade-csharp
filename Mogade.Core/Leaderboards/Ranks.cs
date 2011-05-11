using Newtonsoft.Json;

namespace Mogade.Leaderboards
{
   public class Ranks
   {
      [JsonProperty("1")]
      public int Daily { get; set; }
      [JsonProperty("2")]
      public int Weekly { get; set; }
      [JsonProperty("3")]
      public int Overall { get; set; }
      [JsonProperty("4")]
      public int Yesterday { get; set; }
   }
}