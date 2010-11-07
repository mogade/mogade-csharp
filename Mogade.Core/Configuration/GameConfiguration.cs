using System.Collections.Generic;
using Mogade.Achievements;

namespace Mogade.Configuration
{
   public class GameConfiguration
   {
      private IList<Achievement> _achievements;

      public int Version { get; set; }
      public IList<Achievement> Achievements
      {
         get
         {
            if (_achievements == null)
            {
               _achievements = new List<Achievement>(0);
            }
            return _achievements;
         }
      }
   }
}