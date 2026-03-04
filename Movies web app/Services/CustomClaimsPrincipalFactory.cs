using Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace Movies_web_app.Services
{
    public class CustomClaimsPrincipalFactory:UserClaimsPrincipalFactory<ApplicationUser, IdentityRole>
    {
        public CustomClaimsPrincipalFactory(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IOptions<IdentityOptions> options) : base(userManager, roleManager, options) { }
       
    
    protected override async Task<ClaimsIdentity> GenerateClaimsAsync(ApplicationUser user)
    {
        var identity = await base.GenerateClaimsAsync(user);

        identity.AddClaim(new Claim("FirstName", user.FirstName ?? "User"));
        identity.AddClaim(new Claim("LastName", user.LastName ?? ""));

        string defaultImage= user.Gender=="Female"?"~/Images/Female.png":"~/Images/Male.png";
        identity.AddClaim(new Claim("ProfilePicture", user.ProfilePictureUrl ?? defaultImage));
        return identity;
    }
    }
}
