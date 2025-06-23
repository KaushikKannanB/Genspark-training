using System;

namespace Notify.Interfaces;

public interface IAuthService
{
    public Task<string> HandleGoogleLoginAsync(HttpContext httpContext);
}