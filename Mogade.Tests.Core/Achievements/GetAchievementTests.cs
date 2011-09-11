using System.Linq;
using NUnit.Framework;

namespace Mogade.Tests.Achievements
{
   public class GetAchievementTests : BaseFixture
   {
      [Test]
      public void SendsTheRequest()
      {
         Server.Stub(new ApiExpectation { Method = "GET", Url = "/gamma/achievements", Request = "key=thekey", Response = "[]" });
         new Driver("thekey", "sssshh").GetAchievements(SetIfSuccess);
         WaitOne();
      }

      [Test]
      public void DeserializesAchievements()
      {
         Server.Stub(new ApiExpectation { Method = "GET", Url = "/gamma/achievements", Response = "[{id:'123', name: 'the-name', description: 'the-desc', points: 234}]" });
         new Driver("thekey", "sssshh").GetAchievements(r =>
         {
            Assert.AreEqual(true, r.Success);
            Assert.AreEqual(1, r.Data.Count);
            Assert.AreEqual("123", r.Data.ElementAt(0).Id);
            Assert.AreEqual("the-name", r.Data.ElementAt(0).Name);
            Assert.AreEqual("the-desc", r.Data.ElementAt(0).Description);
            Assert.AreEqual(234, r.Data.ElementAt(0).Points);
            Set();
         });
         WaitOne();
      }

      [Test]
      public void DeserializesEmptyAchievements()
      {
         Server.Stub(new ApiExpectation { Method = "GET", Url = "/gamma/achievements", Response = "[]" });
         new Driver("thekey", "sssshh").GetAchievements(r =>
         {
            Assert.AreEqual(true, r.Success);
            Assert.AreEqual(0, r.Data.Count);
            Set();
         });
         WaitOne();
      }
   }
}