using Backend.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NetflixRemake.Email;
using static Backend.Auth.AccountModel;

namespace Backend.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {

        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IEmailService _emailService;
        private readonly IBasicRegisterMethods _basicRegisterMethods;

        public AccountController(
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            SignInManager<IdentityUser> signInManager,
            IConfiguration configuration,
            IEmailService emailService,
            IBasicRegisterMethods basicRegisterMethods)

        {
            _userManager = userManager;
            _roleManager = roleManager;
            _emailService = emailService;
            _basicRegisterMethods = basicRegisterMethods;
        }
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            var userExists = await _userManager.FindByEmailAsync(model.Email);

            if (userExists != null)
            {
                return StatusCode(StatusCodes.Status409Conflict, "User already exists!");
            }

            var user = new IdentityUser
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Email,
                EmailConfirmed = false,
            };
            string password = _basicRegisterMethods.GenerateRandomPassword();
            string passwordHash = _userManager.PasswordHasher.HashPassword(user, password);
            user.PasswordHash = passwordHash;

            var result = await _userManager.CreateAsync(user, password);

            if (!result.Succeeded)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "User creation failed! Please check user details and try again.");
            }
            foreach (var role in model.Roles)
            {
                if (await _roleManager.RoleExistsAsync(role))
                {
                    await _userManager.AddToRoleAsync(user, role);
                }
            }

            string code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userEmail = user.Email, code = code }, protocol: Request.Scheme);
            string html = $"To successfully log in, please first confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>\n Your password: " + password;

            _emailService.Send(model.Email, "Confirm your account", html);


            return Ok("User created successfully!");
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("confirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string userEmail, string code)
        {
            //click confirm - User Active
            if (userEmail != null && code != null)
            {
                var user = await _userManager.FindByEmailAsync(userEmail);

                if (user == null)
                {
                    return BadRequest();
                }

                var result = await _userManager.ConfirmEmailAsync(user, code);
                if (result.Succeeded)
                {
                    return Ok("Email confirmed");
                }
            }

            return BadRequest();
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("createRole")]
        public async Task<IActionResult> CreateRole(string roleName)
        {
            var role = new IdentityRole
            {
                Name = roleName
            };
            await _roleManager.CreateAsync(role);
            return Ok();
        }
    }
}
