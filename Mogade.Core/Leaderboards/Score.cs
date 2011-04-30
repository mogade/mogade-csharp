using System;
using Newtonsoft.Json;

namespace Mogade.Leaderboards
{
   public class Score
   {
      public string UserName { get; set; }
      public int Points { get; set; }
      public string Data { get; set; }
      [JsonProperty("cat")]
      public DateTime Date { get; set; }
   }
}