using System;

namespace Mogade.ConsoleTester
{
   class Program
   {
      static void Main(string[] args)
      {         
         //connects to production by default, nice thing to have in a #if DEBUG...
         MogadeConfiguration.Configuration(m => m.ConnectToTest());


         var mogade = new Mogade("4cd56def5a74080878000005", "4G@u?=o4>");

         Console.WriteLine(mogade.GameVersion());
         Console.ReadLine();
      }
   }
}
