using NUnit.Framework;

namespace Mogade.Tests.LeaderboardsTest
{
   public class GetRankTests : BaseFixture
   {
      [Test]
      public void SendsIndividualRankRequestToTheServer()
      {
         Server.Stub(new ApiExpectation { Method = "GET", Url = "/ranks", Request = "lid=mybaloney&username=paul&userkey=jessica&scopes=3&v=2", Response = "{}" });
         new Driver("thekey", "sssshh").GetRank("mybaloney", "paul", "jessica", LeaderboardScope.Overall, SetIfSuccess);
         WaitOne();
      }

      [Test]
      public void SendMultipleRankRequestToServer()
      {
         Server.Stub(new ApiExpectation { Method = "GET", Url = "/ranks", Request = "lid=mybaloney&username=paul&userkey=jessica&scopes%5B%5D=1&scopes%5B%5D=2&v=2", Response = "{}" });
         new Driver("thekey", "sssshh").GetRanks("mybaloney", "paul", "jessica", new[]{LeaderboardScope.Daily, LeaderboardScope.Weekly}, SetIfSuccess);
         WaitOne();
      }

      [Test]
      public void SendsAllRanksRequestToServer()
      {
         Server.Stub(new ApiExpectation { Method = "GET", Url = "/ranks", Request = "lid=mybaloney&username=paul&userkey=jessica&scopes%5B%5D=1&scopes%5B%5D=2&scopes%5B%5D=3&scopes%5B%5D=4&v=2", Response = "{}" });
         new Driver("thekey", "sssshh").GetRanks("mybaloney", "paul", "jessica", SetIfSuccess);
         WaitOne();
      }

      [Test]
      public void RetrievesAllTheRanksFromTheResponse()
      {
         Server.Stub(new ApiExpectation { Response = @"{1: 20, 2: 25, 3: 45, 4:22}" });
         new Driver("thekey", "sssshh").GetRanks("mybaloney", "paul", "jessica", ranks =>
         {
            Assert.AreEqual(true, ranks.Success);
            Assert.AreEqual(20, ranks.Data.Daily);
            Assert.AreEqual(25, ranks.Data.Weekly);
            Assert.AreEqual(45, ranks.Data.Overall);
            Assert.AreEqual(22, ranks.Data.Yesterday);
            Set();
         });
         WaitOne();
      }

      [Test]
      public void RetrievesASingleRank()
      {
         Server.Stub(new ApiExpectation { Response = @"{4:55}" });
         new Driver("thekey", "sssshh").GetRank("mybaloney", "paul", "jessica", LeaderboardScope.Yesterday, rank =>
         {
            Assert.AreEqual(true, rank.Success);
            Assert.AreEqual(55, rank.Data);
            Set();
         });
         WaitOne();
      }
   }
}