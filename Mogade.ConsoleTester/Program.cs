using System;
using Mogade.Leaderboards;

namespace Mogade.ConsoleTester
{
   class Program
   {
      static void Main(string[] args)
      {         
         //connects to production by default, nice thing to have in a #if DEBUG...
         MogadeConfiguration.Configuration(m => m.ConnectToTest());


         var mogade = new Mogade("MY", "SECRET");
         var a = mogade.GameVersion();
         var b = mogade.GetGameConfiguration();
         var c = mogade.SaveScore("4cd5760b5a740810d3000002", new Score{Points =  232, UserName = "uname"});
         var d = mogade.GetLeaderboard("4cd5760b5a740810d3000002", LeaderboardScope.Overall, 1);
         Console.ReadLine();
      }
   }
}
