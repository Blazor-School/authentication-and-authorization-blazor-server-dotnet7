using AuthorizeOnRoute.Models;
using Microsoft.AspNetCore.Authorization;

namespace AuthorizeOnRoute.Requirements;

public class AdultRequirementHandler : AuthorizationHandler<AdultRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AdultRequirement requirement)
    {
        var user = User.FromClaimsPrincipal(context.User);

        if (user.Age >= requirement.MinimumAgeToConsiderAnAdult)
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