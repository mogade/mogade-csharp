using System.Collections.Generic;
using NUnit.Framework;

namespace Mogade.Tests
{
   public class SignatureTests : BaseFixture
   {
      protected override bool NeedAServer
      {
         get { return false; }
      }

      [Test]
      public void ProperlySignsThePayLoadBlackBox1()
      {
         var payload = new Dictionary<string, object> {{"itsover", 9000}, {"really", true}, {"howmuch?", "9000"}};
         Assert.AreEqual("b66fd76116f3d2549f33b1d1cf6f4347b73c3482", Communicator.GetSignature(payload, "vegeta!"));
      }
   }   
}