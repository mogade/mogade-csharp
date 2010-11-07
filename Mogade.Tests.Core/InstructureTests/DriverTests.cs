using NUnit.Framework;

namespace Mogade.Tests
{
   public class DriverTests : BaseFixture
   {
      protected override bool NeedAServer
      {
         get { return false; }
      }

      [Test]
      public void ThrowsExceptionForNullOrEmptyGameKey()
      {
         AssertMogadeException("gameKey is required and cannot be null or empty", () => new Driver(null, "something"));
         AssertMogadeException("gameKey is required and cannot be null or empty", () => new Driver(string.Empty, "something"));
      }

      [Test]
      public void ThrowsExceptionForNullOrEmptySecret()
      {
         AssertMogadeException("secret is required and cannot be null or empty", () => new Driver("something", null));
         AssertMogadeException("secret is required and cannot be null or empty", () => new Driver("something", string.Empty));
      }
   }
}