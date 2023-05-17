using Microsoft.AspNetCore.Identity;

namespace APIs.Security.JWT;

public class IdentityInitializer
{
    private readonly ApiSecurityDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public IdentityInitializer(
        ApiSecurityDbContext context,
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager)
    {
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public void Initialize()
    {
        if (_context.Database.EnsureCreated())
        {
            if (!_roleManager.RoleExistsAsync(Roles.ROLE_ACESSO_APIS!).Result)
            {
                var resultado = _roleManager.CreateAsync(
                    new IdentityRole(Roles.ROLE_ACESSO_APIS!)).Result;
                if (!resultado.Succeeded)
                {
                    throw new Exception(
                        $"Erro durante a criação da role {Roles.ROLE_ACESSO_APIS}.");
                }
            }

            CreateUser(
                new ApplicationUser()
                {
                    UserName = "usr01_apis",
                    Email = "usr01_apis@teste.com.br",
                    EmailConfirmed = true
                }, "Usr01ApiValido01!", Roles.ROLE_ACESSO_APIS);

            CreateUser(
                new()
                {
                    UserName = "usr02_apis",
                    Email = "usr02_apis@teste.com.br",
                    EmailConfirmed = true
                }, "Usr02ApiInvalido02!");
        }
    }

    private void CreateUser(
        ApplicationUser user,
        string password,
        string? initialRole = null)
    {
        if (_userManager.FindByNameAsync(user.UserName!).Result == null)
        {
            var resultado = _userManager
                .CreateAsync(user, password).Result;

            if (resultado.Succeeded &&
                !String.IsNullOrWhiteSpace(initialRole))
            {
                _userManager.AddToRoleAsync(user, initialRole).Wait();
            }
        }
    }
}