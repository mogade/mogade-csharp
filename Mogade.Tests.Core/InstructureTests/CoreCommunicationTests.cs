using System.Collections.Generic;
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
         new Communicator(FakeContext.Defaults).SendPayload<object>("PUT", "anything", new Dictionary<string, object>(0), s =>
         {
            Assert.True(s.Raw.Contains(@"""v"":1"), "payload should contain the api version");
            Set();
         });
         WaitOne();
      }
      [Test]
      public void PayloadIncludesTheGameKey()
      {
         Server.Stub(ApiExpectation.EchoAll);
         new Communicator(new FakeContext { Key = "ItsOver9000!" }).SendPayload<object>("PUT", "anything", new Dictionary<string, object>(0), s =>
         {
            Assert.True(s.Raw.Contains(@"""key"":""ItsOver9000!"""), "payload should contain the game key version");
            Set();
         });
         WaitOne();
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
         new Communicator(FakeContext.Defaults).SendPayload<object>("PUT", "anything", payload, s =>
         {
            var response = JObject.Parse(s.Raw);
            Assert.AreEqual(response["key1"].Value<string>(), "value1");
            Assert.AreEqual(response["key2"].Value<decimal>(), 123.4);
            Assert.AreEqual(response.SelectToken("score.username").Value<string>(), "Leto");
            Assert.AreEqual(response.SelectToken("score.points").Value<int>(), 2);
            Set();
         });
         WaitOne();
      }

      [Test]
      public void WontTryToConnectIfNetworkCheckReturnsFalse()
      {
         DriverConfiguration.Configuration(c => c.NetworkAvailableCheck(() => false));
         new Communicator(FakeContext.Defaults).SendPayload<object>("PUT", "anything", null, s =>
         {
            Assert.AreEqual("Network is not available", s.Error.Message);
            Assert.AreEqual(false, s.Success);
            Set();
         });
         WaitOne();
      }

      [Test]
      public void NoNetworkAndNoCallbackDoestThrowException()
      {
         DriverConfiguration.Configuration(c => c.NetworkAvailableCheck(() => false));
         new Communicator(FakeContext.Defaults).SendPayload<object>("PUT", "anything", null, null);         
      }
   }
}