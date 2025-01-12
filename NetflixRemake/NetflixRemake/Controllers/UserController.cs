using Microsoft.AspNetCore.Mvc;
using Services.Users;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(
            IUserService userService,
            ILogger<MovieController> logger)
        {
            _userService = userService;
        }

        [HttpGet("getUser")]
        public async Task<IActionResult> GetUser(string id)
        {
            var user = await _userService.GetUser(id);
            return Ok(user);
        }

        [HttpGet("getUsers")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userService.GetUsers();
            return Ok(users);
        }

        [HttpGet("getUserMovies")]
        public async Task<IActionResult> GetUserMovies(string userId)
        {
            var users = await _userService.GetUserMovies(userId);
            return Ok(users);
        }

        [HttpPut("addToBank")]
        public async Task<IActionResult> AddToBank(float amount, string id)
        {
            await _userService.AddToBank(amount, id);
            return Ok("Amount succesfully added");
        }

        [HttpPut("buyMovie")]
        public async Task<IActionResult> BuyMovie(string userId, int movieId)
        {
            await _userService.BuyMovie(userId, movieId);
            return Ok("Movie successfully bought");
        }

        [HttpPut("sellMovie")]
        public async Task<IActionResult> SellMovie(string userId, int movieId)
        {
            await _userService.SellMovie(userId, movieId);
            return Ok("Movie successfully sold");
        }
    }
}
