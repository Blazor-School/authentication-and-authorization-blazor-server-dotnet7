using Microsoft.AspNetCore.Authorization;

namespace AuthorizeOnRoute.Requirements;

public class AdultRequirement : IAuthorizationRequirement
{
    public int MinimumAgeToConsiderAnAdult { get; set; } = 18;
}