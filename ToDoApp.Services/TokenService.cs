using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ToDoApp.DataAccess;
using ToDoApp.Models.ToDoClass.DTO;

namespace ToDoApp.Services
{
    public interface IJwtTokenService
    {
        string GenerateToken(User user);
        Task<string> CreateUser(RegisterDto user);
        Task<object?> LoginAsync(LoginDto dto);
        string GenerateRefreshToken(User user);
        ClaimsPrincipal? ValidateJwt(string token);
        Task<User> GetUser(int userId);
    }

    public class JwtTokenService : IJwtTokenService
    {
        private readonly JwtSettings _settings;
        private readonly ITokenDataAccess _repository;

        public JwtTokenService(IOptions<JwtSettings> opts, ITokenDataAccess repository)
        {
            _settings = opts.Value;
            _repository=repository;
        }

        private string HashPassword(string password)
        {
            using var sha = System.Security.Cryptography.SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }

        public string GenerateToken(User user)
        {
            var claims = new List<Claim>
            {

                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.Secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _settings.Issuer,
                audience: _settings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_settings.ExpiryMinutes),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }



        public ClaimsPrincipal? ValidateJwt(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_settings.Secret);

            try
            {
                var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = _settings.Issuer,
                    ValidAudience = _settings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                return principal;
            }
            catch
            {
                return null;
            }
        }
        public async Task<User> GetUser(int userId)
        {
            var user = await _repository.GetUserById(userId);
            return user; 
        }
        public string GenerateRefreshToken(User user)
        {
            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, user.UserName),
        new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
        new Claim("token_type", "refresh") 
    };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.Secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _settings.Issuer,
                audience: _settings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddDays(7), // longer expiry
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }



        public async Task<string> CreateUser(RegisterDto userDto)
        {
            var hashedPassword = HashPassword(userDto.Password);

            var user = new User
            {
                UserName = userDto.UserName,
                Password = hashedPassword,
                CreatedDate = DateTime.UtcNow
            };

            await _repository.CreateNewUser(user);
            return "User created successfully";
        }

        public async Task<object?> LoginAsync(LoginDto dto)
        {
            var user = await _repository.GetUserByUsernameAsync(dto.UserName);
            if (user == null) return null;

            var hashedPassword = HashPassword(dto.Password);
            if (user.Password != hashedPassword) return null;

            var accessToken = GenerateToken(user);       // short-lived
            var refreshToken = GenerateRefreshToken(user); // long-lived

            return new
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }
    }
}
