using AuthorizeOnIndividualComponent.Models;
using Microsoft.AspNetCore.Authorization;

namespace AuthorizeOnIndividualComponent.Requirements;

public class EsrbRequirementHandler : AuthorizationHandler<EsrbRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, EsrbRequirement requirement)
    {
        var user = User.FromClaimsPrincipal(context.User);
        int minimumAge = Convert.ToInt32(context.Resource);

        if (user.Age >= minimumAge)
        {
            context.Succeed(requirement);
        }
        else
        {
            context.Fail();
        }

        return Task.CompletedTask;
    }
}