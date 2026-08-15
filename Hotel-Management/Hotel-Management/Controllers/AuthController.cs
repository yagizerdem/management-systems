
using Common;
using Common.DTO;
using Entity.Base;
using Entity.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

[Route("api/[controller]")]
[ApiController]

public class AuthController : ControllerBase
{
	private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;
    private readonly IConfiguration _config;

    public AuthController(UserManager<AppUser> userManager, 
        SignInManager<AppUser> signInManager, 
        RoleManager<IdentityRole<Guid>> roleManager,
        IConfiguration config)
	{
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
        _config = config;
	}

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] RegisterDTO registerdto)
    {
        AppUser user = new AppUser()
        {
            UserName = registerdto.Email, // do not care username for hotel management system
            FirstName = registerdto.FirstName,
            LastName = registerdto.LastName,
            Email = registerdto.Email,
        };

        // default role is customer
        string userrole = Roles.Customer;

        IdentityResult result = await _userManager.CreateAsync(user, registerdto.Password);
        
        if(!result.Succeeded)
        {
            throw new Exception("fucker");
        }

        await this._userManager.AddToRoleAsync(user, userrole);


        return Ok();
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginDTO logindto)
    {

        var result = await _signInManager.PasswordSignInAsync(
            logindto.Email, 
            logindto.Password, 
            logindto.RememberMe,
            lockoutOnFailure: true);


        if(!result.Succeeded)
        {
            throw new Exception("fucker");
        }

        AppUser? user = await _userManager.FindByEmailAsync(logindto.Email);

        if (user == null || user.EntityStatus != EntityStatus.ACTIVE)
        {
            throw new Exception("fucker");
        }


        IList<string> role = await _userManager.GetRolesAsync(user);
        string joinedRole = string.Join(";", role);

        // generate jwt 
        var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(ClaimTypes.Name, user.FirstName),
                    new Claim(ClaimTypes.Surname, user.LastName),
                    new Claim("id", user.Id.ToString()),
                    new Claim("roles", joinedRole)
                };

        JwtSecurityToken token = GetToken(authClaims);

        CookieOptions options = new CookieOptions
        {
            Expires = DateTime.Now.AddDays(7), // Persistent cookie for 7 days
            HttpOnly = true,                   // Blocks client-side JavaScript access (prevents XSS)
            Secure = true,                     // Transmits only over HTTPS connections
            SameSite = SameSiteMode.Strict     // Guards against Cross-Site Request Forgery (CSRF)
        };

        // Response.Cookies.Append("jwt", token.RawData, options);

        return Ok(new
        {
            token = new JwtSecurityTokenHandler().WriteToken(token),
            expiration = token.ValidTo
        });
        
    }

    [HttpPost("logout")]
    [AllowAnonymous]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return Ok();
    }

    [HttpGet("is-logged-in")]
    [Authorize]
    public async Task<IActionResult> IsLoggedIn()
    {
        return Ok();
    }

    private JwtSecurityToken GetToken(List<Claim> authClaims)
    {
        IConfigurationSection section = _config.GetSection("Jwt");

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(section["Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        return new JwtSecurityToken(
            issuer: section["Issuer"],
            audience: section["Audience"],
            expires: DateTime.Now.AddDays(60),
            claims: authClaims,
            signingCredentials: creds
        );
    }



}
