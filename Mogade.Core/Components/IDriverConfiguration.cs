using System;

namespace Mogade
{
   public interface IDriverConfiguration
   {
      IDriverConfiguration ConnectTo(string url);
      IDriverConfiguration ConnectToTest();
      IDriverConfiguration NetworkAvailableCheck(Func<bool> networkCheck);

   }
   /// <summary>
   /// Configures the internal workings of the library.
   /// </summary>
   /// <remarks>
   /// You probably don't need/want to use this class
   /// </remarks>
   public class DriverConfiguration : IDriverConfiguration
   {
      private static readonly DriverData _data = new DriverData();
      private static readonly DriverConfiguration _configuration = new DriverConfiguration();
      public static void Configuration(Action<IDriverConfiguration> action)
      {
         action(_configuration);
      }

      protected DriverConfiguration(){}
      public static void ResetToDefaults()
      {
         _data.Reset();
      }
      public static IDriverData Data
      {
         get { return _data; }
      }

      
      public IDriverConfiguration ConnectTo(string url)
      {
         _data.Url = url;
         return this;
      }

      public IDriverConfiguration ConnectToTest()
      {
         _data.Url = DriverData.TESTURL;
         return this;
      }

      public IDriverConfiguration NetworkAvailableCheck(Func<bool> networkCheck)
      {
         _data.NetworkCheck = networkCheck;
         return this;
      }
   }
}