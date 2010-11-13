using Newtonsoft.Json;

namespace Mogade.Leaderboards
{
   public class Ranks
   {
      public int Daily { get; set; }
      public int Weekly { get; set; }
      public int Overall { get; set; }
      [JsonProperty("top_score")]
      public bool TopScore { get; set; }
   }
}