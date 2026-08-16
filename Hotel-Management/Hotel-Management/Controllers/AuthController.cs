
using Common;
using Common.DTO;
using DAL.Context;
using Entity.Base;
using Entity.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

[Route("api/[controller]")]
[ApiController]

public class AuthController : ControllerBase
{
	private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;
    private readonly HotelManagementContext _context;

    private readonly IConfiguration _config;

    public AuthController(UserManager<AppUser> userManager, 
        SignInManager<AppUser> signInManager, 
        RoleManager<IdentityRole<Guid>> roleManager,
        HotelManagementContext context,
        IConfiguration config)
	{
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
        _context = context;
        _config = config;
	}

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] RegisterDTO registerdto)
    {
        await using var transaction =
      await _context.Database.BeginTransactionAsync();

        try
        {
            var user = new AppUser
            {
                UserName = registerdto.Email,
                FirstName = registerdto.FirstName,
                LastName = registerdto.LastName,
                Email = registerdto.Email,
            };

            const string userRole = Roles.Customer;

            var createResult =
                await _userManager.CreateAsync(user, registerdto.Password);

            if (!createResult.Succeeded)
            {

                List<DiagnosticDetails> details = createResult.Errors.Select(e => 
                new DiagnosticDetails()
                {
                    ErrorCode = ErrorCode.UNKNOWN_ERROR,
                    Message = e.Description
                }).ToList();

                throw new AppException("User registration failed")
                {
                    ErrorCode = ErrorCode.USER_REGISTRATION_FAILED,
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorDiagnostic = new ErrorDiagnostic
                    {
                        ComponentName = nameof(AuthController),
                        MethodName = nameof(Register),
                        Details = details
                    },
                    IsOperational = true
                };
            }

            var roleResult =
                await _userManager.AddToRoleAsync(user, userRole);

            if (!roleResult.Succeeded)
            {

                List<DiagnosticDetails> details = roleResult.Errors.Select(e => 
                new DiagnosticDetails()
                {
                    ErrorCode = ErrorCode.UNKNOWN_ERROR,
                    Message = e.Description
                }).ToList();

                throw new AppException("Failed to assign default user role")
                {
                    ErrorCode = ErrorCode.USER_ROLE_ASSIGNMENT_FAILED,
                    StatusCode = HttpStatusCode.InternalServerError,
                    ErrorDiagnostic = new ErrorDiagnostic
                    {
                        ComponentName = nameof(AuthController),
                        MethodName = nameof(Register),
                        Details = details
                    },
                    IsOperational = true
                };
            }

            await transaction.CommitAsync();

            return CreatedAtAction($"{nameof(AuthController)}/{nameof(Register)}", ApiResponse<AppUser>.Created(user));
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginDTO logindto)
    {

        AppUser? user = await _userManager.FindByEmailAsync(logindto.Email);
        if(user == null)
        {
            throw new AppException("User not found")
            {
                ErrorCode = ErrorCode.USER_NOT_FOUND,
                StatusCode = HttpStatusCode.Unauthorized,
                ErrorDiagnostic = new ErrorDiagnostic
                {
                    ComponentName = nameof(AuthController),
                    MethodName = nameof(Login),
                },
                IsOperational = true
            };
        }

        PasswordVerificationResult result = _userManager.PasswordHasher.VerifyHashedPassword(user, user.PasswordHash, logindto.Password);
        if(result != PasswordVerificationResult.Success)
        {
            throw new AppException("Invalid password")
            {
                ErrorCode = ErrorCode.INVALID_PASSWORD,
                StatusCode = HttpStatusCode.Unauthorized,
                ErrorDiagnostic = new ErrorDiagnostic
                {
                    ComponentName = nameof(AuthController),
                    MethodName = nameof(Login),
                },
                IsOperational = true
            };
        }

        IList<string> roles = await _userManager.GetRolesAsync(user);


        // generate jwt 
        var authClaims = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Email, user.Email!),
                    new Claim(ClaimTypes.Name, user.FirstName),
                    new Claim(ClaimTypes.Surname, user.LastName)
                };


        foreach (var role in roles)
        {
            authClaims.Add(new Claim(ClaimTypes.Role, role));
        }

        JwtSecurityToken token = GetToken(authClaims);


        return Ok(new
        {
            accessToken = new JwtSecurityTokenHandler().WriteToken(token),
            expiration = token.ValidTo
        });
        
    }

    [HttpPost("logout")]
    [AllowAnonymous]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return Ok(ApiResponse<string>.Ok("Successfully logged out"));
    }

    [HttpGet("is-logged-in")]
    [Authorize]
    public async Task<IActionResult> IsLoggedIn()
    {
        return Ok(ApiResponse<string>.Ok("User is logged in"));
    }


    [HttpGet("has-admin-role")]
    [Authorize(Roles = Roles.Admin)]
    public IActionResult IsAdmin()
    {
        return Ok(ApiResponse<string>.Ok("User has admin role"));
    }

    [HttpGet("has-manager-role")]
    [Authorize(Roles = Roles.Manager)]
    public IActionResult IsManager()
    {
        return Ok(ApiResponse<string>.Ok("User has manager role"));
    }

    [HttpGet("has-reception-chief-role")]
    [Authorize(Roles = Roles.ReceptionChief)]
    public IActionResult IsReceptionChief()
    {
        return Ok(ApiResponse<string>.Ok("User has reception chief role"));
    }

    [HttpGet("has-receptionist-role")]
    [Authorize(Roles = Roles.Receptionist)]
    public IActionResult IsReceptionist()
    {
        return Ok(ApiResponse<string>.Ok("User has receptionist role"));
    }

    [HttpGet("has-it-role")]
    [Authorize(Roles = Roles.IT)]
    public IActionResult IsIT()
    {
        return Ok(ApiResponse<string>.Ok("User has IT role"));
    }

    [HttpGet("has-customer-role")]
    [Authorize(Roles = Roles.Customer)]
    public IActionResult IsCustomer()
    {
        return Ok(ApiResponse<string>.Ok("User has customer role"));
    }

    private JwtSecurityToken GetToken(List<Claim> authClaims)
    {
        IConfigurationSection section = _config.GetSection("Jwt");

        string accessTokenMinutes = section["AccessTokenMinutes"] ??
            throw new AppException("AccessTokenMinutes is not configured")
            {
                ErrorCode = ErrorCode.INVALID_CONFIGURATION,
                StatusCode = HttpStatusCode.InternalServerError,
                ErrorDiagnostic = new ErrorDiagnostic
                {
                    ComponentName = nameof(AuthController),
                    MethodName = nameof(Login),
                },
                IsOperational = false
            };

        int minutes = int.Parse(accessTokenMinutes);

        string jwtKey = section["Key"] ??
            throw new AppException("Jwt Key is not configured")
            {
                ErrorCode = ErrorCode.INVALID_CONFIGURATION,
                StatusCode = HttpStatusCode.InternalServerError,
                ErrorDiagnostic = new ErrorDiagnostic
                {
                    ComponentName = nameof(AuthController),
                    MethodName = nameof(Login),
                },
                IsOperational = false
            };


        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        return new JwtSecurityToken(
            issuer: section["Issuer"],
            audience: section["Audience"],
            expires: DateTime.Now.AddMinutes(minutes),
            claims: authClaims,
            signingCredentials: creds
        );
    }


}
