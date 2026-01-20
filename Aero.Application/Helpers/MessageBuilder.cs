using Aero.Application.Constants;

namespace Aero.Application.Helpers
{
    public sealed class MessageBuilder
    {
        public static string Unsuccess(string mac, string command)
        {
            return $"[{mac}] [{command}] :" + ResponseMessage.COMMAND_UNSUCCESS;
        }

        public static string Notfound() 
        {
            return $"hardware not found";
        }
    }
}
