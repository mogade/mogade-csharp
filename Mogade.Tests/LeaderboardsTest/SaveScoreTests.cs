using Mogade.Leaderboards;
using NUnit.Framework;

namespace Mogade.Tests.LeaderboardsTest
{
   public class SaveScoreTests : BaseFixture
   {
      [Test]
      public void SendsScoreWithoutDataToTheServer()
      {
         Server.Stub(new ApiExpectation { Method = "PUT", Url = "/scores", Request = @"{""leaderboard_id"":""mybaloney"",""score"":{""username"":""Scytale"",""points"":10039},""key"":""thekey"",""v"":1,""sig"":""a4a793793b09c24bb9d4af726634aec4""}" });
         var score = new Score {Points = 10039, UserName = "Scytale"};
         new Mogade("thekey", "sssshh").SaveScore("mybaloney", score);
      }

      [Test]
      public void SendsScoreWithDataToTheServer()
      {
         Server.Stub(new ApiExpectation { Method = "PUT", Url = "/scores", Request = @"{""leaderboard_id"":""mybaloney"",""score"":{""username"":""Scytale"",""points"":10039,""data"":""mydata""},""key"":""thekey"",""v"":1,""sig"":""1e50fc7b84c0cd1d61ad543a5618c821""}" });
         var score = new Score { Points = 10039, UserName = "Scytale", Data = "mydata" };
         new Mogade("thekey", "sssshh").SaveScore("mybaloney", score);
      }


      [Test]
      public void RetrievesAllTheRanksFromTheResponse()
      {
         Server.Stub(new ApiExpectation { Response = @"{""daily"": 20, ""weekly"": 25, ""overall"": 45}" });
         var score = new Score { Points = 10039, UserName = "Scytale" };
         var ranks = new Mogade("thekey", "sssshh").SaveScore("mybaloney", score);
         Assert.AreEqual(20, ranks.Daily);
         Assert.AreEqual(25, ranks.Weekly);
         Assert.AreEqual(45, ranks.Overall);
      }

      [Test]
      public void RetrievesAnEmptyRankSet() //SaveScore isn't guaranteed to return all, or even any rank
      {
         Server.Stub(new ApiExpectation { Response = @"{}" });
         var score = new Score { Points = 10039, UserName = "Scytale" };
         var ranks = new Mogade("thekey", "sssshh").SaveScore("mybaloney", score);
         Assert.AreEqual(0, ranks.Daily);
         Assert.AreEqual(0, ranks.Weekly);
         Assert.AreEqual(0, ranks.Overall);
      }

      [Test]
      public void RetrievesAnPartialRankSet() //SaveScore isn't guaranteed to return all, or even any rank
      {
         Server.Stub(new ApiExpectation { Response = @"{""weekly"": 49494}" });
         var score = new Score { Points = 10039, UserName = "Scytale" };
         var ranks = new Mogade("thekey", "sssshh").SaveScore("mybaloney", score);
         Assert.AreEqual(0, ranks.Daily);
         Assert.AreEqual(49494, ranks.Weekly);
         Assert.AreEqual(0, ranks.Overall);
      }

      [Test]
      public void NullOrEmptyLeaderboardIdCausesAnExceptionToBeThrown()
      {
         AssertMogadeException("leaderboardId is required and cannot be null or empty", () => new Mogade("key", "secret").SaveScore(null, new Score()));
         AssertMogadeException("leaderboardId is required and cannot be null or empty", () => new Mogade("key", "secret").SaveScore(string.Empty, new Score()));
      }

      [Test]
      public void NullScoreCausesAnExceptionToBeThrown()
      {
         AssertMogadeException("score is required and cannot be null", () => new Mogade("key", "secret").SaveScore("abc", null));
      }

      [Test]
      public void LongDataCausesAnExceptionToBeThrown()
      {
         AssertMogadeException("score data cannot be longer than 25 characters", () => new Mogade("key", "secret").SaveScore("abc", new Score{Data = new string('a', 26)}));
      }
      [Test]
      public void NullOrEmptyUserNameCausesAnException()
      {
         AssertMogadeException("score username is required and cannot be null or empty", () => new Mogade("key", "secret").SaveScore("abc", new Score()));
         AssertMogadeException("score username is required and cannot be null or empty", () => new Mogade("key", "secret").SaveScore("abc", new Score { UserName = string.Empty }));
      }
      [Test]
      public void LongUserNameCausesAnException()
      {
         AssertMogadeException("score username cannot be longer than 20 characters", () => new Mogade("key", "secret").SaveScore("abc", new Score{UserName = new string('a', 21)}));         
      }
   }
}