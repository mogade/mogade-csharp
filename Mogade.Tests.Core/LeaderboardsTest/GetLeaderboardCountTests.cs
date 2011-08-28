using NUnit.Framework;

namespace Mogade.Tests.LeaderboardsTest
{
   public class GetLeaderboardCountTests : BaseFixture
   {
      [Test]
      public void SendsRequestForLeaderboardCountToTheServer()
      {
         Server.Stub(new ApiExpectation { Method = "GET", Url = "/gamma/scores/count", Request = "lid=theid&scope=2", Response = "0" });
         new Driver("akey", "sssshh2").GetLeaderboardCount("theid", LeaderboardScope.Weekly, SetIfSuccess);
         WaitOne();
      }

      [Test]
      public void RetrievesACount()
      {
         Server.Stub(new ApiExpectation { Response = "55" });
         new Driver("akey", "sssshh2").GetLeaderboardCount("theid", LeaderboardScope.Weekly, response =>
         {
            Assert.AreEqual(55, response.Data);
            Set();
         });
         WaitOne();
      }
   }
}