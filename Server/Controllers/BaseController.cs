using Microsoft.AspNetCore.Mvc;

namespace Server.Controllers;

public abstract class BaseController<T> : ControllerBase where T : BaseController<T>
{
    private ILogger<T>? logger;

    protected ILogger<T> Logger => (logger ??= HttpContext.RequestServices.GetService<ILogger<T>>()) ?? throw new InvalidOperationException();
}