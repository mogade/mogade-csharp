using NUnit.Framework;

namespace Mogade.Tests.StatsTests
{
   public class LogApplicationStartTests : BaseFixture
   {
      [Test]
      public void SendsRequestToTheServer()
      {
         Server.Stub(new ApiExpectation { Method = "POST", Url = "/gamma/stats", Request = "userkey=imspecial&key=akey&sig=ecdc09ff182d5b16f85517100c224639b3611258", Response = null });
         new Driver("akey", "sssshh2").LogApplicationStart("imspecial", SetIfSuccess);
         WaitOne();
      }
   }
}