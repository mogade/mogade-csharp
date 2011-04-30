using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Mogade.JsonConverters
{
   public class LeaderboardHighScoresConverter : JsonConverter
   {
      public override bool CanConvert(Type objectType)
      {
         return (typeof(IDictionary<string, int>).IsAssignableFrom(objectType));
      }
      
      public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
      {
         //we never write this back out
      }
      
      public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
      {
         var entries = serializer.Deserialize<LeaderboardHighScoreEntry[]>(reader);
         var dictionary = (IDictionary<string, int>) existingValue;
         if (entries == null) { return null; }
         foreach (var entry in entries)
         {
            dictionary.Add(entry.Id, entry.Points);
         }
         return dictionary;
      }

      public class LeaderboardHighScoreEntry
      {
         public string Id { get; set; }
         public int Points { get; set; }
      }
   }
}