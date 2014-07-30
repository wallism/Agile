using Nancy;

namespace Agile.Web.Rest
{
    public interface ITokenValidator
    {
        bool Validate(string token, Request request = null);
    }
}