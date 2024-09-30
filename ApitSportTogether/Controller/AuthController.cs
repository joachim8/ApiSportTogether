using ApiSportTogether.model.dbContext;
using ApiSportTogether.model.ObjectContext;
using ApiSportTogether.SignalR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace ApiSportTogether.Controller
{
    [ApiController]
    [Route("ApiSportTogether/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly SportTogetherContext _context;
        private readonly IConfiguration _configuration;
       
        public AuthController(IConfiguration configuration, SportTogetherContext context)
        {
            _context = context;
            _configuration = configuration;
            
        }
        // POST ApiSportTogether/auth/login
        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] UserCredentials credentials)
        {
            Utilisateur? utili = _context.Utilisateurs.Where(u => u.Pseudo == credentials.Pseudo).FirstOrDefault();

            if (utili != null)
            {
                bool bConnect = ValidateUser(credentials);
                if (bConnect)
                {
                    string token = GenerateJwtToken(utili.UtilisateursId);
                    Response.Cookies.Append("AuthToken", token, new CookieOptions { HttpOnly = true });
                    utili.EnLigne = true;
                    _context.Entry(utili).State = EntityState.Modified;
                    try
                    {
                        await _context.SaveChangesAsync();
                        
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!_context.Utilisateurs.Any(u => u.UtilisateursId == utili.UtilisateursId))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                    return Ok(utili);
                }
                else
                {
                    return NotFound("Votre login ou mot de passe n'est pas valide.");
                }
            }
            else
            {
                return NotFound("Votre login ou mot de passe n'est pas valide.");
            }
        }

        private bool ValidateUser(UserCredentials credentials)
        {
            bool IsValid = false;
            Utilisateur? utili = _context.Utilisateurs.Where(u => u.Pseudo == credentials.Pseudo).FirstOrDefault();
            if (utili != null)
            {
                IsValid = VerifyPassword(utili.MotDePasse, credentials.Password);
                utili.EnLigne = true;
            }

            return IsValid;
        }

        private string GenerateJwtToken(int userId)
        {
            string sessionIdentifier = Guid.NewGuid().ToString(); // Identifiant unique de session

            SymmetricSecurityKey securityKey = new(Encoding.UTF8.GetBytes("SportTogetherJoachimAAAAAAAAAAAAAAAAAAAAAAAA"));
            SigningCredentials credentials = new(securityKey, SecurityAlgorithms.HmacSha256);

            Claim[] claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, sessionIdentifier), // JTI (JWT ID) pour l'unicité
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString(), ClaimValueTypes.DateTime)
            };

            JwtSecurityToken token = new(
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: credentials,
                claims: claims
            );

            return new JwtSecurityTokenHandler().WriteToken(token);

        }
        private bool VerifyPassword(string hashedPassword, string providedPassword)
        {
            var hasher = new PasswordHasher<object>();
            string mdp = hasher.HashPassword(null, providedPassword);
            var result = hasher.VerifyHashedPassword(null, hashedPassword, providedPassword);
            return result == PasswordVerificationResult.Success;
        }
        
    }

    public class UserCredentials
    {
        public required string Pseudo { get; set; }
        public required string Password { get; set; }
    }
}
