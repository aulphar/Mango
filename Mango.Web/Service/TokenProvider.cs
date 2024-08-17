using Mango.Web.Service.IService;
using static Mango.Web.Utility.SD;

namespace Mango.Web.Service;

public class TokenProvider : ITokenProvider
{

    private readonly IHttpContextAccessor _contextAccessor;

    public TokenProvider(IHttpContextAccessor contextAccessor)
    {
        _contextAccessor = contextAccessor;
    }

    public void SetToken(string token)
    {
        _contextAccessor.HttpContext?.Response.Cookies.Append(TokenCookie, token);
    }

    public string? GetToken()
    {
        string? token = null;
        bool hasToken = _contextAccessor.HttpContext.Request.Cookies.TryGetValue(TokenCookie, out token);

        return hasToken is true ? token : null;
    }

    public void ClearToken()
    {
        _contextAccessor.HttpContext?.Response.Cookies.Delete(TokenCookie);
    }
}