using Microsoft.AspNetCore.Identity;

namespace WorldCitiesAPI.Entities;

public class ApplicationRole : IdentityRole
{
    private ApplicationRole() : base() { }

    public ApplicationRole(string roleName) : base(roleName) { }
}
