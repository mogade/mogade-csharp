using System.Collections.Generic;
using Mogade.Leaderboards;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace Mogade.Tests
{
   public class CoreCommunicationTests : BaseFixture
   {
      [Test]
      public void PayloadIncludesTheVersion()
      {
         Server.Stub(ApiExpectation.EchoAll);
         var response = new Communicator(FakeContext.Defaults).SendPayload("PUT", "anything", new Dictionary<string, object>(0));
         Assert.True(response.Contains(@"""v"":1"), "payload should contain the api version");
      }
      [Test]
      public void PayloadIncludesTheGameKey()
      {
         Server.Stub(ApiExpectation.EchoAll);
         var response = new Communicator(new FakeContext { Key = "ItsOver9000!" }).SendPayload("PUT", "anything", new Dictionary<string, object>(0));
         Assert.True(response.Contains(@"""key"":""ItsOver9000!"""), "payload should contain the game key version");
      }
      [Test]
      public void PayloadGetsSerializedToJson()
      {
         Server.Stub(ApiExpectation.EchoAll);
         var payload = new Dictionary<string, object>
                       {
                          {"key1", "value1"},
                          {"key2", 123.4},
                          {"score", new {username = "Leto", points = 2}}
                       };
         var response = JObject.Parse(new Communicator(FakeContext.Defaults).SendPayload("PUT", "anything", payload));
         Assert.AreEqual(response["key1"].Value<string>(), "value1");
         Assert.AreEqual(response["key2"].Value<decimal>(), 123.4);
         Assert.AreEqual(response.SelectToken("score.username").Value<string>(), "Leto");
         Assert.AreEqual(response.SelectToken("score.points").Value<int>(), 2);
      }      
   }
}