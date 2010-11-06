using System.Collections.Generic;
using Mogade.Achievements;

namespace Mogade.Configuration
{
   public class GameConfiguration
   {
      public int Version { get; private set; }
      public IList<Achievement> Acheivements { get; private set; }
   }
}