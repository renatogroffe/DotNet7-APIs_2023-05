using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace APIs.Security.JWT;

public class ApiSecurityDbContext : IdentityDbContext<ApplicationUser>
{
    public ApiSecurityDbContext(DbContextOptions<ApiSecurityDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
    }
}