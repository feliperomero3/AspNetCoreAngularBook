using Microsoft.AspNetCore.Identity;

namespace WorldCitiesAPI.Entities;

public class ApplicationUser : IdentityUser
{
    public ApplicationUser(string userName) : base(userName) { }
}
