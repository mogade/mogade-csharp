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
         Assert.AreEqual("8a5c0159ea064a04a0602e0da40d7706adac07a0", Communicator.GetSignature(payload, "vegeta!"));
      }
   }   
}