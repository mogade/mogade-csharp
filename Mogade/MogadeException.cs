using System;
using System.Runtime.Serialization;

namespace Mogade
{
   public class MogadeException : Exception
   {
      public string AdditionalInformation { get; set; }

      public MogadeException() { }
      public MogadeException(string message) : base(message) { }
      public MogadeException(string message, Exception innerException) : this(message, null, innerException) { }
      public MogadeException(string message, string info, Exception innerException) : base(message, innerException)
      {
         AdditionalInformation = info;
      }
      protected MogadeException(SerializationInfo info, StreamingContext context) : base(info, context)
      {
         AdditionalInformation = info.GetString("additionalInfo");
      }
      public override void GetObjectData(SerializationInfo info, StreamingContext context)
      {
         info.AddValue("additionalInfo", AdditionalInformation);
         base.GetObjectData(info, context);
      }
   }
}