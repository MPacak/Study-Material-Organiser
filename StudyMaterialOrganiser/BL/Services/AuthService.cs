using BL.IServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using BL.Models;
using Microsoft.AspNetCore.Http;
using BL.Security;
using DAL.Models;
using DAL.IRepositories;
using Microsoft.Extensions.Configuration;
namespace BL.Services;


public class AuthService : IAuthService
{
	private readonly IHttpContextAccessor _httpContextAccessor;
	private readonly IUnitOfWork _unitOfWork;
	private readonly IPasswordService _passwordService;
	private readonly IConfiguration _configuration;

	public AuthService(IHttpContextAccessor httpContextAccessor, IUnitOfWork unitOfWork,
		IPasswordService passwordService, IConfiguration configuration)
	{
		_httpContextAccessor = httpContextAccessor;
		_unitOfWork = unitOfWork;
		_passwordService = passwordService;
		_configuration = configuration;
	}

	public string GenerateToken(UserLoginDto request)
	{
		var user = Authenticate(request.Username, request.Password);

		if (user != null)
		{
			var jwtKey = _configuration["Jwt:Key"];
			var role = user.Role.ToString();


			var additionalClaims = new List<Claim>
			{
				new Claim(ClaimTypes.Role, role)
			};

			var token = JwtTokenProvider.CreateToken(jwtKey, 10, user.Username, additionalClaims);
			return token;
		}

		throw new Exception("Authentication failed");
	}

	public User Authenticate(string username, string password)
	{
		var user = _unitOfWork.User.GetFirstOrDefault(u => u.Username == username);
		if (user == null) throw new InvalidOperationException("User does not exist");

		if (!_passwordService.VerifyPassword(password, user.PwdHash, user.PwdSalt))
			throw new InvalidOperationException("Invalid password");

		return user;
	}


	public void SignIn(string username, int? role, int userId)
	{
		var claims = new List<Claim>
		{
			new Claim(ClaimTypes.Name, username),
			new Claim(ClaimTypes.Role, role switch
			{
				2 => "Admin",
				1 => "User",
				0 => "NonUser",
				_ => throw new ArgumentOutOfRangeException()
			}),
			new Claim(ClaimTypes.NameIdentifier, userId.ToString())
		};

		var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
		var authProperties = new AuthenticationProperties();

		Task.Run(async () =>
			await _httpContextAccessor.HttpContext.SignInAsync(
				CookieAuthenticationDefaults.AuthenticationScheme,
				new ClaimsPrincipal(claimsIdentity),
				authProperties)
		).GetAwaiter().GetResult();
	}

	public void SignOut()
	{
		Task.Run(async () =>
			await _httpContextAccessor.HttpContext.SignOutAsync(
				CookieAuthenticationDefaults.AuthenticationScheme)
		).GetAwaiter().GetResult();
	}
}