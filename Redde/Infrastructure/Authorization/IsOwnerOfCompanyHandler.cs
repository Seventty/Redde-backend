using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Redde.Application.Authorization;
using System.Security.Claims;

public class IsOwnerOfCompanyHandler(IHttpContextAccessor httpContextAccessor) : AuthorizationHandler<IsOwnerOfCompanyRequirement>
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        IsOwnerOfCompanyRequirement requirement)
    {
        var httpContext = _httpContextAccessor.HttpContext;
        var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);

        var companyIdStr = httpContext?.Request.RouteValues["id"]?.ToString();

        if (int.TryParse(userId, out var uid) &&
            int.TryParse(companyIdStr, out var cid))
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}
