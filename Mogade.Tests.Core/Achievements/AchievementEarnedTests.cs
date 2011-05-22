using NUnit.Framework;

namespace Mogade.Tests.Achievements
{
   public class AchievementEarnedTests : BaseFixture
   {
      [Test]
      public void SendsTheRequest()
      {
         Server.Stub(new ApiExpectation { Method = "POST", Url = "/achievements", Request = @"aid=123abc&username=paul&userkey=jessica&key=thekey&v=2&sig=5c3d993466e2c0d0ee990b594375cc0a43038821", Response = "{}" });
         new Driver("thekey", "sssshh").AchievementEarned("123abc", "paul", "jessica", SetIfSuccess);
         WaitOne();
      }

      [Test]
      public void GetsAnAchievementResponse()
      {
         Server.Stub(new ApiExpectation { Method = "POST", Url = "/achievements", Response = "{id: 'the_id', points: 286}" });
         new Driver("thekey", "sssshh").AchievementEarned("123abc", "paul", "jessica", r =>
         {
            Assert.AreEqual(true, r.Success);
            Assert.AreEqual("the_id", r.Data.Id);
            Assert.AreEqual(286, r.Data.Points);
            Set();
         });
         WaitOne();
      }

      [Test]
      public void GetsAnEmptyAchievement()
      {
         Server.Stub(new ApiExpectation { Method = "POST", Url = "/achievements", Response = "{}" });
         new Driver("thekey", "sssshh").AchievementEarned("123abc", "paul", "jessica", r =>
         {
            Assert.AreEqual(true, r.Success);
            Assert.AreEqual(null, r.Data.Id);
            Assert.AreEqual(0, r.Data.Points);
            Set();
         });
         WaitOne();
      }
   }
}