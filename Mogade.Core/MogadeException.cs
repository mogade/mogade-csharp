using System;

namespace Mogade
{
   public class MogadeException : Exception
   {
      public ErrorMessage Details { get; set; }

      public MogadeException() { }
      public MogadeException(ErrorMessage message) : this(message.Message, message.InnerException){}
      public MogadeException(string message) : base(message) { }
      public MogadeException(string message, Exception innerException) : base(message, innerException) { }
      public MogadeException(ErrorMessage message, Exception innerException) : base(message.Message, innerException)
      {
         Details = message;
      }
   }
}