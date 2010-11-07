using System;

namespace Mogade
{
   public interface IMogadeConfiguration
   {
      IMogadeConfiguration ConnectTo(string url);
      IMogadeConfiguration ConnectToTest();
   }
   /// <summary>
   /// Configures the internal workings of the library.
   /// </summary>
   /// <remarks>
   /// You probably don't need/want to use this class
   /// </remarks>
   public class MogadeConfiguration : IMogadeConfiguration
   {
      private ConfigurationData _data = new ConfigurationData();
      private static readonly MogadeConfiguration _configuration = new MogadeConfiguration();
      public static void Configuration(Action<IMogadeConfiguration> action)
      {
         action(_configuration);
      }
      public static void ResetToDefaults()
      {
         _configuration._data = new ConfigurationData();
      }
      public static IConfigurationData Data
      {
         get { return _configuration._data; }
      }

      
      public IMogadeConfiguration ConnectTo(string url)
      {
         _data.Url = url;
         return this;
      }

      public IMogadeConfiguration ConnectToTest()
      {
         _data.Url = ConfigurationData.TESTURL;
         return this;
      }
   }
}