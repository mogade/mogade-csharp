using Newtonsoft.Json;

namespace Mogade.Achievements
{
   public class Achievement
   {
      public string Id { get; set; }
      public string Name { get; set; }
      [JsonProperty("desc")]
      public string Description { get; set; }
      public int Points { get; set; }
   }
}