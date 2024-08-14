using Genesis.CoreApi.Shared.Cryptography;
using Genesis.Shared.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Genesis.CoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        private IConfiguration _config;
        private Genesis.CoreApi.Repository.IAuthRepository _authRepository;
        private Genesis.CoreApi.Repository.IUserRolesRepository _userRolesRepository;
        private readonly ISymmetricCryptographer _cryptographer;
        public AuthController(IConfiguration config, Genesis.CoreApi.Repository.IAuthRepository authRepository, Genesis.CoreApi.Repository.IUserRolesRepository userRolesRepository, ISymmetricCryptographer cryptographer)
        {
            _config = config;
            _authRepository = authRepository;
            _userRolesRepository = userRolesRepository;
            _cryptographer = cryptographer;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Genesis.Shared.Users.Login user, CancellationToken cancellationToken)
        {

            try
            {
                string ps = _cryptographer.Encrypt(user.Password);
                var controlUser = _authRepository.Find(user.Username, ps);

                if (controlUser != null)
                {
                    int userId = controlUser.UserId;
                    var userRoles = _userRolesRepository.GetRoles(userId);

                    var authClaims = new List<Claim>
                    {
                        new Claim(JwtRegisteredClaimNames.Sub,controlUser.UserName),
                        new Claim(JwtRegisteredClaimNames.Name,controlUser.GetName()),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                    };

                    string[] roles = new string[userRoles.Count];
                    int i = 0;
                    foreach (var userRole in userRoles)
                    {
                        authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                        roles[i] = userRole;
                        i++;
                    }
                    var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:Key"]));
                    var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
                    var tokeOptions = new JwtSecurityToken(
                        issuer: _config["JWT:Issuer"],
                        audience: _config["JWT:Issuer"],
                        claims: authClaims,
                        expires: DateTime.Now.AddMinutes(20),
                        signingCredentials: signinCredentials
                    );
                    var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
                    var refreshToken = GenerateRefreshToken();
                    _authRepository.UpdateUserRefreshTokens(controlUser.UserName, controlUser.RefreshToken, refreshToken);
                    UserLogin User = new()
                    {
                        UserId = userId,
                        Username = controlUser.UserName,
                        Email = controlUser.Email,
                        DisplayName = controlUser.GetName(),
                        Access_Token = tokenString,
                        Refresh_Token = refreshToken,
                        Roles = roles,
                    };
                    return Ok(User);
                }
                else
                {
                    return Problem(
                       detail: "Sistemde böyle bir kullanıcı yoktur.",
                       title:"Kayıt bulunamadı");

                }
            }
            catch (Exception ex)
            {
                return Problem(
                    detail: ex.StackTrace,
                    title: ex.Message);
            }

           
        }

        [HttpPost("reflesh")]
        public async Task<IActionResult> Reflesh([FromBody] Genesis.CoreApi.DataModels.RefleshTokens refleshToken, CancellationToken cancellationToken)
        {
            
            try
            {
                var controlUser = _authRepository.FindByRefleshToken(refleshToken.Username, refleshToken.Refresh_Token);
                if (controlUser != null)
                {
                    int userId = controlUser.UserId;
                    var userRoles = _userRolesRepository.GetRoles(userId);

                    var authClaims = new List<Claim>
                    {
                        new Claim(JwtRegisteredClaimNames.Sub,controlUser.UserName),
                        new Claim(JwtRegisteredClaimNames.Name,controlUser.GetName()),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                    };

                    string[] roles = new string[userRoles.Count];
                    int i = 0;
                    foreach (var userRole in userRoles)
                    {
                        authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                        roles[i] = userRole;
                        i++;
                    }


                    var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:Key"]));
                    var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
                    var tokeOptions = new JwtSecurityToken(
                        issuer: _config["JWT:Issuer"],
                        audience: _config["JWT:Issuer"],
                        claims: authClaims,
                        expires: DateTime.Now.AddMinutes(20),
                        signingCredentials: signinCredentials
                    );
                    var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
                    var refreshToken = GenerateRefreshToken();
                    _authRepository.UpdateUserRefreshTokens(refleshToken.Username, refleshToken.Refresh_Token, refreshToken);


                    UserLogin User = new()
                    {

                        UserId = controlUser.UserId,
                        Username = controlUser.UserName,
                        Email = controlUser.Email,
                        DisplayName = controlUser.GetName(),
                        Access_Token = tokenString,
                        Refresh_Token = refreshToken,
                        Roles = roles
                    };
                    return Ok(User);

                }
                else
                {
                    return Problem(
                        detail: "Sistemde böyle bir kullanıcı yoktur.",
                        title: "Kayıt bulunamadı");
                }
            
            }
            catch(Exception ex)
            {
                return Problem(
                   detail: ex.StackTrace,
                   title: ex.Message);
            }

           
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromQuery] string username, [FromQuery]  string refresh_token, CancellationToken cancellationToken)
        {
            try
            {
                var controlUser = _authRepository.FindByRefleshToken(username, refresh_token);
                if (controlUser != null)
                {
                    var claims = new[] {
                    new Claim(JwtRegisteredClaimNames.Sub,controlUser.UserName),
                    new Claim(JwtRegisteredClaimNames.Name,controlUser.GetName()),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

                    var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:Key"]));
                    var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
                    var tokeOptions = new JwtSecurityToken(
                        issuer: _config["JWT:Issuer"],
                        audience: _config["JWT:Issuer"],
                        claims: claims,
                        expires: DateTime.Now.AddMinutes(20),
                        signingCredentials: signinCredentials
                    );
                    var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
                    _authRepository.UpdateUserRefreshTokens(username, refresh_token,"");
                    return Ok();
                }
            }
            catch
            {
                throw;
            }

            return Unauthorized();
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }
    }
}
