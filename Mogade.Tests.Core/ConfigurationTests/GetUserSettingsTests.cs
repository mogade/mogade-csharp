using NUnit.Framework;

namespace Mogade.Tests.ConfigurationTests
{
   public class GetUserSettingsTests : BaseFixture
   {
      [Test]
      public void SendsRequestToTheServer()
      {
         Server.Stub(new ApiExpectation { Method = "POST", Url = "/conf/my", Request = @"{""username"":""Edric"",""unique"":""2323"",""key"":""thekey"",""v"":1,""sig"":""6e274f04cc65861614e39e58ca29fb24""}" });
         new Driver("thekey", "sssshh").GetUserSettings("Edric", "2323", SetIfSuccess);
         WaitOne();
      }
      [Test]
      public void HandlesEmptyConfigurationFromServer()
      {
         Server.Stub(new ApiExpectation { Response = "{}" });
         new Driver("thekey", "sssshh").GetUserSettings("Edric", "2323", settings =>
         {
            Assert.AreEqual(true, settings.Success);
            Assert.AreEqual(0, settings.Data.Achievements.Count);
            Set();
         });
         WaitOne();
      }
      [Test]
      public void GetsAchievementConfigurationFromServer()
      {
         Server.Stub(new ApiExpectation { Response = "{achievements: ['abc', '123']}" });
         new Driver("thekey", "sssshh").GetUserSettings("Edric", "2323", settings =>
         {
            Assert.AreEqual(true, settings.Success);
            Assert.AreEqual(2, settings.Data.Achievements.Count);
            Assert.AreEqual("abc", settings.Data.Achievements[0]);
            Assert.AreEqual("123", settings.Data.Achievements[1]);
            Set();
         });
         WaitOne();
      }
      [Test]
      public void GetsTopScoresConfigurationFromServer()
      {
         Server.Stub(new ApiExpectation { Response = "{leaderboards: [{id: 'id1', points: 123}, {id: 'id5', points: 9001}]}" });
         new Driver("thekey", "sssshh").GetUserSettings("Edric", "2323", settings =>
         {
            Assert.AreEqual(true, settings.Success);
            Assert.AreEqual(2, settings.Data.LeaderboardHighScores.Count);
            Assert.AreEqual(123, settings.Data.LeaderboardHighScores["id1"]);
            Assert.AreEqual(9001, settings.Data.LeaderboardHighScores["id5"]);
            Set();
         });
         WaitOne();
      }
      [Test]
      public void NullOrEmptyUserNameCausesAnException()
      {
         AssertMogadeException("username is required and cannot be null or empty", () => new Driver("key", "secret").GetUserSettings(null, "something", r => { }));
         AssertMogadeException("username is required and cannot be null or empty", () => new Driver("key", "secret").GetUserSettings(string.Empty, "something", r => { }));
      }
      [Test]
      public void NullOrEmptyUniqueIdentifierCausesAnException()
      {
         AssertMogadeException("unique identifier is required and cannot be null or empty", () => new Driver("key", "secret").GetUserSettings("something", null, r => { }));
         AssertMogadeException("unique identifier is required and cannot be null or empty", () => new Driver("key", "secret").GetUserSettings("something", string.Empty, r => { }));
      }
      [Test]
      public void LongUserNameCausesAnException()
      {
         AssertMogadeException("username cannot be longer than 20 characters", () => new Driver("key", "secret").GetUserSettings(new string('c', 25), "something", r => { }));
      }
   }
}