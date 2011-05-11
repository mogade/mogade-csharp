using System.Collections.Generic;
using NUnit.Framework;

namespace Mogade.Tests
{
   public class CoreCommunicationTests : BaseFixture
   {
      [Test]
      public void PayloadIncludesTheVersion()
      {
         Server.Stub(ApiExpectation.EchoAll);
         new Communicator(FakeContext.Defaults).SendPayload<object>("POST", "anything", new Dictionary<string, object>(0), s =>
         {
            Assert.True(s.Raw.Contains("v=2"), "payload should contain the api version");
            Set();
         });
         WaitOne();
      }
      [Test]
      public void PayloadIncludesTheGameKey()
      {
         Server.Stub(ApiExpectation.EchoAll);
         new Communicator(new FakeContext { Key = "ItsOver9000!" }).SendPayload<object>("GET", "anything", new Dictionary<string, object>(0), s =>
         {
            Assert.True(s.Raw.Contains("key=ItsOver9000!"), "payload should contain the game key version");
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