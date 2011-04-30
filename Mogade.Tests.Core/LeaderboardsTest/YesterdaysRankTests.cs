namespace Mogade.Tests.LeaderboardsTest
{   
   using NUnit.Framework;

   public class YesterdaysRankTests : BaseFixture
   {
      [Test]
      public void SendsRequestToServer()
      {
         Server.Stub(new ApiExpectation { Method = "POST", Url = "/scores/yesterdays_rank", Request = @"{""leaderboard_id"":""mybaloney"",""username"":""Scytale"",""unique"":""gom jabbar"",""key"":""thekey"",""v"":1,""sig"":""134a21bc30cb7745b8376855988616ee""}"});
         new Driver("thekey", "sssshh").GetYesterdaysTopRank("mybaloney", "Scytale", "gom jabbar", SetIfSuccess);
         WaitOne();
      }

      [Test]
      public void RetrievesTheRankFromTheServer()
      {
         Server.Stub(new ApiExpectation { Response = "{rank: 54}" });
         new Driver("thekey", "sssshh").GetYesterdaysTopRank("mybaloney", "Scytale", "gom jabbar", rank =>
         {
            Assert.AreEqual(54, rank.Data);
            Set();
         });
         WaitOne();
      }
   }
}