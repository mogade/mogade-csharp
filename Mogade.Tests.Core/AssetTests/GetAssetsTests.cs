using System;
using NUnit.Framework;

namespace Mogade.Tests.AssetTests
{
   public class GetAssetsTests : BaseFixture
   {
      [Test]
      public void SendsRequestForTheAssetsToTheServer()
      {
         Server.Stub(new ApiExpectation { Method = "GET", Url = "/gamma/assets", Request = "key=akey", Response = "[]" });
         new Driver("akey", "sssshh2").GetAssets(SetIfSuccess);
         WaitOne();
      }

      [Test]
      public void RetrievesTheAssets()
      {
         Server.Stub(new ApiExpectation { Response = "[{name: 'a-name', type: 33, dated: '2009-02-16T04:06:06Z', file: 'a.zip', meta: 'har'}]" });
         new Driver("akey", "sssshh2").GetAssets(response =>
         {
            Assert.AreEqual(1, response.Data.Count);
            Assert.AreEqual("a-name", response.Data[0].Name);
            Assert.AreEqual("a.zip", response.Data[0].File);
            Assert.AreEqual(33, response.Data[0].Type);
            Assert.AreEqual(new DateTime(2009, 2, 16, 4, 6, 6), response.Data[0].Dated.ToUniversalTime());
            Assert.AreEqual("har", response.Data[0].Meta);
            Set();
         });
         WaitOne();
      }
   }
}