namespace Mogade
{
   public interface IRequestContext
   {
      int ApiVersion{get;}
      string Secret { get; }
      string Key { get; }
   }
}