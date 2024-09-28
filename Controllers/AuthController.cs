using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BCrypt.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations; // Include Data Annotations
using TodoApp.Data;
using TodoApp.Dtos;

namespace TodoApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly TodoContext _context;
        private readonly IConfiguration _configuration;

        public AuthController(TodoContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // POST api/auth/signup
        [HttpPost("signup")]
        public async Task<IActionResult> SignUp([FromBody] SignUpModel signUpModel)
        {
            // Check if the model is valid
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Return 400 with validation errors
            }

            // Check if the username or email already exists
            if (await _context.Users.AnyAsync(u => u.Username == signUpModel.Username || u.Email == signUpModel.Email))
            {
                return BadRequest(new { message = "Username or Email already exists." });
            }

            // Create a new user with hashed password
            var user = new User
            {
                Username = signUpModel.Username,
                Email = signUpModel.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(signUpModel.Password),
                CreatedAt = DateTime.UtcNow
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(new { message = "User registered successfully" });
        }

        // POST api/auth/signin
        [HttpPost("signin")]
        public async Task<IActionResult> SignIn([FromBody] SignInModel signInModel)
        {
            // Check if the model is valid
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Return 400 with validation errors
            }

            // Find user by username
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Username == signInModel.Username);

            // Verify the user exists and the password is correct
            if (user == null || !BCrypt.Net.BCrypt.Verify(signInModel.Password, user.PasswordHash))
            {
                return Unauthorized(new { message = "Invalid username or password" });
            }

            // Generate JWT token
            var token = GenerateJwtToken(user);

            return Ok(new { token });
        }

        // Private method to generate JWT token
        private string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(1), // Token validity
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
