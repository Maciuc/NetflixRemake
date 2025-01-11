using Backend.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using NetflixRemake.Email;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static Backend.Auth.AccountModel;

namespace Backend.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;


        private HttpContext _httpContext;

        public AuthController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IConfiguration configuration,
            IHttpContextAccessor contextAccessor,
            IEmailService emailService)
        {
            _userManager = userManager;
            _configuration = configuration;
            _signInManager = signInManager;
            _httpContext = contextAccessor.HttpContext;
            _emailService = emailService;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await _userManager.FindByNameAsync(model.Username);

            if (user == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Login failed! User don't found!");
            }

            var emailConfirmed = await _userManager.IsEmailConfirmedAsync(user);
            if (!emailConfirmed)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Login failed! In order to log in, please confirm the email.");
            }

            var checkPassword = await _userManager.CheckPasswordAsync(user, model.Password);
            if (!checkPassword)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Login failed! Wrong password!");
            }

            if (user != null && checkPassword)
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };


                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                await _userManager.RemoveAuthenticationTokenAsync(user, "Default", "RefreshToken");
                var newRefreshToken = await _userManager.GenerateUserTokenAsync(user, "Default", "RefreshToken");
                await _userManager.SetAuthenticationTokenAsync(user, "Default", "RefreshToken", newRefreshToken);


                var token = CreateToken(authClaims);
                await _userManager.UpdateAsync(user);

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                });
            }

            return Unauthorized();
        }

        [HttpGet("user-details")]
        [Authorize]
        public async Task<IActionResult> GetUserDetailsByAccesToken()
        {
            var accessToken = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", string.Empty);
            var key = Encoding.ASCII.GetBytes(_configuration["JWT:Secret"]);
            var handler = new JwtSecurityTokenHandler();
            var validations = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false
            };
            var claims = handler.ValidateToken(accessToken, validations, out var tokenSecure);

            return new ObjectResult(new
            {
                UserName = claims.FindFirstValue(ClaimTypes.Name),
                UserId = claims.FindFirstValue(ClaimTypes.NameIdentifier),
                UserRoles = claims.FindAll(x => x.Type == ClaimTypes.Role).Select(x => x.Value),
            });
        }

        [HttpPost]
        [Route("refresh-token")]
        public async Task<IActionResult> RefreshToken(string accessToken)
        {
            var principal = GetPrincipalFromExpiredToken(accessToken);
            string username = principal.Identity.Name;
            var user = await _userManager.FindByIdAsync(username);

            var refreshToken = await _userManager.GetAuthenticationTokenAsync(user, "Default", "RefreshToken");
            var isValid = await _userManager.VerifyUserTokenAsync(user, "Default", "RefreshToken", refreshToken);
            if (isValid)
            {
                var newToken = CreateToken(principal.Claims.ToList());
                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(newToken),
                    expiration = newToken.ValidTo
                });
            }
            return Unauthorized();

        }



        [HttpPost]
        [Authorize]
        [Route("logout")]
        public async Task<IActionResult> Logout()
        {
            if (User.Identity is null)
            {
                return BadRequest("Invalid user name");
            }
            else
            {
                string userName = User.Identity.Name ?? string.Empty;

                var user = await _userManager.FindByNameAsync(userName);
                if (user == null)
                {
                    return BadRequest("Invalid user name");
                }
                await _userManager.UpdateSecurityStampAsync(user);
            }
            await _signInManager.SignOutAsync();
            return Ok();
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("forgot-password")]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null || !await _userManager.IsEmailConfirmedAsync(user))
                {
                    return BadRequest("Invalid user!");
                }

                string code = await _userManager.GeneratePasswordResetTokenAsync(user);
                var callbackUrl = Url.Action("ResetPassword", "Auth", new { userId = user.Id, code = System.Web.HttpUtility.UrlEncode(code) }, protocol: Request.Scheme);
                _emailService.Send(user.Email, "Evaluation - Reset pasword", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");

                return Ok();
            }

            return BadRequest("Something wrong!");
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("reset-password")]
        public async Task<ActionResult> ResetPassword(ResetPasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Something wrong!");
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return BadRequest("Invalid user!");
            }
            var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);
            if (!result.Succeeded)
            {
                return BadRequest("Something wrong!");
            }

            return Ok("Password reset successfully!");
        }

        private JwtSecurityToken CreateToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
            _ = int.TryParse(_configuration["JWT:TokenValidityInMinutes"], out int tokenValidityInMinutes);

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.UtcNow.AddMinutes(tokenValidityInMinutes),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return token;
        }
        private ClaimsPrincipal? GetPrincipalFromExpiredToken(string? token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"])),
                ValidateLifetime = false,
                //ClockSkew = TimeSpan.Zero
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
            if (securityToken is not JwtSecurityToken jwtSecurityToken
                    || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            return principal;
        }

    }
}
