namespace Mogade
{
   public static class ValidationHelper
   {
      public static void AssertValidId(string id, string name)
      {
         AssertNotNullOrEmpty(id, name);
      }
      public static void AssertNotNull(object @object, string name)
      {
         if (@object == null)
         {
            throw new MogadeException(string.Format("{0} is required and cannot be null", name));
         }         
      }

      public static void AssertNotNullOrEmpty(string @string, string name)
      {
         if (string.IsNullOrEmpty(@string))
         {
            throw new MogadeException(string.Format("{0} is required and cannot be null or empty", name));
         }
      }

      public static void AssertMaximumLength(string @string, int maximumLength, string name)
      {
         if (@string != null && @string.Length > maximumLength)
         {
            throw new MogadeException(string.Format("{0} cannot be longer than {1} characters", name, maximumLength));
         }
      }
   }
}