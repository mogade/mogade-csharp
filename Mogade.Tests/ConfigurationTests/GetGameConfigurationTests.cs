using NUnit.Framework;

namespace Mogade.Tests.ConfigurationTests
{
   public class GetGameConfigurationTests : BaseFixture
   {
      [Test]
      public void SendsRequestForVersionTheServer()
      {
         Server.Stub(new ApiExpectation { Method = "POST", Url = "/conf", Request = @"{""key"":""akey"",""v"":1,""sig"":""b2cf06b6f6f1d7c3b51092670b005010""}", Response = "{version:0}" });
         new Mogade("akey", "sssshh2").GetGameConfiguration(r => { });
      }

      [Test]
      public void GetsTheVersionFromTheServer()
      {
         Server.Stub(new ApiExpectation { Response = "{version: 48, achievements: [{id: 'id1', name: 'first', points: 100, desc: 'dfirst'}, {id: 'id2', name: 'second', points: 200}]}" });
         new Mogade("akey", "sssshh2").GetGameConfiguration(configuration =>
         {
            Assert.AreEqual(48, configuration.Version);

            Assert.AreEqual(2, configuration.Achievements.Count);
            Assert.AreEqual("id1", configuration.Achievements[0].Id);
            Assert.AreEqual("first", configuration.Achievements[0].Name);
            Assert.AreEqual(100, configuration.Achievements[0].Points);
            Assert.AreEqual("dfirst", configuration.Achievements[0].Description);

            Assert.AreEqual("id2", configuration.Achievements[1].Id);
            Assert.AreEqual("second", configuration.Achievements[1].Name);
            Assert.AreEqual(200, configuration.Achievements[1].Points);
            Assert.AreEqual(null, configuration.Achievements[1].Description);
            Set();
         });
         WaitOne();
      } 
   }
}