using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace AuthenticationWithGoogle.Models;

public class User
{
    public string Username { get; set; } = "";
    public string Password { get; set; } = "";
    public int Age { get; set; }
    public List<string> Roles { get; set; } = new();

    public ClaimsPrincipal ToClaimsPrincipal() => new(new ClaimsIdentity(new Claim[]
    {
        new (ClaimTypes.Name, Username),
        new (ClaimTypes.Hash, Password),
        new (nameof(Age), Age.ToString())
    }.Concat(Roles.Select(r => new Claim(ClaimTypes.Role, r)).ToArray()),
    "BlazorSchool"));

    public static User FromClaimsPrincipal(ClaimsPrincipal principal) => new()
    {
        Username = principal.FindFirst(ClaimTypes.Name)?.Value ?? "",
        Password = principal.FindFirst(ClaimTypes.Hash)?.Value ?? "",
        Age = Convert.ToInt32(principal.FindFirst(nameof(Age))?.Value),
        Roles = principal.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList()
    };

    public static User? FromGoogleJwt(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        if (tokenHandler.CanReadToken(token))
        {
            var jwtSecurityToken = tokenHandler.ReadJwtToken(token);

            return new()
            {
                Username = jwtSecurityToken.Claims.First(c => c.Type == "name").Value,
                Password = ""
            };
        }

        return null;
    }
}