using NUnit.Framework;

namespace Mogade.Tests.StatsTests
{
   public class LogApplicationStartTests : BaseFixture
   {
      [Test]
      public void SendsRequestToTheServer()
      {
         Server.Stub(new ApiExpectation { Method = "POST", Url = "/stats", Request = "unique=imspecial&key=akey&v=2&sig=7e7b8613f0e9080b3336dd7f98c80fe50915328c", Response = null });
         new Driver("akey", "sssshh2").LogApplicationStart("imspecial", SetIfSuccess);
         WaitOne();
      }
   }
}