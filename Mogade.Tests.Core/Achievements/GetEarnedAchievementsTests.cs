using System.Linq;
using NUnit.Framework;

namespace Mogade.Tests.Achievements
{
   public class GetEarnedAchievementsTests : BaseFixture
   {
      [Test]
      public void SendsTheRequest()
      {
         Server.Stub(new ApiExpectation { Method = "GET", Url = "/achievements", Request = "username=paul&userkey=jessica&key=thekey&v=2", Response = "[]" });
         new Driver("thekey", "sssshh").GetEarnedAchievements("paul", "jessica", SetIfSuccess);
         WaitOne();
      }

      [Test]
      public void DeserializesEarnedAchievements()
      {
         Server.Stub(new ApiExpectation { Method = "GET", Url = "/achievements", Response = "['its over', '9000']" });
         new Driver("thekey", "sssshh").GetEarnedAchievements("paul", "jessica", r =>
         {
            Assert.AreEqual(true, r.Success);
            Assert.AreEqual(2, r.Data.Count);
            Assert.AreEqual("its over", r.Data.ElementAt(0));
            Assert.AreEqual("9000", r.Data.ElementAt(1));
            Set();
         });
         WaitOne();
      }

      [Test]
      public void DeserializesEmptyAchievements()
      {
         Server.Stub(new ApiExpectation { Method = "GET", Url = "/achievements", Response = "[]" });
         new Driver("thekey", "sssshh").GetEarnedAchievements("paul", "jessica", r =>
         {
            Assert.AreEqual(true, r.Success);
            Assert.AreEqual(0, r.Data.Count);
            Set();
         });
         WaitOne();
      }
   }
}