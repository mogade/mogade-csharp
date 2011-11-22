using System.Collections.Generic;
using NUnit.Framework;

namespace Mogade.Tests
{
   public class CoreCommunicationTests : BaseFixture
   {
      [Test]
      public void PayloadIncludesTheGameKey()
      {
         Server.Stub(ApiExpectation.EchoAll);
         new Communicator(new FakeContext { Key = "ItsOver9000!" }).SendPayload<object>("POST", "anything", new Dictionary<string, object>(0), s =>
         {
            Assert.True(s.Raw.Contains("key=ItsOver9000!"), "payload should contain the game key version");
            Set();
         });
         WaitOne();
      }

      [Test]
      public void PayloadProperlyHandlesIEnumerables()
      {
         Server.Stub(ApiExpectation.EchoAll);
         new Communicator(new FakeContext { Key = "ItsOver9000!" }).SendPayload<object>("GET", "anything", new Dictionary<string, object>{{"scopes", new[]{2,5,6}}}, s =>
         {
            Assert.True(s.Raw.Contains("scopes%5B%5D=2&scopes%5B%5D=5&scopes%5B%5D=6"), "payload should contain the game key version");
            Set();
         });
         WaitOne();
      }

      [Test]
      public void PayloadPropertyEncodesValues()
      {
         Server.Stub(ApiExpectation.EchoAll);
         new Communicator(new FakeContext { Key = "ItsOver9000!" }).SendPayload<object>("GET", "anything", new Dictionary<string, object> { { "data", "2 + 3 = 5" } }, s =>
         {
            Assert.True(s.Raw.Contains("data=2%20%2B%203%20%3D%205"), "payload should contain the game key version");
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