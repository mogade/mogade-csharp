using NUnit.Framework;

namespace Mogade.Tests.ConfigurationTests
{
   public class GetGameConfigurationTests : BaseFixture
   {
      [Test]
      public void SendsRequestForVersionTheServer()
      {
         Server.Stub(new ApiExpectation { Method = "POST", Url = "/conf", Request = @"{""key"":""akey"",""v"":1,""sig"":""b2cf06b6f6f1d7c3b51092670b005010""}", Response = "{version:0}" });
         new Driver("akey", "sssshh2").GetGameConfiguration(SetIfSuccess);
         WaitOne();
      }

      [Test]
      public void GetsTheVersionFromTheServer()
      {
         Server.Stub(new ApiExpectation { Response = "{version: 48}" });
         new Driver("akey", "sssshh2").GetGameConfiguration(configuration =>
         {
            Assert.AreEqual(true, configuration.Success);
            Assert.AreEqual(48, configuration.Data.Version);         
            Set();
         });
         WaitOne();
      }
      [Test]
      public void GetsTheAchievementsFromTheServer()
      {
         Server.Stub(new ApiExpectation { Response = "{achievements: [{id: 'id1', name: 'first', points: 100, desc: 'dfirst'}, {id: 'id2', name: 'second', points: 200}]}" });
         new Driver("akey", "sssshh2").GetGameConfiguration(configuration =>
         {
            Assert.AreEqual(2, configuration.Data.Achievements.Count);
            Assert.AreEqual("id1", configuration.Data.Achievements[0].Id);
            Assert.AreEqual("first", configuration.Data.Achievements[0].Name);
            Assert.AreEqual(100, configuration.Data.Achievements[0].Points);
            Assert.AreEqual("dfirst", configuration.Data.Achievements[0].Description);

            Assert.AreEqual("id2", configuration.Data.Achievements[1].Id);
            Assert.AreEqual("second", configuration.Data.Achievements[1].Name);
            Assert.AreEqual(200, configuration.Data.Achievements[1].Points);
            Assert.AreEqual(null, configuration.Data.Achievements[1].Description);
            Set();
         });
         WaitOne();
      }
      [Test]
      public void GetsTheLeaderboardsFromTheServer()
      {
         Server.Stub(new ApiExpectation { Response = "{leaderboards: [{id: 'id1', name: 'first'}, {id: 'id2', name: 'second'}]}" });
         new Driver("akey", "sssshh2").GetGameConfiguration(configuration =>
         {
            Assert.AreEqual(2, configuration.Data.Leaderboards.Count);
            Assert.AreEqual("id1", configuration.Data.Leaderboards[0].Id);
            Assert.AreEqual("first", configuration.Data.Leaderboards[0].Name);

            Assert.AreEqual("id2", configuration.Data.Leaderboards[1].Id);
            Assert.AreEqual("second", configuration.Data.Leaderboards[1].Name);
            Set();
         });
         WaitOne();
      }
   }
}