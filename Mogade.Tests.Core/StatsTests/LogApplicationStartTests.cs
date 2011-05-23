using NUnit.Framework;

namespace Mogade.Tests.StatsTests
{
   public class LogApplicationStartTests : BaseFixture
   {
      [Test]
      public void SendsRequestToTheServer()
      {
         Server.Stub(new ApiExpectation { Method = "POST", Url = "/stats", Request = "userkey=imspecial&key=akey&v=2&sig=3f6350c0c1149c2a765dd1dc3db5c1eb5eb740cf", Response = null });
         new Driver("akey", "sssshh2").LogApplicationStart("imspecial", SetIfSuccess);
         WaitOne();
      }
   }
}