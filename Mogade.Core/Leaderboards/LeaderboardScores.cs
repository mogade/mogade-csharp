using System.Collections.Generic;
using Newtonsoft.Json;

namespace Mogade.Leaderboards
{
   public class LeaderboardScores
   {
      [JsonProperty("scores")]
      public IList<Score> Scores { get; set; }
      [JsonProperty("page")]
      public int Page { get; set; }
   }
}