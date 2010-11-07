using System.Collections.Generic;

namespace Mogade.Configuration
{
   public class UserSettings
   {
      private IList<string> _achievements;
      public IList<string> Achievements
      {
         get
         {
            if (_achievements == null)
            {
               _achievements = new List<string>(5);
            }
            return _achievements;
         }
      }
   }
}