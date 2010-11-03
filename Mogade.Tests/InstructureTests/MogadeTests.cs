using NUnit.Framework;

namespace Mogade.Tests
{
   public class MogadeTests : BaseFixture
   {
      protected override bool NeedAServer
      {
         get { return false; }
      }

      [Test]
      public void ThrowsExceptionForNullOrEmptyGameKey()
      {
         AssertMogadeException("gameKey is required and cannot be null or empty", () => new Mogade(null, "something"));
         AssertMogadeException("gameKey is required and cannot be null or empty", () => new Mogade(string.Empty, "something"));
      }

      [Test]
      public void ThrowsExceptionForNullOrEmptySecret()
      {
         AssertMogadeException("secret is required and cannot be null or empty", () => new Mogade("something", null));
         AssertMogadeException("secret is required and cannot be null or empty", () => new Mogade("something", string.Empty));
      }

      [Test]
      public void ReturnsTheLogo()
      {
         //this seems like a stupid way to test this
         Assert.AreEqual(251, Mogade.Logo.Width);
         Assert.AreEqual(54, Mogade.Logo.Height);
      }
   }
}