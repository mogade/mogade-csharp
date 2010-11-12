using System.Collections.Generic;
using Newtonsoft.Json;

namespace Mogade.Leaderboards
{
   public class Leaderboard
   {
      [JsonProperty("scores")]
      public IList<Score> Scores { get; set; }
   }
}