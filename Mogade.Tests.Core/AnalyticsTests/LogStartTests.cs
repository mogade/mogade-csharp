namespace Mogade.Tests.AnalyticsTest
{
   using NUnit.Framework;

   public class LogStartTests : BaseFixture
   {
      [Test]
      public void SendsTheHitToTheServer()
      {
         Server.Stub(new ApiExpectation { Method = "PUT", Url = "/analytics/start", Request = @"{""unique"":""usersunique"",""key"":""akey"",""v"":1,""sig"":""308d60fbea974eebcc762036431c958f""}" });
         Server.OnInvoke(Set);
         new Driver("akey", "sssshh2").LogApplicationStart("usersunique");         
         WaitOne();
      }
   }
}