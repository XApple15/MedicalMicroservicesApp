using AuthService.Models.DTO;
using AuthService.Models.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using AuthService.Factories;

namespace AuthService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILoginResponseFactory _loginResponseFactory;

        public AuthController(UserManager<IdentityUser> userManager, ILoginResponseFactory loginResponseFactory)
        {
            _userManager = userManager;
            _loginResponseFactory = loginResponseFactory;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDTO registerRequestDTO)
        {
            var identityUser = new IdentityUser()
            {
                Email = registerRequestDTO.Email,
                UserName = registerRequestDTO.UserName,
                PhoneNumber = registerRequestDTO.PhoneNumber,
            };

            var identityResult = await _userManager.CreateAsync(identityUser, registerRequestDTO.Password);

            if (identityResult.Succeeded && registerRequestDTO.Roles?.Any() == true)
            {
                identityResult = await _userManager.AddToRolesAsync(identityUser, registerRequestDTO.Roles);
                if (identityResult.Succeeded)
                    return Ok("User created successfully");
            }

            return BadRequest(identityResult.Errors);
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginRequestDTO)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(loginRequestDTO.Email);
                if (user != null && await _userManager.CheckPasswordAsync(user, loginRequestDTO.Password))
                {
                    var roles = await _userManager.GetRolesAsync(user);
                    if (roles != null && roles.Contains(loginRequestDTO.Role))
                    {
                        var response = _loginResponseFactory.Create(user, roles);
                        return Ok(response);
                    }
                }
                return BadRequest("Invalid login attempt");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("test")]
        public IActionResult Test()
        {
            return Ok("Auth Service is running");
        }
    }
}
