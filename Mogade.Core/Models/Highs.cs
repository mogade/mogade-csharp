using Newtonsoft.Json;

namespace Mogade
{
   public class Highs
   {
      [JsonProperty("1")]
      public bool Daily { get; set; }
      [JsonProperty("2")]
      public bool Weekly { get; set; }
      [JsonProperty("3")]
      public bool Overall { get; set; }


      public bool GetByScope(LeaderboardScope scope)
      {
         switch (scope)
         {
            case LeaderboardScope.Daily:
               return Daily;
            case LeaderboardScope.Overall:
               return Overall;
            case LeaderboardScope.Weekly:
               return Weekly;
         }
         return false;
      }
   }
}